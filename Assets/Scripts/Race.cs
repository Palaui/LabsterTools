using System.Collections.Generic;
using UnityEngine;


public class Race : MonoBehaviour
{
    private List<GameObject> loadedPieces = new List<GameObject>();
    private TrackScriptable track;
    private TrackAnalisys analisys;

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

    public void Play()
    {
        
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
    }
}