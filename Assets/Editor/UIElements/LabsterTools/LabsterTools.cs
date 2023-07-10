using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class LabsterTools : EditorWindow
{
    private VisualElement root;
    private BaseManager currentManager;
    private GroupBox managerGroup;


    [MenuItem("Labster/ToolManager")]
    public static void ShowExample()
    {
        LabsterTools window = GetWindow<LabsterTools>();
        window.titleContent = new GUIContent("Tools Manager");
    }

    public void CreateGUI()
    {
        minSize = new Vector2(500, 200);

        root = rootVisualElement;
        managerGroup = new GroupBox();

        RadioButtonGroup radioGroup = new RadioButtonGroup();
        radioGroup.style.alignSelf = Align.Center;
        radioGroup.style.marginTop = 10;

        RadioButton trackManager = new RadioButton("Track Manager");
        trackManager.RegisterCallback<MouseUpEvent>(e =>
        {
            TrackManagerElement trackManagerElement = new TrackManagerElement();
            ChangeManager(trackManagerElement);
        });
        trackManager.style.marginRight = 20;
        trackManager.style.fontSize = 16;

       RadioButton trackPieceManager = new RadioButton("Track Piece Manager");
        trackPieceManager.RegisterCallback<MouseUpEvent>(e =>
        {
            TrackPieceManagerElement trackPieceManagerElement = new TrackPieceManagerElement();
            ChangeManager(trackPieceManagerElement);
        });
        trackPieceManager.style.marginRight = 20;
        trackPieceManager.style.fontSize = 16;
        RadioButton carManager = new RadioButton("Car Manager");
        carManager.RegisterCallback<MouseUpEvent>(e =>
        {
            CarManagerElement carManagerElement = new CarManagerElement();
            ChangeManager(carManagerElement);
        });
        carManager.style.fontSize = 16;

        radioGroup.Add(trackManager);
        radioGroup.Add(trackPieceManager);
        radioGroup.Add(carManager);

        root.Add(radioGroup);
        root.Add(managerGroup);
    }

    private void ChangeManager(BaseManager newManager)
    {
        currentManager?.Disable();
        while (managerGroup.childCount > 0)
            managerGroup.RemoveAt(0);

        currentManager = newManager;
        currentManager.Initialize(managerGroup);
        managerGroup.Add(currentManager);
    }
}