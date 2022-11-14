using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(2)]
public class UI_MenuController : DebuggableBaseClass
{
    #region Fields and props
    private UI_MainMenu uI_MainMenu;
    private UI_MapMenu uI_MapMenu;
    private UI_StatsMenu uI_StatsMenu;
    private UI_UpgradeMenu uI_UpgradeMenu;
    private UI_SettingsMenu uI_SettingsMenu;
    private UI_GameScene uI_GameScene;
    private UI_FeedbackPopup uI_FeedbackPopup;
    private UI_PrizePopup uI_PrizePopup;

    public GraphicRaycaster GameSceneUIGraphicRaycaster => uI_GameScene.GameSceneUI_GraphicRaycaster;
    public Button SpecialSpellModeButton => uI_GameScene.GameSceneUI_SpecialSpellModeButton;
    public Button GameSceneUIWavesButton => uI_GameScene.GameSceneUI_SpawnWavesButton;
    public Button GameSceneUIPauseButton => uI_GameScene.GameSceneUI_PauseButton;
    public Button GameSceneMenu_BackToMenuNavButton => uI_GameScene.UI_GameScenePauseMenu.GameSceneMenu_BackToMenuNavButton;
    #endregion

    private void Awake()
    {
        GameManager.Instance.PlayerProgressDataController.UpgradePointsAmountChanged += UpdateUpgradePointsCounter;

        GetObjectsAndButtons();

        uI_MainMenu.SetActive_MainMenuElementsContainer(true);
        uI_MainMenu.AddListenersToButtons(UI_Manager.Instance.IsGameLaunchedFirstTime);
    }

    private void GetObjectsAndButtons()
    {
        uI_MainMenu = transform.Find("uI_MainMenu").GetComponent<UI_MainMenu>();
        uI_MapMenu = transform.Find("uI_MapMenu").GetComponent<UI_MapMenu>();
        uI_StatsMenu = transform.Find("uI_StatsMenu").GetComponent<UI_StatsMenu>();
        uI_UpgradeMenu = transform.Find("uI_UpgradeMenu").GetComponent<UI_UpgradeMenu>();
        uI_SettingsMenu = transform.Find("uI_SettingsMenu").GetComponent<UI_SettingsMenu>();
        uI_GameScene = transform.Find("uI_GameScene").GetComponent<UI_GameScene>();
        uI_FeedbackPopup = transform.Find("uI_FeedbackPopup").GetComponent<UI_FeedbackPopup>();
        uI_PrizePopup = transform.Find("uI_PrizePopup").GetComponent<UI_PrizePopup>();
    }

    public void Tutorial_SpecialSpellModeButtonTween(Vector3 scale, float time)
    {
        LeanTween.scale(SpecialSpellModeButton.gameObject, scale, time)
            .setEaseInOutBack().setLoopPingPong();
    }

    public void Tutorial_SpawnWavesButtonTween(Vector3 scale, float time)
    {
        LeanTween.scale(GameSceneUIWavesButton.gameObject, scale, time)
      .setEaseInOutBack().setLoopPingPong();
    }

    public void Tutorial_PauseButtonTween(Vector3 scale, float time)
    {
        LeanTween.scale(GameSceneUIPauseButton.gameObject, scale, time)
     .setEaseInOutBack().setLoopPingPong();
    }

    public void OnGameSceneUnloaded()
    {
        uI_MainMenu.OnGameSceneUnloaded();
        uI_MapMenu.OnGameSceneUnloaded();
        uI_UpgradeMenu.OnGameSceneUnloaded();
        uI_GameScene.OnGameSceneUnloaded();
    }

    public void OnGameSceneLoaded()
    {
        uI_MainMenu.OnGameSceneLoaded();
        uI_MapMenu.OnGameSceneLoaded();
        uI_UpgradeMenu.OnGameSceneLoaded();
        uI_GameScene.OnGameSceneLoaded();
    }

    public void InitializeUpgradePointsAmountForUpgradeMenu() => UI_Manager.Instance.InitializeUpgradePointsAmountForUpgradeMenu();

    public List<Button> GetMusicButtons()
    {
        return new List<Button>()
        {
            ////mainMenuMusicButton,
            //upgradeMenuMusicButton,
            ////mapMenuMusicButton,
            //gameSceneMenuMusicButton };
        };
    }

    public void EnableSpawnWavesButton() => uI_GameScene.EnableSpawnWavesButton();

    public void ToggleSpecialSpellMode()
    {
        UI_Manager.Instance.ToggleSpecialSpellMode();
        PrintDebugLog($"Special spell mode toggled in {GetType().Name} - Active: {uI_GameScene.GameSceneUI_SpecialSpellModeScreen.activeInHierarchy}");
    }

    public void StartGame(GameModes gameMode) => UI_Manager.Instance.StartGame(gameMode);

    public void OnGestureRecognized() => uI_GameScene.OnGestureRecognized();

    public void ToggleFeedbackPopup() => uI_FeedbackPopup.ToggleContainer();

    public void Toggle_Is_UI_Active() => UI_Manager.Instance.Toggle_Is_UI_Active();

    public void ToggleUpgradeMenu() => uI_UpgradeMenu.ToggleUpgradeMenuElementsContainer();

    public void ToggleStatsMenu() => uI_StatsMenu.ToggleStatsMenuElementsContainer();

    public void ToggleMapMenuElementsContainer() => uI_MapMenu.ToggleMapMenuElementsContainer();

    public void ToggleMainMenu() => uI_MainMenu.ToggleMainMenuElementsContainer();

    public void TogglePauseMenu() => uI_GameScene.TogglePauseMenu();

    public void ToggleGamePause() => UI_Manager.Instance.ToggleGamePause();

    public void ToggleGameSceneUI() => uI_GameScene.ToggleGameSceneUIElementsContainer();

    public void ToggleSettingsMenu() => uI_SettingsMenu.ToggleSettingsMenuContainer();

    public void UpdateUpgradePointsCounter(int currentUpgradePointsAmount) => uI_UpgradeMenu.UpdateUpgradePointsCounter(currentUpgradePointsAmount);

    public void UpdateProjectileSpellCooldownUI(float spellCD, float maxSpellCD) => uI_GameScene.SpellCooldownsUI.GetProjectileSpellsCD(spellCD, maxSpellCD);

    public void UpdateWallSpellCooldownUI(float spellCD, float maxSpellCD) => uI_GameScene.SpellCooldownsUI.GetpWallSpellsCD(spellCD, maxSpellCD);

    public void UpdateSkyDropSpellCooldownUI(float spellCD, float maxSpellCD) => uI_GameScene.SpellCooldownsUI.GetSkyDropSpellsCD(spellCD, maxSpellCD);

    public void UpdateSpecialSpellCooldownUI(float spellCD, float maxSpellCD) => uI_GameScene.SpellCooldownsUI.GetSpecialSpellsCD(spellCD, maxSpellCD);

    public void UpgradeButtonSelected(UI_SpellUpgrade spellUpgrade) => uI_UpgradeMenu.SetSelectedSpellUpgrade(spellUpgrade);

    public void SetLevelDifficulty(Difficulty levelDifficulty) => UI_Manager.Instance.SetLevelDifficulty(levelDifficulty);

    public void ResetPlayerProgress() => UI_Manager.Instance.ResetPlayerProgress();

    public void RestartLevel() => GameManager.Instance.RestartGameScene();

    public void ToggleTutorial() => UI_Manager.Instance.OnTutorialToggled();

    public void UnloadLevel() => UI_Manager.Instance.UnloadGameScene();

    public void SpawnNextWave() => UI_Manager.Instance.SpawnNextWave();

    public void SetLevelIndex(int levelIndex) => UI_Manager.Instance.SetLevelIndex(levelIndex);

    public void ToggleGameWinMenu() => uI_GameScene.ToggleGameWin();

    public void ToggleGameLoseMenu() => uI_GameScene.ToggleGameLose();

    public void SetActive_SpecialSpellModeButton(bool cooldownActive) => uI_GameScene.SetActive_SpecialSpellModeButton(cooldownActive);

    public void TogglePrizePopup() => uI_PrizePopup.ToggleContainer();

    public void DisableWavesBar() => uI_GameScene.DisableWavesBar();
}
