using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScenePauseMenu : UI_BaseClass
{
    private UI_GameScene uI_GameScene;

    private GameObject gameSceneMenuElementsContainer;
    private Button backButton;
    private Button backToMenuNavButton;
    private Button settingsNavButton;

    public Button GameSceneMenu_BackToMenuNavButton => backToMenuNavButton;

    protected override void Awake()
    {
        base.Awake();

        uI_GameScene = transform.parent.parent.GetComponent<UI_GameScene>();
    }

    public void ToggleGameScenePauseMenu()
    {
        Helpers.SetActive_Toggle(gameSceneMenuElementsContainer);
        uI_GameScene.ToggleGamePause();
        uI_GameScene.Toggle_Is_UI_Active();
    }

    public void OnGameSceneUnloaded()
    {
        if (gameSceneMenuElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameSceneMenuElementsContainer);
    }

    public void OnGameSceneLoaded()
    {
        if (gameSceneMenuElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameSceneMenuElementsContainer);
    }

    protected override void GetObjectsAndButtons()
    {
        gameSceneMenuElementsContainer = transform.Find("canvas").Find("gameSceneMenuElementsContainer").gameObject;
        var table = gameSceneMenuElementsContainer.transform.Find("table");
        backButton = table.Find("backButton").GetComponent<Button>();
        backToMenuNavButton = table.Find("backToMenuNavButton").GetComponent<Button>();
        settingsNavButton = table.Find("settingsNavButton").GetComponent<Button>();
    }

    protected override void AddListeners()
    {
        backButton.onClick.AddListener(ToggleGameScenePauseMenu);
        backToMenuNavButton.onClick.AddListener(ToggleGameScenePauseMenu);
        backToMenuNavButton.onClick.AddListener(UnloadLevel);
        settingsNavButton.onClick.AddListener(ToggleSettingsMenu);
    }

    private void ToggleSettingsMenu() => uI_GameScene.ToggleSettingsMenu();

    private void UnloadLevel() => uI_GameScene.UnloadLevel();

}
