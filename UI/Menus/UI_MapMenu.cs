using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapMenu : UI_BaseClass
{
    private UI_LevelPreview uI_LevelPreview;

    private GameObject mapMenuElementsContainer;
    private Button mapMenu_BackButton;
    private Button mapMenu_UpgradeMenuNavButton;
    private Button mapMenu_StatsMenuNavButton;
    private Button mapMenu_SettingsMenuNavButton;
    private Button mapMenu_levelButton_1;
    private Button mapMenu_TutorialNavButton;

    protected override void Awake()
    {
        base.Awake();

        mapMenuElementsContainer.SetActive(false);
        uI_LevelPreview.gameObject.SetActive(false);
    }

    public void StartGame(GameModes gameMode) => uI_MenuController.StartGame(gameMode);

    public void SetLevelDifficulty(Difficulty normal) => uI_MenuController.SetLevelDifficulty(normal);

    public void ToggleMapMenuElementsContainer() => mapMenuElementsContainer.SetActive(!mapMenuElementsContainer.activeInHierarchy);

    public void ToggleLevelPreview() => uI_LevelPreview.gameObject.SetActive(!uI_LevelPreview.gameObject.activeInHierarchy);

    public void OnGameSceneLoaded()
    {
        mapMenuElementsContainer.SetActive(false);
        uI_LevelPreview.gameObject.SetActive(false);
    }

    public void OnGameSceneUnloaded()
    {
        if (!mapMenuElementsContainer.activeInHierarchy)
            mapMenuElementsContainer.SetActive(true);

        if (uI_LevelPreview.gameObject.activeInHierarchy)
            uI_LevelPreview.gameObject.SetActive(false);
    }

    protected override void GetObjectsAndButtons()
    {
        var canvas = transform.Find("canvas");
        uI_LevelPreview = canvas.Find("levelPreview").GetComponent<UI_LevelPreview>();

        mapMenuElementsContainer = canvas.Find("mapMenuElementsContainer").gameObject;
        mapMenu_BackButton = mapMenuElementsContainer.transform.Find("table").Find("backButton").GetComponent<Button>();
        mapMenu_UpgradeMenuNavButton = mapMenuElementsContainer.transform.Find("upgradeNavButton").GetComponent<Button>();
        mapMenu_StatsMenuNavButton = mapMenuElementsContainer.transform.Find("statsNavButton").GetComponent<Button>();
        mapMenu_SettingsMenuNavButton = mapMenuElementsContainer.transform.Find("settingsNavButton").GetComponent<Button>();
        mapMenu_levelButton_1 = mapMenuElementsContainer.transform.Find("table").Find("level (1)").GetComponent<Button>();
        mapMenu_TutorialNavButton = mapMenuElementsContainer.transform.Find("tutorialNavButton").GetComponent<Button>();
    }

    protected override void AddListeners()
    {
        mapMenu_BackButton.onClick.AddListener(ToggleMainMenu);
        mapMenu_BackButton.onClick.AddListener(ToggleMapMenuElementsContainer);

        mapMenu_UpgradeMenuNavButton.onClick.AddListener(ToggleUpgradeMenu);
        mapMenu_UpgradeMenuNavButton.onClick.AddListener(ToggleMapMenuElementsContainer);

        mapMenu_StatsMenuNavButton.onClick.AddListener(ToggleStatsMenu);
        mapMenu_StatsMenuNavButton.onClick.AddListener(ToggleMapMenuElementsContainer);

        mapMenu_SettingsMenuNavButton.onClick.AddListener(ToggleSettingsMenu);

        mapMenu_levelButton_1.onClick.AddListener(ToggleLevelPreview);
        mapMenu_levelButton_1.onClick.AddListener(delegate { SetLevelIndex(1); });

        mapMenu_TutorialNavButton.onClick.AddListener(OnTutorialToggled);
    }

    private void OnTutorialToggled() => uI_MenuController.ToggleTutorial();

    private void SetLevelIndex(int levelIndex) => uI_MenuController.SetLevelIndex(levelIndex);

    private void ToggleSettingsMenu() => uI_MenuController.ToggleSettingsMenu();

    private void ToggleStatsMenu() => uI_MenuController.ToggleStatsMenu();

    private void ToggleUpgradeMenu() => uI_MenuController.ToggleUpgradeMenu();

    private void ToggleMainMenu() => uI_MenuController.ToggleMainMenu();

}
