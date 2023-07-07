using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class TrackManager : EditorWindow
{
    private const string tracksPath = "Assets/Assigned/Tracks";
    private const string trackPiecesPath = "Assets/Assigned/TrackPieces";

    private VisualElement root;
    private GroupBox topGroup;
    private GroupBox newTrackroup;


    [MenuItem("Labster/TrackManager")]
    public static void ShowExample()
    {
        TrackManager wnd = GetWindow<TrackManager>();
        wnd.titleContent = new GUIContent("TrackManager");
    }

    public void CreateGUI()
    {
        minSize = new Vector2(500, 200);

        root = rootVisualElement;

        CreateTopGroup();
        CreateTopElements();

        CreateNewTrackGroup();
        CreateNewTracklements();

        root.Add(topGroup);
    }


    private void CreateTopGroup()
    {
        topGroup = new GroupBox();
        topGroup.style.alignContent = Align.Center;
        topGroup.style.alignItems = Align.Center;
        topGroup.style.justifyContent = Justify.FlexStart;
        topGroup.style.flexDirection = FlexDirection.Row;
    }

    private void CreateTopElements()
    {
        ObjectField objectField = new ObjectField();
        objectField.objectType = typeof(TrackPiece);
        objectField.style.maxWidth = 300;
        objectField.label = "Track";
        objectField.tooltip = "Drag or select the track you want to work on.";
        objectField.RegisterValueChangedCallback((evt) =>
        {
            if (evt.newValue != null)
                Debug.Log(evt.newValue.name);
        });
        topGroup.Add(objectField);

        Button button = new Button();
        button.text = "Create New Track";
        button.tooltip = "Press to create a new track.";
        button.clicked += () =>
        {
            root.Remove(topGroup);
            root.Add(newTrackroup);
        };
        topGroup.Add(button);
    }

    private void CreateNewTrackGroup()
    {
        newTrackroup = new GroupBox();
        newTrackroup.style.flexDirection = FlexDirection.Column;
        newTrackroup.style.alignItems = Align.Center;
        newTrackroup.style.justifyContent = Justify.Center;
    }

    private void CreateNewTracklements()
    {
        Label label = new Label();
        label.text = "";
        label.style.color = Color.red;

        TextField textField = new TextField();
        textField.style.maxWidth = 300;
        textField.label = "New track name";
        textField.tooltip = "Enter the name of the new track.";
        textField.value = "New Track";
        textField.maxLength = 50;
        textField.RegisterValueChangedCallback((evt) =>
        {
            if (evt.newValue.Length == 0)
                label.text = "The name of the track cannot be empty.";
            else
                label.text = "";
        });

        Button button = new Button();
        button.text = "Accept";
        button.tooltip = "Press to finish the creation of a new track.";
        button.clicked += () =>
        {
            if (textField.value.Length == 0)
                return;

            CreateNewTrack(textField.value);
        };

        newTrackroup.Add(textField);
        newTrackroup.Add(button);
        newTrackroup.Add(label);
    }

    private void CreateNewTrack(string trackName)
    {
        Track track = CreateInstance<Track>();
        track.name = trackName;
        AssetDatabase.CreateAsset(track, tracksPath + "/" + trackName + ".asset");
        AssetDatabase.SaveAssets();

        root.Remove(newTrackroup);
        root.Add(topGroup);
    }
}