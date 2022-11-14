using UnityEngine;
using UnityEngine.UI;

public class UI_GameLose : MonoBehaviour
{
    private UI_GameScene uI_GameScene;

    private GameObject gameLoseElementsContainer;
    private GameObject upgradePointsText;
    private Button backToMenuButton;
    private Button restartButton;

    private void Awake()
    {
        GetObjectsAndButtons();
        AddListenersForButtons();
    }

    public void OnGameSceneUnloaded()
    {
        if (gameLoseElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameLoseElementsContainer);
    }

    public void OnGameSceneLoaded()
    {
        if (gameLoseElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameLoseElementsContainer);
    }

    public void ToggleGameLose()
    {
        if (GameManager.Instance.GameMode == GameModes.SandboxMode)
            upgradePointsText.SetActive(true);
        else upgradePointsText.SetActive(false);

        Helpers.SetActive_Toggle(gameLoseElementsContainer);
    }

    private void AddListenersForButtons()
    {
        backToMenuButton.onClick.AddListener(ToggleGameLoseMenu);
        backToMenuButton.onClick.AddListener(UnloadLevel);

        restartButton.onClick.AddListener(ToggleGameLoseMenu);
        restartButton.onClick.AddListener(RestartLevel);
    }

    private void GetObjectsAndButtons()
    {
        uI_GameScene = transform.parent.parent.GetComponent<UI_GameScene>();

        gameLoseElementsContainer = transform.Find("canvas").Find("gameLoseUIElementsContainer").gameObject;
        var table = gameLoseElementsContainer.transform.Find("table");
        upgradePointsText = table.Find("upgradePointsText").gameObject;
        backToMenuButton = table.Find("backToMapMenuNavButton").GetComponent<Button>();
        restartButton = table.Find("restartButton").GetComponent<Button>();
    }

    private void RestartLevel()
    {
        GameManager.Instance.ShowAd();
        uI_GameScene.RestartLevel();
    }

    private void UnloadLevel() => uI_GameScene.GoBackToMapMenu();

    private void ToggleGameLoseMenu() => Helpers.SetActive_Toggle(gameLoseElementsContainer);
}
