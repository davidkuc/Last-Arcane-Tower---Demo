using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

[MovedFrom("TutorialManager")]
public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField, FormerlySerializedAs("orc")] private GameObject orcPrefab;
    [SerializeField, FormerlySerializedAs("cyclop")] private GameObject cyclopPrefab;
    [SerializeField, FormerlySerializedAs("garg")] private GameObject gargPrefab;
    [Space]
    [Header("Tween Settings")]
    [SerializeField] private Vector3 buttonTweenScale;
    [SerializeField] private float buttonTweenTime;
    [Space]
    [Header("Testing stuff")]
    [SerializeField] private int testIndex;

    private List<GameObject> uI_dialogs;
    private List<GameObject> dialogs;
    private int index = 0;
    private Canvas uI_DialogsCanvas;
    private Transform dialogsContainer;

    public int Index => index;

    protected override void Awake()
    {
        base.Awake();

        uI_dialogs = new List<GameObject>();
        dialogs = new List<GameObject>();

        var canvas = transform.Find("UI_DialogsContainer").Find("Canvas");
        uI_DialogsCanvas = canvas.GetComponent<Canvas>();
        dialogsContainer = transform.Find("DialogsContainer");

        AddBackToMainMenuListeners();
        UI_DialogsCanvasSetup();
        LoadDialogs(canvas);

        GameManager.Instance.SetGodMode(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.SetGodMode(false);
        GameManager.Instance.TutorialActive = false;

        GameManager.Instance.OnTutorialFinish();
    }

    private void OnEnable()
    {
        GameManager.Instance.SetGodMode(true);
        GameManager.Instance.TutorialActive = true;
    }

    private void UI_DialogsCanvasSetup()
    {
        uI_DialogsCanvas.worldCamera = Camera.main;
        uI_DialogsCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        uI_DialogsCanvas.sortingLayerID = SortingLayer.NameToID("UI");
        uI_DialogsCanvas.sortingOrder = 8;
    }

    void Start() => TriggerDialog();

    public void CancelAllTweens() => LeanTween.cancelAll();

    [ContextMenu("Trigger Dialog")]
    public void TriggerDialogInEditor()
    {
        this.index = testIndex;
        TriggerDialog();
    }

    public void TriggerDialog()
    {
        if (uI_dialogs == null || uI_dialogs.Count == 0
            || dialogs == null || dialogs.Count == 0
            || Index > uI_dialogs.Count)
            return;

        if (Index == uI_dialogs.Count)
        {
            DeactivatePreviousDialog();

            index++;
            return;
        }

        if (Index > 0)
            DeactivatePreviousDialog();

        ActivateCurrentDialog();
        index++;
    }

    private void ActivateCurrentDialog()
    {
        if (Index >= uI_dialogs.Count || Index >= dialogs.Count)
            return;

        if (uI_dialogs != null || uI_dialogs.Count > 0)
            uI_dialogs[Index]?.SetActive(true);

        if (dialogs != null || dialogs.Count > 0)
            dialogs[Index]?.SetActive(true);

        SetupDialog();
    }

    private void SetupDialog()
    {
        switch (Index)
        {
            case 1:
                Dialog1();
                break;
            case 2:
                Dialog2();
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                Dialog5();
                break;
            case 6:
                break;
            case 7:
                Dialog7();
                break;
            case 8:
                Dialog8();
                break;
            case 9:
                Dialog9();
                break;
            case 10:
                Dialog10();
                break;
            case 11:
                Dialog11();
                break;
            case 12:
                Dialog12();
                break;
            case 13:
                Dialog13();
                break;
            case 14:
                Dialog14();
                break;
            case 16:
                Dialog16();
                break;
            case 17:
                Dialog17();
                break;
            default:
                break;
        }
    }

    private void Dialog17()
    {
        AddTweenForPauseButton(buttonTweenScale, buttonTweenTime);
        AddPauseButtonListeners();
    }

    private void Dialog16()
    {
        AddTweenForSpawnWavesButton(buttonTweenScale, buttonTweenTime);
        AddSpawnWavesButtonListeners();
    }

    private void Dialog14() => AddListenersForSpecialSpellThrow_Dialog14();

    private void Dialog13()
    {
        AddTweenForSpecialSpellModeButton(buttonTweenScale, buttonTweenTime);
        AddSpecialSpellModeButtonListeners();
    }

    private void Dialog12() => StartCoroutine(SpawnWaves_Dialog12());

    private IEnumerator SpawnWaves_Dialog12()
    {
        var multiplier = 1.3f;
        for (int i = 1; i < 5; i++)
        {
            multiplier *= i;
            InstantiateEnemy(orcPrefab, new Vector2(4f + multiplier, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
            InstantiateEnemy(orcPrefab, new Vector2(5f + multiplier, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
            InstantiateEnemy(cyclopPrefab, new Vector2(7f + multiplier, Settings.CyclopSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
            InstantiateEnemy(gargPrefab, new Vector2(9f + multiplier, Settings.GargSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);

            InstantiateEnemy(orcPrefab, new Vector2(-4f - multiplier, Settings.OrcSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);
            InstantiateEnemy(orcPrefab, new Vector2(-5f - multiplier, Settings.OrcSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);
            InstantiateEnemy(cyclopPrefab, new Vector2(-7f - multiplier, Settings.CyclopSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);
            InstantiateEnemy(gargPrefab, new Vector2(-9f - multiplier, Settings.GargSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private void Dialog11() => AddListenersForSpecialSpellThrow_Dialog11();

    private void Dialog10()
    {
        AddTweenForSpecialSpellModeButton(buttonTweenScale, buttonTweenTime);
        AddSpecialSpellModeButtonListeners();

        InstantiateEnemy(orcPrefab, new Vector2(-4f, Settings.OrcSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(orcPrefab, new Vector2(-5f, Settings.OrcSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(cyclopPrefab, new Vector2(-7f, Settings.CyclopSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(gargPrefab, new Vector2(-9f, Settings.GargSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);
    }

    private void Dialog9() => AddListenersForSpecialSpellThrow_Dialog9();

    private void Dialog8()
    {
        AddTweenForSpecialSpellModeButton(buttonTweenScale, buttonTweenTime);
        AddSpecialSpellModeButtonListeners();
    }

    private void Dialog7()
    {
        InstantiateEnemy(orcPrefab, new Vector2(4f, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(orcPrefab, new Vector2(5f, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(cyclopPrefab, new Vector2(7f, Settings.CyclopSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(gargPrefab, new Vector2(9f, Settings.GargSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
    }

    private void Dialog5()
    {
        InstantiateEnemy(orcPrefab, new Vector2(4f, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(orcPrefab, new Vector2(5f, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
        InstantiateEnemy(orcPrefab, new Vector2(6f, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);
    }

    private void Dialog2() => InstantiateEnemy(orcPrefab, new Vector2(-7f, Settings.OrcSpawnPosition_Y), Quaternion.identity, Settings.TutorialEnemyMovementSpeed);

    private void Dialog1() => InstantiateEnemy(orcPrefab, new Vector2(7f, Settings.OrcSpawnPosition_Y), Quaternion.identity, -Settings.TutorialEnemyMovementSpeed);

    private void InstantiateEnemy(GameObject go, Vector2 pos, Quaternion rotation, float enemyMoveDirection)
    {
        Instantiate(go, pos, rotation, GameManager.Instance.WaveSystem.transform)
            .SetEnemyMoveDirection(enemyMoveDirection)
            .SetEnemyFacingDirection(enemyMoveDirection)
            .SetEnemyRigidbodyToDynamic();
    }

    private void DeactivatePreviousDialog()
    {
        if (uI_dialogs != null || uI_dialogs.Count > 0)
            uI_dialogs[Index - 1]?.SetActive(false);

        if (dialogs != null || dialogs.Count > 0)
            dialogs[Index - 1]?.SetActive(false);
    }

    private void LoadDialogs(Transform canvas)
    {
        for (int i = 0; i < 20; i++)
        {
            var uI_Dialog = canvas.Find($"UI_Dialog ({i})");
            if (uI_Dialog == null)
                break;

            uI_dialogs.Add(uI_Dialog.gameObject);
        }


        for (int i = 0; i < 20; i++)
        {
            var dialog = dialogsContainer.Find($"Dialog ({i})");
            if (dialog == null)
                break;

            dialogs.Add(dialog.gameObject);
        }
    }

    private void AddListenersForSpecialSpellThrow_Dialog14()
    {
        GameManager.Instance.GestureRecognizedWithSpellName += CheckIfSpecialSpellWasThrown_Dialog14;
        GameManager.Instance.GestureRecognizedWithSpellName += StopCheckingIfSpecialSpellWasThrown_Dialog14;
    }

    private void AddListenersForSpecialSpellThrow_Dialog11()
    {
        GameManager.Instance.GestureRecognizedWithSpellName += CheckIfSpecialSpellWasThrown_Dialog11;
        GameManager.Instance.GestureRecognizedWithSpellName += StopCheckingIfSpecialSpellWasThrown_Dialog11;
    }

    private void AddListenersForSpecialSpellThrow_Dialog9()
    {
        GameManager.Instance.GestureRecognizedWithSpellName += CheckIfSpecialSpellWasThrown_Dialog9;
        GameManager.Instance.GestureRecognizedWithSpellName += StopCheckingIfSpecialSpellWasThrown_Dialog9;
    }


    private void StopCheckingIfSpecialSpellWasThrown_Dialog14(string specialSpellName)
    {
        if (specialSpellName == "Vaporize")
        {
            GameManager.Instance.GestureRecognizedWithSpellName -= CheckIfSpecialSpellWasThrown_Dialog14;
            GameManager.Instance.GestureRecognizedWithSpellName -= StopCheckingIfSpecialSpellWasThrown_Dialog14;
        }
    }

    private void StopCheckingIfSpecialSpellWasThrown_Dialog11(string specialSpellName)
    {
        if (specialSpellName == "ArmadilloLeft")
        {
            GameManager.Instance.GestureRecognizedWithSpellName -= CheckIfSpecialSpellWasThrown_Dialog11;
            GameManager.Instance.GestureRecognizedWithSpellName -= StopCheckingIfSpecialSpellWasThrown_Dialog11;
        }
    }

    private void StopCheckingIfSpecialSpellWasThrown_Dialog9(string specialSpellName)
    {
        if (specialSpellName == "ArmadilloRight")
        {
            GameManager.Instance.GestureRecognizedWithSpellName -= CheckIfSpecialSpellWasThrown_Dialog9;
            GameManager.Instance.GestureRecognizedWithSpellName -= StopCheckingIfSpecialSpellWasThrown_Dialog9;
        }
    }

    private void CheckIfSpecialSpellWasThrown_Dialog14(string specialSpellName)
    {
        if (specialSpellName == "Vaporize")
            TriggerDialog();
    }


    private void CheckIfSpecialSpellWasThrown_Dialog11(string specialSpellName)
    {
        if (specialSpellName == "ArmadilloLeft")
            TriggerDialog();
    }


    private void CheckIfSpecialSpellWasThrown_Dialog9(string specialSpellName)
    {
        if (specialSpellName == "ArmadilloRight")
            TriggerDialog();
    }

    private void AddPauseButtonListeners()
    {
        UI_Manager.Instance.PauseButton.onClick.AddListener(LeanTween.cancelAll);
        UI_Manager.Instance.PauseButton.onClick.AddListener(ResetButtonScale);
        UI_Manager.Instance.PauseButton.onClick.AddListener(TriggerDialog);
        UI_Manager.Instance.PauseButton.onClick.AddListener(CleanupPauseButtonListeners);
    }

    private void CleanupPauseButtonListeners()
    {
        UI_Manager.Instance.PauseButton.onClick.RemoveListener(LeanTween.cancelAll);
        UI_Manager.Instance.PauseButton.onClick.RemoveListener(ResetButtonScale);
        UI_Manager.Instance.PauseButton.onClick.RemoveListener(TriggerDialog);
        UI_Manager.Instance.PauseButton.onClick.RemoveListener(CleanupPauseButtonListeners);
    }

    private void AddSpawnWavesButtonListeners()
    {
        UI_Manager.Instance.SpawnWavesButton.onClick.AddListener(LeanTween.cancelAll);
        UI_Manager.Instance.SpawnWavesButton.onClick.AddListener(ResetButtonScale);
        UI_Manager.Instance.SpawnWavesButton.onClick.AddListener(TriggerDialog);
        UI_Manager.Instance.SpawnWavesButton.onClick.AddListener(CleanupSpawnWavesButtonListeners);
    }

    private void CleanupSpawnWavesButtonListeners()
    {
        UI_Manager.Instance.SpawnWavesButton.onClick.RemoveListener(LeanTween.cancelAll);
        UI_Manager.Instance.SpawnWavesButton.onClick.RemoveListener(ResetButtonScale);
        UI_Manager.Instance.SpawnWavesButton.onClick.RemoveListener(TriggerDialog);
        UI_Manager.Instance.SpawnWavesButton.onClick.RemoveListener(CleanupSpawnWavesButtonListeners);
    }

    private void CleanupSpecialSpellModeButtonListeners()
    {
        UI_Manager.Instance.SpecialSpellModeButton.onClick.RemoveListener(LeanTween.cancelAll);
        UI_Manager.Instance.SpecialSpellModeButton.onClick.RemoveListener(ResetButtonScale);
        UI_Manager.Instance.SpecialSpellModeButton.onClick.RemoveListener(TriggerDialog);
        UI_Manager.Instance.SpecialSpellModeButton.onClick.RemoveListener(CleanupSpecialSpellModeButtonListeners);
    }

    private void AddSpecialSpellModeButtonListeners()
    {
        UI_Manager.Instance.SpecialSpellModeButton.onClick.AddListener(LeanTween.cancelAll);
        UI_Manager.Instance.SpecialSpellModeButton.onClick.AddListener(ResetButtonScale);
        UI_Manager.Instance.SpecialSpellModeButton.onClick.AddListener(TriggerDialog);
        UI_Manager.Instance.SpecialSpellModeButton.onClick.AddListener(CleanupSpecialSpellModeButtonListeners);
    }

    private void AddBackToMainMenuListeners()
    {
        UI_Manager.Instance.BackToMainMenuButton.onClick.AddListener(GameManager.Instance.UnloadTutorialScene);
        UI_Manager.Instance.BackToMainMenuButton.onClick.AddListener(CleanupBackToMainMenuButtonListeners);
    }

    private void CleanupBackToMainMenuButtonListeners()
    {
        UI_Manager.Instance.BackToMainMenuButton.onClick.RemoveListener(GameManager.Instance.UnloadTutorialScene);
        UI_Manager.Instance.BackToMainMenuButton.onClick.RemoveListener(CleanupBackToMainMenuButtonListeners);
    }

    private void AddTweenForPauseButton(Vector3 scale, float time) => UI_Manager.Instance.PauseButtonTween(scale, time);

    private void AddTweenForSpecialSpellModeButton(Vector3 scale, float time) => UI_Manager.Instance.Tutorial_SpecialSpellModeButtonTween(scale, time);

    private void AddTweenForSpawnWavesButton(Vector3 scale, float time) => UI_Manager.Instance.Tutorial_SpawnWavesButtonTween(scale, time);

    private void ResetButtonScale() => UI_Manager.Instance.SpecialSpellModeButton.GetComponent<RectTransform>().localScale = Vector3.one;
}
