using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string NO_SELECTED_CAR = "No car selected";
    private const string NO_SELECTED_TRACK = "No track selected";
    private const string SELECTED_CAR = "Selected Car";
    private const string PREFERRED_CAR = "Preferred Car";

    [SerializeField] private DataScriptable data;
    [SerializeField] private CarEntryWidget carEntryWidgetPrefab;
    [SerializeField] private TrackEntryWidget trackEntryWidgetPrefab;

    [SerializeField] private Transform carsLayout;
    [SerializeField] private Transform tracksLayout;

    [SerializeField] private TextMeshProUGUI selectedCarText;
    [SerializeField] private TextMeshProUGUI preferredCarText;

    [SerializeField] private Button playButton;

    private CarScriptable selectedCar;
    private TrackScriptable selectedTrack;


    private void Start()
    {
        ButtonToggleGroup buttonToggleGroup = carsLayout.GetComponent<ButtonToggleGroup>();
        foreach (CarScriptable car in data.Cars)
        {
            CarEntryWidget carEntryWidget = Instantiate(carEntryWidgetPrefab, carsLayout);
            carEntryWidget.Set(car, buttonToggleGroup);
            carEntryWidget.Pressed += OnCarPressed;
        }

        buttonToggleGroup = tracksLayout.GetComponent<ButtonToggleGroup>();
        foreach (TrackScriptable track in data.Tracks)
        {
            TrackEntryWidget trackEntryWidget = Instantiate(trackEntryWidgetPrefab, tracksLayout);
            trackEntryWidget.Set(track, buttonToggleGroup);
            trackEntryWidget.Pressed += OnTrackPressed;
        }

        selectedCarText.text = NO_SELECTED_CAR;
        preferredCarText.text = NO_SELECTED_TRACK;

        playButton.onClick.AddListener(OnPlayPressed);
    }


    private void OnCarPressed(object sender, CarScriptable car)
    {
        selectedCar = car;
        selectedCarText.text = $"{SELECTED_CAR}: {car.name}";
    }

    private void OnTrackPressed(object sender, TrackScriptable track)
    {
        selectedTrack = track;
        preferredCarText.text = $"{PREFERRED_CAR}: {track.name}";
    }

    private void OnPlayPressed()
    {
        gameObject.SetActive(false);
    }
}