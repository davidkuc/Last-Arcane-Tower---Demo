using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameWin : MonoBehaviour
{
    private UI_GameScene uI_GameScene;

    private GameObject gameWinElementsContainer;
    private TMP_Text upgradePointsAmountText;
    private Button backToMapMenuNavButton;

    private void Awake()
    {
        GetObjectsAndButtons();
        AddListenersToButtons();
    }

    public void OnGameSceneUnloaded()
    {
        if (gameWinElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameWinElementsContainer);
    }

    public void OnGameSceneLoaded()
    {
        if (gameWinElementsContainer.activeInHierarchy)
            Helpers.SetActive_Toggle(gameWinElementsContainer);
    }

    public void UpdateUpgradePointsAmountText(int starsAmount) => upgradePointsAmountText.text = starsAmount.ToString();

    public void ToggleGameWin()
    {
        if (gameWinElementsContainer.activeInHierarchy)
            GameManager.Instance.ToggleFeedbackPopup();

        Helpers.SetActive_Toggle(gameWinElementsContainer);
    }

    private void AddListenersToButtons() => backToMapMenuNavButton.onClick.AddListener(ToggleGameWin);

    private void GetObjectsAndButtons()
    {
        uI_GameScene = transform.parent.parent.GetComponent<UI_GameScene>();

        gameWinElementsContainer = transform.Find("canvas").Find("gameWinUIElementsContainer").gameObject;
        backToMapMenuNavButton = gameWinElementsContainer.transform.Find("table").Find("backToMapMenuNavButton").GetComponent<Button>();
        upgradePointsAmountText = gameWinElementsContainer.transform.Find("UpgradePointsInformation")
            .Find("UpgradePointsAmountText").GetComponent<TMP_Text>();
    }

    private void OnEnable() => UpdateUpgradePointsAmountText(GameManager.Instance.WaveSystem.GetStarsAmountInthisLevel());
}
