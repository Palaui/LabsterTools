using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Labster/Data", order = 1)]
public class DataScriptable : ScriptableObject
{
    [SerializeField] private List<TrackScriptable> tracks = new List<TrackScriptable>();
    [SerializeField] private List<CarScriptable> cars = new List<CarScriptable>();


    public List<TrackScriptable> Tracks => tracks;
    public List<CarScriptable> Cars => cars;

#if UNITY_EDITOR

    public void UpdateTracks(List<TrackScriptable> tracks)
    {
        this.tracks = tracks;
    }

    public void UpdateCars(List<CarScriptable> cars)
    {
        this.cars = cars;
    }

#endif

}