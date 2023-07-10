using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Race : MonoBehaviour
{
    private List<GameObject> loadedPieces = new List<GameObject>();

    private TrackScriptable track;
    private TrackAnalisys analisys;

    private CarScriptable car;
    private GameObject carObject;

    [SerializeField] private Material startMaterial;
    [SerializeField] private Material endMaterial;
    [SerializeField] private Material trackMaterial;

    [SerializeField] private Transform cam;

    private float angle = 0;
    private bool isPlaying = false;


    public void LoadTrack(TrackScriptable track)
    {
        foreach (GameObject piece in loadedPieces)
            Destroy(piece);
        loadedPieces.Clear();

        this.track = track;
        analisys = new TrackAnalisys(track);

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
    }

    public void Play(CarScriptable car)
    {
        this.car = car;
        carObject = Instantiate(car.Prefab);
        carObject.transform.SetPositionAndRotation(track.StartPieceModel.position, track.StartPieceModel.rotation);
        carObject.AddComponent<Car>().Initialize(car);
        carObject.GetComponent<Renderer>().material.color = car.Color;

        carObject.transform.LookAt(analisys.stepPositions[1]);
        carObject.transform.position += 0.75f * Vector3.up;

        isPlaying = true;
    }


    private void Update()
    {
        if (!isPlaying && track)
        {
            angle += Time.deltaTime * 10;
            cam.position = analisys.center;
            cam.eulerAngles = new Vector3(35, angle, 0);
            cam.position -= cam.forward * analisys.size * 1.5f;
        }

        if (isPlaying)
        {
            cam.SetPositionAndRotation(carObject.transform.position, carObject.transform.rotation);
            cam.localEulerAngles = new Vector3(35, cam.eulerAngles.y, 0);
            cam.position -= cam.forward * 5;
        }
    }
}