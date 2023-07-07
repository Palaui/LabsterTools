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


    [MenuItem("Labster/TrackManager")]
    public static void ShowExample()
    {
        TrackManager wnd = GetWindow<TrackManager>();
        wnd.titleContent = new GUIContent("TrackManager");
    }

    public void CreateGUI()
    {
        root = rootVisualElement;

        CreateTopGroup();
        CreateTopElements();
    }


    private void CreateTopGroup()
    {
        topGroup = new GroupBox();
        topGroup.style.alignContent = Align.Center;
        topGroup.style.alignItems = Align.Center;
        topGroup.style.justifyContent = Justify.FlexStart;
        topGroup.style.flexDirection = FlexDirection.Row;
        root.Add(topGroup);
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
            Debug.Log("Create New Track");
        };
        topGroup.Add(button);
    }
}