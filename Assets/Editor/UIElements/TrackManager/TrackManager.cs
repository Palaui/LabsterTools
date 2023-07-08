using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class TrackManager : EditorWindow
{
    private const string DATA_PATH = "Assets/Assigned/Data.asset";
    private const string TRACKS_PATH = "Assets/Assigned/Tracks/";
    private const string TRACK_PIECES_PATH = "Assets/Assigned/TrackPieces/";

    private List<GameObject> loadedPieces = new List<GameObject>();
    private GameObject currentPieceGhost = null;

    private VisualElement root;
    private GroupBox mainGroup;
    private GroupBox newTrackGroup;

    private ObjectField trackObjectField;
    private GroupBox piecesGrid;

    private TrackPieceScriptable selectedPiece = null;


    [MenuItem("Labster/TrackManager")]
    public static void ShowWindow()
    {
        TrackManager window = GetWindow<TrackManager>();
        window.titleContent = new GUIContent("TrackManager");
    }

    private void OnDisable()
    {
        DestroyGhost();
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
        UpdateAllTracks();
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
        trackObjectField.objectType = typeof(TrackScriptable);
        trackObjectField.style.maxWidth = 300;
        trackObjectField.label = "Track";
        trackObjectField.tooltip = "Drag or select the track you want to work on.";
        trackObjectField.RegisterValueChangedCallback((evt) =>
        {
            RefreshGrid();
            Load();
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
            selectedPiece = null;
            DestroyGhost();
            RefreshGrid();
        };

        mainGroup.Add(refreshButton);
        mainGroup.Add(topGroup);
        topGroup.Add(trackObjectField);
        topGroup.Add(button);
    }

    private void CreateTrackPiecesGrid()
    {
        if (trackObjectField.value == null)
        {
            selectedPiece = null;
            DestroyGhost();
            return;
        }

        piecesGrid = new GroupBox();
        piecesGrid.style.flexDirection = FlexDirection.Row;
        piecesGrid.style.flexWrap = Wrap.Wrap;
        piecesGrid.style.alignItems = Align.FlexStart;
        piecesGrid.style.justifyContent = Justify.FlexStart;

        string[] assets = AssetDatabase.FindAssets($"t:{typeof(TrackPieceScriptable).Name}", new string[] { TRACK_PIECES_PATH });
        foreach (string id in assets)
        {
            TrackPieceScriptable trackPiece = AssetDatabase.LoadAssetAtPath<TrackPieceScriptable>(AssetDatabase.GUIDToAssetPath(id));
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
                image.style.backgroundColor = selectedPiece && selectedPiece.name == trackPiece.name ? Color.green : Color.black;
                image.image = AssetPreview.GetAssetPreview(trackPiece.Icon);
                image.tooltip = trackPiece.name;

                image.RegisterCallback<MouseDownEvent>((evt) =>
                {
                    if (evt.button == 0)
                    {
                        selectedPiece = trackPiece;
                        InstantiateGhost();
                        RefreshGrid();
                    }
                });
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

    private void RefreshGrid()
    {
        if (mainGroup.Contains(piecesGrid))
            mainGroup.Remove(piecesGrid);
        CreateTrackPiecesGrid();
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
            label.text = evt.newValue.Length == 0 ? "The name of the track cannot be empty." : "";
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
        TrackScriptable track = CreateInstance<TrackScriptable>();
        track.name = trackName;
        AssetDatabase.CreateAsset(track, $"{TRACKS_PATH}{trackName}.asset");
        AssetDatabase.SaveAssets();

        trackObjectField.value = track;
        root.Remove(newTrackGroup);
        root.Add(mainGroup);

        RefreshGrid();
        UpdateAllTracks();
    }

    private void InstantiateGhost()
    {
        DestroyGhost();
        currentPieceGhost = Instantiate(selectedPiece.Prefab);
        TrackPiecePlacer placer = currentPieceGhost.AddComponent<TrackPiecePlacer>();
        placer.Initialize(trackObjectField.value as TrackScriptable, selectedPiece);
        placer.Completed += (_, _) =>
        {
            selectedPiece = null;
            RefreshGrid();
            Load();
        };

        Selection.objects = new Object[] { currentPieceGhost };
    }

    private void DestroyGhost()
    {
        if (currentPieceGhost != null)
            DestroyImmediate(currentPieceGhost);
    }

    private void Load()
    {
        DestroyGhost();

        foreach (GameObject piece in loadedPieces)
            DestroyImmediate(piece);
        loadedPieces.Clear();

        if (trackObjectField.value == null)
            return;

        TrackScriptable track = trackObjectField.value as TrackScriptable;
        if (track.PieceModels.Count == 0)
            return;

        for (int i = 0; i < track.PieceModels.Count; i++)
        {
            TrackPieceModel model = track.PieceModels[i];
            GameObject spawnedPiece = Instantiate(model.piece.Prefab);
            spawnedPiece.transform.SetPositionAndRotation(model.position, model.rotation);
            loadedPieces.Add(spawnedPiece);
            TrackPieceElement element = spawnedPiece.AddComponent<TrackPieceElement>();
            element.Initialize(track, model);
        }
    }

    private void UpdateAllTracks()
    {
        List<TrackScriptable> tracks = new List<TrackScriptable>();
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(TrackScriptable).Name}", new string[] { TRACKS_PATH });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            tracks.Add(AssetDatabase.LoadAssetAtPath<TrackScriptable>(path));
        }

        // Get Data scriptable
        DataScriptable data = AssetDatabase.LoadAssetAtPath<DataScriptable>(DATA_PATH);
        data.UpdateTracks(tracks);
    }
}