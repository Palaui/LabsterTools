using System;
using System.Collections.Generic;
using UnityEngine;


public class Race : MonoBehaviour
{
    private const float RISK_THRESHOLD = 0.1f;
    private const float PREVIEW_DISTANCE_MULTIPLIER = 1.5f;
    private const float DISTANCE_TO_FINISH_THRESHOLD = 2.5f;

    private List<GameObject> loadedPieces = new List<GameObject>();

    private TrackScriptable track;
    private TrackAnalisys analisys;

    private CarScriptable car;
    private GameObject carObject;
    private PrometeoCarController controller;

    private float angle = 0;
    private float startTime;
    private bool isPlaying = false;

    [SerializeField] private Material startMaterial;
    [SerializeField] private Material endMaterial;
    [SerializeField] private Material trackMaterial;

    [SerializeField] private Transform cam;


    public EventHandler<float> TimeChanged;
    public EventHandler RaceFinished;


    public void LoadTrack(TrackScriptable track, TrackAnalisys analisys)
    {
        foreach (GameObject piece in loadedPieces)
            Destroy(piece);
        loadedPieces.Clear();

        if (track == null)
            return;

        this.track = track;
        this.analisys = analisys;

        for (int i = 0; i < track.PieceModels.Count; i++)
        {
            TrackPieceModel model = track.PieceModels[i];
            GameObject spawnedPiece = Instantiate(model.piece.Prefab);
            spawnedPiece.transform.SetPositionAndRotation(model.position, model.rotation);
            loadedPieces.Add(spawnedPiece);

            if (model.id == track.StartPieceModel.id)
                spawnedPiece.GetComponent<Renderer>().material = startMaterial;
            else if (model.id == track.EndPieceModel.id)
                spawnedPiece.GetComponent<Renderer>().material = endMaterial;
            else
                spawnedPiece.GetComponent<Renderer>().material = trackMaterial;
        }

        LoadCar(car);
    }

    public void LoadCar(CarScriptable car)
    {
        if (carObject)
            Destroy(carObject);

        if (car == null)
            return;

        this.car = car;

        if (track == null)
            return;

        carObject = Instantiate(car.Prefab);
        carObject.transform.SetPositionAndRotation(track.StartPieceModel.position, track.StartPieceModel.rotation);
        controller = carObject.GetComponent<PrometeoCarController>();
        controller.bodyRenderer.material.color = car.Color;
        controller.accelerationMultiplier = Mathf.RoundToInt(car.Acceleration);
        controller.maxSpeed = Mathf.RoundToInt(car.MaxSpeed);
        controller.steeringSpeed = car.TurnSpeed;
        controller.handbrakeDriftMultiplier = Mathf.RoundToInt(EditorConstants.CAR_MAX_GRIP - car.Grip);
        controller.enabled = false;


        carObject.transform.LookAt(analisys.stepPositions[1]);
        carObject.transform.position += 0.75f * Vector3.up;
    }

    public void Play()
    {
        isPlaying = true;
        startTime = Time.realtimeSinceStartup;
        controller.enabled = true;
    }

    public void Stop()
    {
        isPlaying = false;
        LoadTrack(track, new TrackAnalisys(track));
        LoadCar(car);
    }


    private void Update()
    {
        if (!isPlaying && track)
        {
            angle += Time.deltaTime * 10;
            cam.position = analisys.center;
            cam.eulerAngles = new Vector3(35, angle, 0);
            cam.position -= PREVIEW_DISTANCE_MULTIPLIER * analisys.size * cam.forward;
        }

        if (isPlaying)
        {
            float time = Time.realtimeSinceStartup - startTime;
            TimeChanged?.Invoke(this, time);

            cam.SetPositionAndRotation(carObject.transform.position, carObject.transform.rotation);
            cam.localEulerAngles = new Vector3(35, cam.eulerAngles.y, 0);
            cam.position -= cam.forward * 5;

            if (carObject.transform.position.y < -10)
                RaceFinished?.Invoke(this, EventArgs.Empty);

            if (Vector3.Distance(carObject.transform.position, track.EndPieceModel.position) < DISTANCE_TO_FINISH_THRESHOLD)
            {
                RaceFinished?.Invoke(this, EventArgs.Empty);

                if (!PlayerPrefs.HasKey(track.name))
                    PlayerPrefs.SetFloat(track.name, time);
                else if (PlayerPrefs.GetFloat(track.name) > time)
                    PlayerPrefs.SetFloat(track.name, time);
            }

            // Risk management
            if (Mathf.Abs((controller.frontLeftMesh.transform.position.y + controller.rearLeftMesh.transform.position.y) / 2 -
                (controller.frontRightMesh.transform.position.y + controller.rearRightMesh.transform.position.y) / 2) > RISK_THRESHOLD)
            {
                controller.accelerationMultiplier = Mathf.RoundToInt(car.Acceleration + car.RiskAcceleration);
            }
            else
                controller.accelerationMultiplier = Mathf.RoundToInt(car.Acceleration);
        }
    }
}