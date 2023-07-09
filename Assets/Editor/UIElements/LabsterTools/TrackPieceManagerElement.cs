using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class TrackPieceManagerElement : BaseManager
{
    private const string TRACK_PIECES_PATH = "Assets/Assigned/TrackPieces/";

    private GameObject currentPieceGhost = null;

    private VisualElement root;
    private GroupBox mainGroup;
    private GroupBox pieceGroup;
    private GroupBox newPieceGroup;

    private ObjectField pieceObjectField;

    public override void Disable()
    {
        DestroyGhost();

        pieceObjectField.value = null;
    }

    public override void Initialize(VisualElement root)
    {
        this.root = root;

        CreateTitle();
        CreateMainElementsGroup();

        CreateTopElements();
        CreatePieceGroup();

        CreateNewPieceGroup();
        CreateNewPiecelements();

        root.Add(mainGroup);
    }

    private void CreateTitle()
    {
        Label title = new Label();
        title.text = "Track Piece Manager";
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

        pieceObjectField = new ObjectField();
        pieceObjectField.objectType = typeof(TrackPieceScriptable);
        pieceObjectField.style.maxWidth = 300;
        pieceObjectField.label = "Piece";
        pieceObjectField.tooltip = "Drag or select the piece you want to work on.";
        pieceObjectField.RegisterValueChangedCallback((evt) =>
        {
            CreatePieceGroup();
        });

        Button button = new Button();
        button.text = "Create New Piece";
        button.tooltip = "Press to create a new piece.";
        button.clicked += () =>
        {
            root.Remove(mainGroup);
            root.Add(newPieceGroup);
        };

        mainGroup.Add(topGroup);
        topGroup.Add(pieceObjectField);
        topGroup.Add(button);
    }

    private void CreatePieceGroup()
    {
        DestroyGhost();
        if (pieceGroup != null && mainGroup.Contains(pieceGroup))
            mainGroup.Remove(pieceGroup);
        if (pieceObjectField.value == null)
            return;

        pieceGroup = new GroupBox();
        pieceGroup.style.flexDirection = FlexDirection.Column;
        pieceGroup.style.alignItems = Align.Center;
        pieceGroup.style.justifyContent = Justify.Center;

        TrackPieceScriptable piece = (TrackPieceScriptable)pieceObjectField.value;

        ObjectField prefabObjectField = new ObjectField();
        prefabObjectField.objectType = typeof(GameObject);
        prefabObjectField.style.maxWidth = 300;
        prefabObjectField.label = "Piece Prefab";
        prefabObjectField.tooltip = "Drag or select the prefab you want to use.";
        prefabObjectField.value = piece.Prefab;
        prefabObjectField.RegisterValueChangedCallback((evt) =>
        {
            piece.Prefab = (GameObject)evt.newValue;
            EditorUtility.SetDirty(piece);
            InstantiateGhost();
        });
        pieceGroup.Add(prefabObjectField);

        ObjectField iconObjectField = new ObjectField();
        iconObjectField.objectType = typeof(Sprite);
        iconObjectField.style.maxWidth = 300;
        iconObjectField.label = "Piece Icon";
        iconObjectField.tooltip = "Drag or select the icon you want to use.";
        iconObjectField.value = piece.Icon;
        iconObjectField.RegisterValueChangedCallback((evt) =>
        {
            piece.Icon = (Sprite)evt.newValue;
            EditorUtility.SetDirty(piece);
        });
        pieceGroup.Add(iconObjectField);

        mainGroup.Add(pieceGroup);
        InstantiateGhost();
    }

    private void CreateNewPieceGroup()
    {
        newPieceGroup = new GroupBox();
        newPieceGroup.style.flexDirection = FlexDirection.Column;
        newPieceGroup.style.alignItems = Align.Center;
        newPieceGroup.style.justifyContent = Justify.Center;
    }

    private void CreateNewPiecelements()
    {
        Label label = new Label();
        label.text = "";
        label.style.color = Color.red;

        TextField textField = new TextField();
        textField.style.maxWidth = 300;
        textField.label = "New piece name";
        textField.tooltip = "Enter the name of the new piece.";
        textField.value = "New Piece";
        textField.maxLength = 50;
        textField.RegisterValueChangedCallback((evt) =>
        {
            label.text = evt.newValue.Length == 0 ? "The name of the piece cannot be empty." : "";
        });

        Button acceptButton = new Button();
        acceptButton.text = "Accept";
        acceptButton.tooltip = "Press to finish the creation of a new piece.";
        acceptButton.clicked += () =>
        {
            if (textField.value.Length == 0)
                return;

            CreateNewPiece(textField.value);
        };

        Button backButton = new Button();
        backButton.text = "Back";
        backButton.tooltip = "Press to go back to the main menu.";
        backButton.style.position = Position.Absolute;
        backButton.style.top = 0;
        backButton.style.right = 0;
        backButton.clicked += () =>
        {
            root.Remove(newPieceGroup);
            root.Add(mainGroup);
        };

        newPieceGroup.Add(textField);
        newPieceGroup.Add(acceptButton);
        newPieceGroup.Add(label);
        newPieceGroup.Add(backButton);
    }

    private void CreateNewPiece(string pieceName)
    {
        TrackPieceScriptable track = ScriptableObject.CreateInstance<TrackPieceScriptable>();
        track.name = pieceName;
        AssetDatabase.CreateAsset(track, $"{TRACK_PIECES_PATH}{pieceName}.asset");
        AssetDatabase.SaveAssets();

        pieceObjectField.value = track;
        root.Remove(newPieceGroup);
        root.Add(mainGroup);
    }

    private void InstantiateGhost()
    {
        DestroyGhost();
        TrackPieceScriptable piece = (TrackPieceScriptable)pieceObjectField.value;
        if (piece.Prefab == null)
            return;

        currentPieceGhost = Object.Instantiate(piece.Prefab);
        Selection.objects = new Object[] { currentPieceGhost };
    }

    private void DestroyGhost()
    {
        if (currentPieceGhost != null)
            Object.DestroyImmediate(currentPieceGhost);
    }
}