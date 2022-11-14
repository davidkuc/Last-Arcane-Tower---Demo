using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_BaseClass
{
    private UI_GameScenePauseMenu uI_GameScenePauseMenu;
    private UI_GameWin uI_GameWin;
    private UI_GameLose uI_GameLose;
    private SpellCooldownsUI spellCooldownsUI;

    private GameObject gameSceneUIElementsContainer;
    private GameObject wavesProgressBar;
    private GameObject gameSceneUI_CooldownsBar;
    private GameObject gameSceneUI_SpecialSpellModeScreen;
    private Button gameSceneUI_PauseButton;
    private Button gameSceneUI_SpawnWavesButton;
    private Button gameSceneUI_SpecialSpellModeButton;
    private GraphicRaycaster gameSceneUI_GraphicRaycaster;

    public GraphicRaycaster GameSceneUI_GraphicRaycaster => gameSceneUI_GraphicRaycaster;
    public Button GameSceneUI_SpawnWavesButton => gameSceneUI_SpawnWavesButton;
    public Button GameSceneUI_PauseButton => gameSceneUI_PauseButton;
    public GameObject GameSceneUI_SpecialSpellModeScreen => gameSceneUI_SpecialSpellModeScreen;
    public Button GameSceneUI_SpecialSpellModeButton => gameSceneUI_SpecialSpellModeButton;
    public UI_GameScenePauseMenu UI_GameScenePauseMenu => uI_GameScenePauseMenu;
    public SpellCooldownsUI SpellCooldownsUI => spellCooldownsUI;

    protected override void Awake() => base.Awake();

    private void OnEnable() => GameManager.Instance.GameSceneLoaded += EnableWavesBar;

    public void WavesButton_CheckIfSandboxModeIsOn()
    {
        if (GameManager.Instance.GameMode == GameModes.SandboxMode)
        gameSceneUI_SpawnWavesButton.interactable = false;
    }

    public void EnableWavesBar()
    {
        if (GameManager.Instance.GameMode == GameModes.NormalMode)
            wavesProgressBar.SetActive(true);
    }

    public void DisableWavesBar() => wavesProgressBar.SetActive(false);

    public void SetActive_SpecialSpellModeButton(bool cooldownActive) => gameSceneUI_SpecialSpellModeButton.interactable = !cooldownActive;

    public void UnloadLevel() => uI_MenuController.UnloadLevel();

    public void ToggleSettingsMenu() => uI_MenuController.ToggleSettingsMenu();

    public void RestartLevel() => uI_MenuController.RestartLevel();

    public void ToggleGameWin() => uI_GameWin.ToggleGameWin();

    public void ToggleGameLose() => uI_GameLose.ToggleGameLose();

    public void GoBackToMapMenu() => uI_MenuController.UnloadLevel();

    public void ToggleGameSceneUIElementsContainer()
    {
        EnableSpawnWavesButton();
        Helpers.SetActive_Toggle(gameSceneUIElementsContainer);
    }

    public void EnableSpawnWavesButton() => gameSceneUI_SpawnWavesButton.interactable = true;

    public void ToggleSpecialSpellMode()
    {
        Helpers.SetActive_Toggle(GameSceneUI_SpecialSpellModeScreen);
        uI_MenuController.ToggleSpecialSpellMode();
    }

    public void TogglePauseMenu() => UI_GameScenePauseMenu.ToggleGameScenePauseMenu();

    public void ToggleGamePause() => uI_MenuController.ToggleGamePause();

    public void Toggle_Is_UI_Active() => uI_MenuController.Toggle_Is_UI_Active();

    public void OnGestureRecognized() => Helpers.SetActive_Toggle(GameSceneUI_SpecialSpellModeScreen);

    public void OnGameSceneUnloaded()
    {
        if (gameSceneUIElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameSceneUIElementsContainer);

        uI_GameScenePauseMenu.OnGameSceneUnloaded();
        uI_GameWin.OnGameSceneUnloaded();
        uI_GameLose.OnGameSceneUnloaded();
    }

    public void OnGameSceneLoaded()
    {
        if (!gameSceneUIElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameSceneUIElementsContainer);

        uI_GameScenePauseMenu.OnGameSceneLoaded();
        uI_GameWin.OnGameSceneLoaded();
        uI_GameLose.OnGameSceneLoaded();
    }

    protected override void AddListeners()
    {
        GameSceneUI_PauseButton.onClick.AddListener(TogglePauseMenu);
        GameSceneUI_SpawnWavesButton.onClick.AddListener(SpawnNextWave);
        GameSceneUI_SpecialSpellModeButton.onClick.AddListener(ToggleSpecialSpellMode);
        GameSceneUI_SpawnWavesButton.onClick.AddListener(WavesButton_CheckIfSandboxModeIsOn);
    }

    protected override void GetObjectsAndButtons()
    {
        var canvas = transform.Find("canvas");
        uI_GameScenePauseMenu = canvas.Find("uI_GameScenePauseMenu").GetComponent<UI_GameScenePauseMenu>();
        gameSceneUI_GraphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        gameSceneUIElementsContainer = canvas.Find("gameSceneUIElementsContainer").gameObject;
        wavesProgressBar = gameSceneUIElementsContainer.transform.Find("WavesProgressBar").gameObject;

        uI_GameWin = canvas.transform.Find("uI_GameWin").GetComponent<UI_GameWin>();
        uI_GameLose = canvas.transform.Find("uI_GameLose").GetComponent<UI_GameLose>();

        gameSceneUI_PauseButton = gameSceneUIElementsContainer.transform.Find("pauseButton").GetComponent<Button>();
        gameSceneUI_SpawnWavesButton = gameSceneUIElementsContainer.transform.Find("spawnWaveButton").GetComponent<Button>();
        gameSceneUI_SpecialSpellModeScreen = gameSceneUIElementsContainer.transform.Find("specialSpellModeScreen").gameObject;
        gameSceneUI_SpecialSpellModeButton = gameSceneUIElementsContainer.transform.Find("specialSpellModeButton").GetComponent<Button>();
        gameSceneUI_CooldownsBar = gameSceneUIElementsContainer.transform.Find("cooldownsBar").gameObject;
        spellCooldownsUI = gameSceneUI_CooldownsBar.GetComponent<SpellCooldownsUI>();
    }

    private void SpawnNextWave() => uI_MenuController.SpawnNextWave();
}
