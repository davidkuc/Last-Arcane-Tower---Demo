using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager>
{
    #region Events
    public event Action SpecialSpellModeTriggeredEvent;
    public event Action GameSceneLoaded;
    public event Action GameSceneUnloaded;
    public event Action GestureRecognized;
    public event Action GestureNotRecognized;
    public event Action<Player_SO> GameSavedInto_Player_SO;
    public event Action<Player_SO> GameLoadedInto_Player_SO;
    public event Action<int> LevelIndexSet;

    public event Action<string> GestureRecognizedWithSpellName;
    #endregion


    [SerializeField] private bool devMode;
    [SerializeField] private bool godMode;

    private Difficulty stageDifficulty;

    private WaveSystem waveSystem;
    private AudioManager audioManager;
    private AdManager adManager;
    private Camera mainCamera;
    private PlayerProgressDataController playerProgressDataController;
    private DifficultyController difficultyController;
    private LevelController levelController;
    private Transform spellSpawn;
    private Transform projectileSpellSource;

    readonly string waveSystemName = "WaveSystem";
    ScenesSettings_SO scenesSettings_SO;

    public Transform SpellSpawn => spellSpawn;
    public Transform ProjectileSpellSource => projectileSpellSource;
    public bool DevMode => devMode;
    public bool GodMode => godMode;
    public bool TutorialActive { get; set; }
    public bool IsPlayingFirstTime => PlayerProgressDataController.Player_SO.isPlayingFirstTime;
    public bool SpecialSpellModeActive => InputManager.Instance.IsSpecialSpellModeActive;
    public Difficulty StageDifficulty => difficultyController.CurrentLevelDifficulty;
    public DifficultyController DifficultyController => difficultyController;
    public PlayerProgressDataController PlayerProgressDataController => playerProgressDataController;
    public WaveSystem WaveSystem => waveSystem;
    public LevelController LevelController => levelController;
    public GameModes GameMode => levelController.GameMode;
    public Camera MainCamera => mainCamera;
    private string GameSceneName => DevMode ? Settings.GameScene_Dev_Name : Settings.GameSceneName;

    protected override void Awake()
    {
        base.Awake();

        mainCamera = Camera.main;
        adManager = GetComponent<AdManager>();
        waveSystem = transform.Find(waveSystemName).GetComponent<WaveSystem>();
        playerProgressDataController = GetComponent<PlayerProgressDataController>();
        difficultyController = GetComponent<DifficultyController>();
        levelController = GetComponent<LevelController>();
        audioManager = transform.Find("Audio").GetComponent(typeof(AudioManager)) as AudioManager;

        spellSpawn = transform.Find("Spell Spawn").transform;
        projectileSpellSource = transform.Find("Projectile Spell Source").transform;

        scenesSettings_SO = ResourceLoader.LoadScenesSettings_SO();

        WaveSystem.GameWon += OnGameWon;
        SpellUpgrade.OnUpgrade += OnUpgrade;
        GameSceneLoaded += LoadPlayerForManagers;
        GestureRecognized += ToggleSpecialSpellMode;
        LevelController.LevelIndexSet += WaveSystem.SetLevelIndex;

    }

    private void Update() => PrintDebugLog($"Special spell mode active: {SpecialSpellModeActive}");

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            GameSavedInto_Player_SO?.Invoke(PlayerProgressDataController.Player_SO);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            GameSavedInto_Player_SO?.Invoke(PlayerProgressDataController.Player_SO);
    }

    private void OnApplicationQuit()
    {
        GameSavedInto_Player_SO?.Invoke(PlayerProgressDataController.Player_SO);
        PlayerProgressDataController.SaveGame();
    }

    public void StartGame(GameModes gameMode)
    {
        levelController.SetGameMode(gameMode);
        LoadLevel(levelController.LevelIndex);
        if (gameMode == GameModes.SandboxMode)
        {
            UI_Manager.Instance.DisableWavesBar();
        }
    }

    public void ShowAd() => adManager.ShowAd();

    public void SetLevelDifficulty(Difficulty levelDifficulty) => difficultyController.SetLevelDifficulty(levelDifficulty);

    public void InvokeSavePlayerData() => GameSavedInto_Player_SO?.Invoke(PlayerProgressDataController.Player_SO);

    public void InvokeLoadPlayerData() => GameLoadedInto_Player_SO?.Invoke(PlayerProgressDataController.Player_SO);

    public void InitializeUpgradePointsAmountForUpgradeMenu() => playerProgressDataController.InvokeUpgradePointsAmountChanged(playerProgressDataController.Player_SO.upgradePointsAvailable);

    public void OnGestureNotRecognized() => GestureNotRecognized?.Invoke();

    public void InvokeGestureRecognizedWithSpellName(string recognizedGesture) => GestureRecognizedWithSpellName?.Invoke(recognizedGesture);

    public void ChangeDifficulty(Difficulty difficulty) => stageDifficulty = difficulty;

    [ContextMenu("Load Game Scene")]
    public void LoadGameScene(LevelData_SO wave)
    {
        UI_Manager.Instance.ToggleMenuParticleEffect(); // Turn off menu effects
        audioManager.TurnOffMenuMusic();
        audioManager.TurnOnBattleMusic();
        Scene scene = SceneManager.GetSceneByName(DevMode ? Settings.GameScene_Dev_Name : Settings.GameSceneName);
        if (scene == null) Debug.Log("Scene name error SceneControl");
        SceneManager.LoadSceneAsync(DevMode ? Settings.GameScene_Dev_Name : Settings.GameSceneName, LoadSceneMode.Additive);

        scenesSettings_SO.levelData_SO = wave;
        scenesSettings_SO.levelScore = 0;
        WaveSystem.StartTheGame();
        GameSceneLoaded?.Invoke();

        UI_Manager.Instance.EnableSpawnWavesButton();
    }

    [ContextMenu("Unload Game Scene")]
    public void UnloadGameScene()
    {
        if (TutorialActive)
            return;

        if (GameMode == GameModes.SandboxMode)
            playerProgressDataController.AddUpgradePoints(1);

        UI_Manager.Instance.ToggleMenuParticleEffect(); // Turn on menu effects
        audioManager.TurnOffBattleMusic();
        audioManager.TurnOnMenuMusic();
        Scene scene = SceneManager.GetSceneByName(GameSceneName);
        if (scene == null) Debug.Log("Scene name error GameManager");

        SceneManager.UnloadSceneAsync(GameSceneName);
        WaveSystem.EndTheGame();
        GameSceneUnloaded?.Invoke();
    }

    public void UnloadTutorialScene()
    {
        UI_Manager.Instance.ToggleMenuParticleEffect();
        audioManager.TurnOffBattleMusic();
        audioManager.TurnOnMenuMusic();

        SceneManager.UnloadSceneAsync(Settings.TutorialSceneName);
        GameSceneUnloaded?.Invoke();
    }

    public void LoadLevel(int levelIndex) => LoadGameScene(levelController.Levels_SO.levels[levelIndex]);

    public void LoadTutorial()
    {
        audioManager.TurnOffMenuMusic();
        audioManager.TurnOnBattleMusic();

        SceneManager.LoadSceneAsync("TutorialScene", LoadSceneMode.Additive);
        GameSceneLoaded?.Invoke();
    }

    [ContextMenu("Restart Game Scene")]
    public void RestartGameScene()
    {
        UnloadGameScene();
        LoadGameScene(scenesSettings_SO.levelData_SO);
    }

    [ContextMenu("Reset Player Progress")]
    public void ResetPlayerProgress()
    {
        PlayerProgressDataController.ResetPlayerProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [ContextMenu("Open Player Data Folder")]
    public void OpenPlayerDataFolder() => SaveSystem.OpenPlayerDataFolder();

    public void ToggleSpecialSpellMode() => InputManager.Instance.ToggleSpecialSpellMode("null");

    public List<Button> GetMusicButtons() => UI_Manager.Instance.GetMusicButtons();

    [ContextMenu("Win Game")]
    public void GameWin()
    {
        UnloadGameScene();
        UI_Manager.Instance.ToggleGameWinMenu();
    }

    public void SetGodMode(bool value) => godMode = value;

    public void OnTutorialFinish()
    {
        playerProgressDataController.Player_SO.isPlayingFirstTime = false;
        WaveSystem.DestroyAllMonters();
    }

    public void SetLevelIndex(int levelIndex) => levelController.SetLevelIndex(levelIndex);

    public void SpawnNextWave() => waveSystem.OnClickSpawnWave();

    public void ToggleGamePause()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        InputManager.Instance.ToggleGamePause();
    }

    public void ToggleFeedbackPopup()
    {
        if (playerProgressDataController.Player_SO.feedbackPoppedUpOnce)
            return;


        if (levelController.LevelIndex == 1)
        {
            UI_Manager.Instance.ToggleFeedbackPopup();
            playerProgressDataController.Player_SO.feedbackPoppedUpOnce = true;
        }
    }

    public void AddUpgradePoints(int amount) => playerProgressDataController.AddUpgradePoints(amount);

    private void OnUpgrade() => PlayerProgressDataController.DecrementUpgradePoints();

    private IEnumerator DelayPlayerLoading(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        var playerLoaders = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerLoader>();
        foreach (var item in playerLoaders)
        {
            item.LoadPlayer();
        }
    }

    private void LoadPlayerForManagers() => StartCoroutine(DelayPlayerLoading(0.2f));

    private void OnGameWon(int starsAmount) => playerProgressDataController.AddUpgradePoints(starsAmount);
}
