using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class TrackManager : EditorWindow
{
    private const string tracksPath = "Assets/Assigned/Tracks/";
    private const string trackPiecesPath = "Assets/Assigned/TrackPieces/";

    private VisualElement root;
    private GroupBox mainGroup;
    private GroupBox newTrackGroup;

    private ObjectField trackObjectField;
    private GroupBox piecesGrid;


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

        CreateTitle();
        CreateMainElementsGroup();
        CreateTopElements();
        CreateTrackPiecesGrid();

        CreateNewTrackGroup();
        CreateNewTracklements();

        root.Add(mainGroup);
    }


    private void CreateTitle()
    {
        Label title = new Label();
        title.text = "Track Manager";
        title.style.unityTextAlign = TextAnchor.UpperCenter;
        title.style.marginTop = 10;
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.fontSize = 20;

        root.Add(title);
    }

    private void CreateMainElementsGroup()
    {
        mainGroup = new GroupBox();
        mainGroup.style.alignContent = Align.Center;
        mainGroup.style.alignItems = Align.Center;
        mainGroup.style.justifyContent = Justify.FlexStart;
        mainGroup.style.flexDirection = FlexDirection.Column;
    }

    private void CreateTopElements()
    {
        GroupBox topGroup = new GroupBox();
        topGroup.style.alignContent = Align.Center;
        topGroup.style.alignItems = Align.Center;
        topGroup.style.justifyContent = Justify.FlexStart;
        topGroup.style.flexDirection = FlexDirection.Row;

        trackObjectField = new ObjectField();
        trackObjectField.objectType = typeof(Track);
        trackObjectField.style.maxWidth = 300;
        trackObjectField.label = "Track";
        trackObjectField.tooltip = "Drag or select the track you want to work on.";
        trackObjectField.RegisterValueChangedCallback((evt) =>
        {
            if (evt.newValue != null)
                Debug.Log(evt.newValue.name);
        });

        Button button = new Button();
        button.text = "Create New Track";
        button.tooltip = "Press to create a new track.";
        button.clicked += () =>
        {
            root.Remove(mainGroup);
            root.Add(newTrackGroup);
        };

        Button refreshButton = new Button();
        refreshButton.text = "Refresh";
        refreshButton.tooltip = "Refresh this window.";
        refreshButton.style.position = Position.Absolute;
        refreshButton.style.top = 0;
        refreshButton.style.right = 0;
        refreshButton.clicked += () =>
        {
            mainGroup.Remove(piecesGrid);
            CreateTrackPiecesGrid();
        };

        mainGroup.Add(refreshButton);
        mainGroup.Add(topGroup);
        topGroup.Add(trackObjectField);
        topGroup.Add(button);
    }

    private void CreateTrackPiecesGrid()
    {
        piecesGrid = new GroupBox();
        piecesGrid.style.flexDirection = FlexDirection.Row;
        piecesGrid.style.flexWrap = Wrap.Wrap;
        piecesGrid.style.alignItems = Align.FlexStart;
        piecesGrid.style.justifyContent = Justify.FlexStart;

        string[] assets = AssetDatabase.FindAssets($"t:{typeof(TrackPiece).Name}", new[] { trackPiecesPath });
        foreach (string id in assets)
        {
            TrackPiece trackPiece = AssetDatabase.LoadAssetAtPath<TrackPiece>(AssetDatabase.GUIDToAssetPath(id));
            if (!trackPiece)
                continue;

            Image image = new Image();
            image.style.width = 50;
            image.style.height = 50;
            image.style.marginLeft = 5;
            image.style.marginRight = 5;
            image.style.marginTop = 5;
            image.style.marginBottom = 5;
            image.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            if (trackPiece.IsCorrectlySet)
            {
                image.style.backgroundColor = Color.black;
                image.image = AssetPreview.GetAssetPreview(trackPiece.Icon);
                image.tooltip = trackPiece.name;
            }
            else
            {
                image.style.backgroundColor = Color.red;
                image.tooltip = $"{trackPiece.name}: Not correctly set.";
            }
            piecesGrid.Add(image);
        }
        
        mainGroup.Add(piecesGrid);
    }

    private void CreateNewTrackGroup()
    {
        newTrackGroup = new GroupBox();
        newTrackGroup.style.flexDirection = FlexDirection.Column;
        newTrackGroup.style.alignItems = Align.Center;
        newTrackGroup.style.justifyContent = Justify.Center;
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

        Button acceptButton = new Button();
        acceptButton.text = "Accept";
        acceptButton.tooltip = "Press to finish the creation of a new track.";
        acceptButton.clicked += () =>
        {
            if (textField.value.Length == 0)
                return;

            CreateNewTrack(textField.value);
        };

        Button backButton = new Button();
        backButton.text = "Back";
        backButton.tooltip = "Press to go back to the main menu.";
        backButton.style.position = Position.Absolute;
        backButton.style.top = 0;
        backButton.style.right = 0;
        backButton.clicked += () =>
        {
            root.Remove(newTrackGroup);
            root.Add(mainGroup);
        };

        newTrackGroup.Add(textField);
        newTrackGroup.Add(acceptButton);
        newTrackGroup.Add(label);
        newTrackGroup.Add(backButton);
    }

    private void CreateNewTrack(string trackName)
    {
        Track track = CreateInstance<Track>();
        track.name = trackName;
        AssetDatabase.CreateAsset(track, $"{tracksPath}{trackName}.asset");
        AssetDatabase.SaveAssets();

        trackObjectField.value = track;
        root.Remove(newTrackGroup);
        root.Add(mainGroup);
    }
}