using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneratorUIManager : MonoBehaviour {
    private enum SetColorTo { Plane, Road, StartingTile, FinishTile, Foreground, Background}
    private SetColorTo setColor;
    private MapGenerator map;
    [SerializeField] private GameObject backGroundPanel;
    [SerializeField] private GameObject customizationButtons;
    [SerializeField] private GameObject customizationFeatiures;
    [SerializeField] private GameObject backButton;

    [Header("Customize tile size")]
    [SerializeField] private GameObject roadCustomizatioFeatures;
    [SerializeField] private Text outLinePercentText;

    [Header("Change Color")]
    [SerializeField] private GameObject changeColorButton;
    [SerializeField] private GameObject colorFeatures;
    [SerializeField] private ColorPicker colorPickerWindow;

    private bool changeRoadSize = true;

    private void Start() {
        if (map == null) {
            map = FindObjectOfType<MapGenerator>();
        }
        DiactivateAll();
    }

    #region Color Picker
    private void FixedUpdate() {
        if (!colorPickerWindow.gameObject.activeInHierarchy) {
            return;
        }

        switch (setColor) {
            case SetColorTo.Plane:
                map.Map.customizeMap.planeColor = colorPickerWindow.ReturnColor();
                map.ChangeColorMap();
                break;
            case SetColorTo.Road:
                map.Map.customizeMap.roadColor = colorPickerWindow.ReturnColor();
                map.ChangeColorMap();
                break;
            case SetColorTo.StartingTile:
                map.Map.customizeMap.spawnTileColor = colorPickerWindow.ReturnColor();
                map.SetStartTileColor();
                break;
            case SetColorTo.FinishTile:
                map.Map.customizeMap.finishTileColor = colorPickerWindow.ReturnColor();
                map.SetFinishTileColor();
                break;
            case SetColorTo.Foreground:
                map.Map.customizeMap.foreGroundColor = colorPickerWindow.ReturnColor();
                break;
            case SetColorTo.Background:
                map.Map.customizeMap.backGroundColor = colorPickerWindow.ReturnColor();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ActivateColorFeatures() {
        CustomizationButtons(false);
        FeatureButtons(true);
        colorFeatures.SetActive(true);
    }

    public void PlaneColor() {
        colorPickerWindow.gameObject.SetActive(true);
        setColor = SetColorTo.Plane;
    }

    public void RoadColor() {
        colorPickerWindow.gameObject.SetActive(true);
        setColor = SetColorTo.Road;
    }

    public void StartTileColor() {
        colorPickerWindow.gameObject.SetActive(true);
        setColor = SetColorTo.StartingTile;
    }

    public void FinishTileColor() {
        colorPickerWindow.gameObject.SetActive(true);
        setColor = SetColorTo.FinishTile;
    }

    public void ForeGroundColor() {
        colorPickerWindow.gameObject.SetActive(true);
        setColor = SetColorTo.Foreground;
    }

    public void BackGroundColor() {
        colorPickerWindow.gameObject.SetActive(true);
        setColor = SetColorTo.Background;
    }

    public void Blend() {
        map.BlendColors();
    }
    #endregion

    #region Road Customization
    //Activates road customizxation features
    public void ActivateRoadCustomization() {
        CustomizationButtons(false);
        FeatureButtons(true);
        roadCustomizatioFeatures.SetActive(true);
    }

    //Toggle
    public void ChangeRoadSize(bool t) {
        changeRoadSize = t;
    }

    public void OutLinePercentSlider(float i) {
        map.ChangeTileSize(i, changeRoadSize);
        outLinePercentText.text = i.ToString("F3");
    }
    #endregion

    #region Customization And Feature buttons status
    //Sets status on customization buttons
    private void CustomizationButtons(bool t) {
        customizationButtons.SetActive(t);
        backButton.SetActive(!t);
    }

    //Sets status on feature buttons
    private void FeatureButtons(bool t) {
        customizationFeatiures.SetActive(t);
        backButton.SetActive(t);
    }
    #endregion

    #region Map Customization
    public void SetMapSizeX(int s) {
        map.Map.mapSize.x = s;
        //int.TryParse(s, out map.Map.mapSize.x);
    }

    public void SetMapSizeY(string s) {
        int.TryParse(s, out map.Map.mapSize.y);
    }
    public void SetRoadCountMin(string s) {
        int.TryParse(s, out map.Map.roadsCount.min);
    }
    public void SetRoadCountMax(string s) {
        int.TryParse(s, out map.Map.roadsCount.max);
    }
    public void SetRoadLentghMin(string s) {
        int.TryParse(s, out map.Map.roadLength.min);
    }
    public void SetRoadLentghMax(string s) {
        int.TryParse(s, out map.Map.roadLength.max);
    }
    #endregion

    #region Creates Map 
    //Creates map when button is pressed
    public void CreateMap() {
        map.CreateMap();
        DiactivateAll();
        ActivateBackground();
    }

    //Activates when first plane is created
    private void ActivateBackground() {
        backGroundPanel.SetActive(true);
        CustomizationButtons(true);
        FeatureButtons(false);
    }
    #endregion

    
    public void Back() {
        //Set active first menu
        CustomizationButtons(true);

        //Diactivate everything doesn't belong to first menu
        DiactivateAll();

        //Diactivate back button
        backButton.SetActive(false);

    }

    private void DiactivateAll() {
        FeatureButtons(false);
        roadCustomizatioFeatures.SetActive(false);
        colorFeatures.SetActive(false);
        colorPickerWindow.gameObject.SetActive(false);
    }

    //public void GeneratePath() {
    //    map.GeneratePathNodes();
    //}
}