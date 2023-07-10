using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string NO_SELECTED_CAR = "No car selected";
    private const string NO_SELECTED_TRACK = "No track selected";
    private const string SELECTED_CAR = "Selected Car";
    private const string PREFERRED_CAR = "Preferred Car";

    [SerializeField] private GameObject waitingRoomUI;
    [SerializeField] private GameObject raceUI;

    [SerializeField] private Race race;
    [SerializeField] private DataScriptable data;
    [SerializeField] private CarEntryWidget carEntryWidgetPrefab;
    [SerializeField] private TrackEntryWidget trackEntryWidgetPrefab;

    [SerializeField] private Transform carsLayout;
    [SerializeField] private Transform tracksLayout;
    [SerializeField] private CarDataWidget carDataWidget;

    [SerializeField] private TextMeshProUGUI selectedCarText;
    [SerializeField] private TextMeshProUGUI preferredCarText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI currentTimeText;

    [SerializeField] private Button playButton;
    [SerializeField] private Button backButton;

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
        backButton.onClick.AddListener(OnBackPressed);

        race.TimeChanged += ShowCurrentTime;
        carDataWidget.Set(null);
    }

    private void OnCarPressed(object sender, CarScriptable car)
    {
        selectedCar = car;
        selectedCarText.text = $"{SELECTED_CAR}: {car.name}";
        race.LoadCar(car);
        carDataWidget.Set(car);
    }

    private void OnTrackPressed(object sender, TrackScriptable track)
    {
        selectedTrack = track;
        preferredCarText.text = $"{PREFERRED_CAR}: {track.name}";
        race.LoadTrack(track);
    }

    private void OnPlayPressed()
    {
        if (!selectedTrack || !selectedCar)
            return;

        race.Play();
        race.RaceFinished += (_, _) => { OnBackPressed(); };
        waitingRoomUI.SetActive(false);
        raceUI.SetActive(true);

        recordText.text = PlayerPrefs.HasKey(selectedTrack.name) ?
            $"Record - {FormatTime(PlayerPrefs.GetFloat(selectedTrack.name))}" :
            "Record - No record yet";
    }

    private void OnBackPressed()
    {
        race.RaceFinished = null;
        race.Stop();
        waitingRoomUI.SetActive(true);
        raceUI.SetActive(false);
    }

    private void ShowCurrentTime(object sender, float time)
    {
        currentTimeText.text = $"Timer: {FormatTime(time)}";
    }

    private string FormatTime(float time)
    {
        return $"{(int)time / 60}:" +
                $"{(int)time % 60}:" +
                $"{(int)(time * 1000) % 1000}";
    }
}