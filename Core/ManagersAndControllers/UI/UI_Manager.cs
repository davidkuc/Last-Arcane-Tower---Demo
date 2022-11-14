using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : Singleton<UI_Manager>, IPlayerLoader
{
    public event Action OnUI_Activated;
    public event Action OnUI_Deactivated;

    private UI_GameController uI_GameController;
    private UI_MenuController uI_MenuController;
    private Canvas rootCanvas;

    private GameObject menuParticleEffect;
    private PlayerManager player;

    private bool is_UI_Active;

    [Header("Text pop-up when enemy gets hit")]
    [SerializeField] private bool showDamageAndEffectsOnHit;

    public PlayerManager Player => player;
    public bool Is_UI_Active => is_UI_Active;
    public bool ShowDamageText => showDamageAndEffectsOnHit;
    public bool IsGameLaunchedFirstTime => GameManager.Instance.IsPlayingFirstTime;
    public GraphicRaycaster GameSceneUIGraphicRaycaster => uI_MenuController.GameSceneUIGraphicRaycaster;
    public Button SpecialSpellModeButton => uI_MenuController.SpecialSpellModeButton;
    public Button SpawnWavesButton => uI_MenuController.GameSceneUIWavesButton;
    public Button PauseButton => uI_MenuController.GameSceneUIPauseButton;
    public Button BackToMainMenuButton => uI_MenuController.GameSceneMenu_BackToMenuNavButton;

    private void Update() => PrintDebugLog($"Is UI Active: {Is_UI_Active}");

    protected override void Awake()
    {
        base.Awake();

        is_UI_Active = true;
        var canvas = transform.Find("canvas");
        rootCanvas = canvas.GetComponent<Canvas>();
        uI_GameController = canvas.Find("UI_GameController").GetComponent<UI_GameController>();
        uI_MenuController = canvas.Find("UI_MenuController").GetComponent<UI_MenuController>();
        menuParticleEffect = canvas.Find("MainMenu PE").gameObject;

        SetupRootCanvas();

        GameManager.Instance.GameSceneLoaded += OnGameSceneLoaded;
        GameManager.Instance.GameSceneUnloaded += OnGameSceneUnloaded;
        GameManager.Instance.GestureRecognizedWithSpellName += OnGestureRecognizedWithSpellName;
    }

    private void SetupRootCanvas()
    {
        rootCanvas.worldCamera = GameManager.Instance.MainCamera;
        rootCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        rootCanvas.sortingLayerID = SortingLayer.NameToID("Projectiles");
        rootCanvas.sortingOrder = 6;
    }

    public void Set_UI_Active(bool isActive)
    {
        is_UI_Active = isActive;
        if (!isActive)
        {
            if (OnUI_Deactivated != null) OnUI_Deactivated();
        }
        else
        {
            if (OnUI_Activated != null) OnUI_Activated();
        }
    }

    public void OnGameSceneUnloaded()
    {
        Set_UI_Active(true);
        uI_MenuController.OnGameSceneUnloaded();
    }

    public void OnGameSceneLoaded()
    {
        Set_UI_Active(false);
        uI_MenuController.OnGameSceneLoaded();
    }

    public void EnableSpawnWavesButton() => uI_MenuController.EnableSpawnWavesButton();

    public void InitializeUpgradePointsAmountForUpgradeMenu() => GameManager.Instance.InitializeUpgradePointsAmountForUpgradeMenu();

    public void LoadLevel(LevelData_SO levelData) => GameManager.Instance.LoadGameScene(levelData);

    public void OnGestureRecognizedWithSpellName(string specialSpellName) => uI_MenuController.OnGestureRecognized();

    public void Tutorial_SpecialSpellModeButtonTween(Vector3 scale, float time)
        => uI_MenuController.Tutorial_SpecialSpellModeButtonTween(scale, time);

    public void Tutorial_SpawnWavesButtonTween(Vector3 scale, float time)
        => uI_MenuController.Tutorial_SpawnWavesButtonTween(scale, time);

    public void PauseButtonTween(Vector3 scale, float time)
        => uI_MenuController.Tutorial_PauseButtonTween(scale, time);

    public void LoadPlayer() => player = PlayerManager.Instance;

    public void Toggle_Is_UI_Active() => is_UI_Active = !is_UI_Active;

    public void ToggleSpecialSpellMode() => GameManager.Instance.ToggleSpecialSpellMode();

    public void ToggleUpgradeMenu() => uI_MenuController.ToggleUpgradeMenu();

    public void ToggleMapMenu() => uI_MenuController.ToggleMapMenuElementsContainer();

    public void ToggleMainMenu() => uI_MenuController.ToggleMainMenu();

    public void ToggleGameSceneMenu() => uI_MenuController.TogglePauseMenu();

    public void ToggleGameSceneUI() => uI_MenuController.ToggleGameSceneUI();

    public void ToggleGameWinMenu() => uI_MenuController.ToggleGameWinMenu();

    public void ToggleGameLoseMenu() => uI_MenuController.ToggleGameLoseMenu();

    public void ToggleMenuParticleEffect() => menuParticleEffect.SetActive(!menuParticleEffect.activeInHierarchy);

    public void UpdateProjectileSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateProjectileSpellCooldownUI(spellCD, maxSpellCD);

    public void UpdateWallSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateWallSpellCooldownUI(spellCD, maxSpellCD);

    public void UpdateSkyDropSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateSkyDropSpellCooldownUI(spellCD, maxSpellCD);

    public void UpdateSpecialSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateSpecialSpellCooldownUI(spellCD, maxSpellCD);

    //public void LoadLevel(int index) => uI_MenuController.LoadLevel(index);

    public List<Button> GetMusicButtons() => uI_MenuController.GetMusicButtons();

    public void OnTutorialToggled() => GameManager.Instance.LoadTutorial();

    public void UpgradeButtonSelected(UI_SpellUpgrade spellUpgrade) => uI_MenuController.UpgradeButtonSelected(spellUpgrade);

    public void ResetPlayerProgress() => GameManager.Instance.ResetPlayerProgress();

    public void StartGame(GameModes gameMode) => GameManager.Instance.StartGame(gameMode);

    public void SetLevelDifficulty(Difficulty levelDifficulty) => GameManager.Instance.SetLevelDifficulty(levelDifficulty);

    public void SetLevelIndex(int levelIndex) => GameManager.Instance.SetLevelIndex(levelIndex);

    public void UnloadGameScene() => GameManager.Instance.UnloadGameScene();

    public void SpawnNextWave() => GameManager.Instance.SpawnNextWave();

    public void ToggleGamePause() => GameManager.Instance.ToggleGamePause();

    public void SetActive_SpecialSpellModeButton(bool cooldownActive) => uI_MenuController.SetActive_SpecialSpellModeButton(cooldownActive);

    public void ToggleFeedbackPopup() => uI_MenuController.ToggleFeedbackPopup();

    public void DisableWavesBar() => uI_MenuController.DisableWavesBar();
}
