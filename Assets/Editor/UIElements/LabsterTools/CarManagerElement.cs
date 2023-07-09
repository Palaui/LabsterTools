using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;

public class CarManagerElement : BaseManager
{
    private const string DATA_PATH = "Assets/Assigned/Data.asset";
    private const string CARS_PATH = "Assets/Assigned/Cars/";

    private GameObject currentCarGhost = null;

    private VisualElement root;
    private GroupBox mainGroup;
    private GroupBox carGroup;
    private GroupBox newCarGroup;

    private ObjectField carObjectField;


    public override void Disable()
    {
        DestroyGhost();

        carObjectField.value = null;
    }

    public override void Initialize(VisualElement root)
    {
        this.root = root;

        CreateTitle();
        CreateMainElementsGroup();

        CreateTopElements();
        CreateCarGroup();

        CreateNewCarGroup();
        CreateNewCarlements();

        UpdateAllCars();

        root.Add(mainGroup);
    }


    private void CreateTitle()
    {
        Label title = new Label();
        title.text = "Car Manager";
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

        carObjectField = new ObjectField();
        carObjectField.objectType = typeof(CarScriptable);
        carObjectField.style.maxWidth = 300;
        carObjectField.label = "Car";
        carObjectField.tooltip = "Drag or select the car you want to work on.";
        carObjectField.RegisterValueChangedCallback((evt) =>
        {
            CreateCarGroup();
        });

        Button button = new Button();
        button.text = "Create New Car";
        button.tooltip = "Press to create a new car.";
        button.clicked += () =>
        {
            root.Remove(mainGroup);
            root.Add(newCarGroup);
        };

        mainGroup.Add(topGroup);
        topGroup.Add(carObjectField);
        topGroup.Add(button);
    }

    private void CreateCarGroup()
    {
        DestroyGhost();
        if (carGroup != null && mainGroup.Contains(carGroup))
            mainGroup.Remove(carGroup);
        if (carObjectField.value == null)
            return;

        carGroup = new GroupBox();
        carGroup.style.flexDirection = FlexDirection.Column;
        carGroup.style.alignItems = Align.Center;
        carGroup.style.justifyContent = Justify.Center;

        CarScriptable car = (CarScriptable)carObjectField.value;
        carGroup.Add(SetFloatField("Acceleration", "Increase of the speed of the car", car.Acceleration, (float value) => { car.Acceleration = value; }));
        carGroup.Add(SetFloatField("Max Speed", "Maximum speed of the car", car.MaxSpeed, (float value) => { car.MaxSpeed = value; }));
        carGroup.Add(SetFloatField("Turn Speed", "Speed of rotation of the car", car.TurnSpeed, (float value) => { car.TurnSpeed = value; }));
        carGroup.Add(SetFloatField("Grip", "How much speed can the car maintain when turning", car.Grip, (float value) => { car.Grip = value; }));
        carGroup.Add(SetFloatField("Risk Acceleration", "Acceleration when the car is on the edges", car.RiskAcceleration, (float value) => { car.RiskAcceleration = value; }));

        ColorField colorField = new ColorField();
        colorField.label = "Color";
        colorField.tooltip = "Color of the car";
        colorField.value = car.Color;
        colorField.RegisterValueChangedCallback((evt) =>
        {
            car.Color = evt.newValue;
            ChangeGhostColor();
        });
        carGroup.Add(colorField);

        mainGroup.Add(carGroup);

        InstantiateGhost();
    }

    private void CreateNewCarGroup()
    {
        newCarGroup = new GroupBox();
        newCarGroup.style.flexDirection = FlexDirection.Column;
        newCarGroup.style.alignItems = Align.Center;
        newCarGroup.style.justifyContent = Justify.Center;
    }

    private void CreateNewCarlements()
    {
        Label label = new Label();
        label.text = "";
        label.style.color = Color.red;

        TextField textField = new TextField();
        textField.style.maxWidth = 300;
        textField.label = "New car name";
        textField.tooltip = "Enter the name of the new car.";
        textField.value = "New Car";
        textField.maxLength = 50;
        textField.RegisterValueChangedCallback((evt) =>
        {
            label.text = evt.newValue.Length == 0 ? "The name of the car cannot be empty." : "";
        });

        Button acceptButton = new Button();
        acceptButton.text = "Accept";
        acceptButton.tooltip = "Press to finish the creation of a new car.";
        acceptButton.clicked += () =>
        {
            if (textField.value.Length == 0)
                return;

            CreateNewCar(textField.value);
        };

        Button backButton = new Button();
        backButton.text = "Back";
        backButton.tooltip = "Press to go back to the main menu.";
        backButton.style.position = Position.Absolute;
        backButton.style.top = 0;
        backButton.style.right = 0;
        backButton.clicked += () =>
        {
            root.Remove(newCarGroup);
            root.Add(mainGroup);
        };

        newCarGroup.Add(textField);
        newCarGroup.Add(acceptButton);
        newCarGroup.Add(label);
        newCarGroup.Add(backButton);
    }

    private void CreateNewCar(string carName)
    {
        CarScriptable car = ScriptableObject.CreateInstance<CarScriptable>();
        car.name = carName;
        AssetDatabase.CreateAsset(car, $"{CARS_PATH}{carName}.asset");
        AssetDatabase.SaveAssets();

        carObjectField.value = car;
        root.Remove(newCarGroup);
        root.Add(mainGroup);

        CreateCarGroup();
        UpdateAllCars();
    }

    private FloatField SetFloatField(string label, string tooltip, float value, Action<float> onChangeAction)
    {
        FloatField accelerationField = new FloatField();
        accelerationField.style.maxWidth = 300;
        accelerationField.label = label;
        accelerationField.tooltip = tooltip;
        accelerationField.value = value;
        accelerationField.RegisterCallback<ChangeEvent<float>>((evt) =>
        {
            onChangeAction(evt.newValue);
            EditorUtility.SetDirty(carObjectField.value as CarScriptable);
        });
        return accelerationField;
    }

    private void InstantiateGhost()
    {
        DestroyGhost();
        currentCarGhost = UnityEngine.Object.Instantiate((carObjectField.value as CarScriptable).Prefab);
        ChangeGhostColor();

        Selection.objects = new UnityEngine.Object[] { currentCarGhost };
    }

    private void ChangeGhostColor()
    {
        if (currentCarGhost == null || carObjectField.value == null)
            return;

        currentCarGhost.GetComponent<Renderer>().sharedMaterial.color = (carObjectField.value as CarScriptable).Color;
    }

    private void DestroyGhost()
    {
        if (currentCarGhost != null)
            UnityEngine.Object.DestroyImmediate(currentCarGhost);
    }

    private void UpdateAllCars()
    {
        List<CarScriptable> cars = new List<CarScriptable>();
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(CarScriptable).Name}", new string[] { CARS_PATH });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            cars.Add(AssetDatabase.LoadAssetAtPath<CarScriptable>(path));
        }

        // Get Data scriptable
        DataScriptable data = AssetDatabase.LoadAssetAtPath<DataScriptable>(DATA_PATH);
        data.UpdateCars(cars);
        EditorUtility.SetDirty(data);
    }

    private void MoveSceneViewCamera()
    {
        Vector3 position = SceneView.lastActiveSceneView.pivot;
        position.z -= 10.0f;
        SceneView.lastActiveSceneView.pivot = position;
        SceneView.lastActiveSceneView.Repaint();
    }
}