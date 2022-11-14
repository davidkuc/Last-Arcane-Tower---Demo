using UnityEngine.UI;

public class UI_LevelPreview : DebuggableBaseClass
{
    private UI_MapMenu uI_MapMenu;

    private Button backButton;
    private Button sandboxModeButton;
    private Button normalModeButton;
    private Button difficulty_NormalButton;
    private Button difficulty_HardButton;
    private Button difficulty_CrazyButton;

    private Image levelPreviewImage;

    private void Awake()
    {
        uI_MapMenu = transform.parent.parent.GetComponent<UI_MapMenu>();

        backButton = transform.Find("backButton").GetComponent<Button>();
        sandboxModeButton = transform.Find("sandboxModeButton").GetComponent<Button>();
        normalModeButton = transform.Find("normalModeButton").GetComponent<Button>();
        difficulty_NormalButton = transform.Find("difficulty_NormalButton").GetComponent<Button>();
        difficulty_HardButton = transform.Find("difficulty_HardButton").GetComponent<Button>();
        difficulty_CrazyButton = transform.Find("difficulty_CrazyButton").GetComponent<Button>();

        levelPreviewImage = transform.Find("levelPreviewImage").GetComponent<Image>();

        AddListenersForButtons();
    }

    private void OnEnable() => difficulty_NormalButton.Select();

    private void AddListenersForButtons()
    {
        backButton.onClick.AddListener(ToggleUI_LevelPreview);
        normalModeButton.onClick.AddListener(delegate { uI_MapMenu.StartGame(GameModes.NormalMode); });
        sandboxModeButton.onClick.AddListener(delegate { uI_MapMenu.StartGame(GameModes.SandboxMode); });
        difficulty_NormalButton.onClick.AddListener(delegate { uI_MapMenu.SetLevelDifficulty(Difficulty.normal); });
        difficulty_HardButton.onClick.AddListener(delegate { uI_MapMenu.SetLevelDifficulty(Difficulty.hard); });
        difficulty_CrazyButton.onClick.AddListener(delegate { uI_MapMenu.SetLevelDifficulty(Difficulty.crazy); });
    }

    private void StartGame(GameModes gameMode) => uI_MapMenu.StartGame(gameMode);

    private void ToggleUI_LevelPreview() => gameObject.SetActive(!gameObject.activeInHierarchy);
}


