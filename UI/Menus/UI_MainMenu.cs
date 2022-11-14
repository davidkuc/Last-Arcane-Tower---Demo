using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : UI_BaseClass
{
    private GameObject mainMenuElementsContainer;
    private GameObject logo;
    private Button playNavButton;

    protected override void Awake()
    {
        base.Awake();
        logo.SetActive(false);
        playNavButton.gameObject.SetActive(false);
        Invoke("TogglePlayButton", 2f);
        Invoke("ToggleLogo", 0.7f);
    }

    private void ToggleLogo() => Helpers.SetActive_Toggle(logo);

    private void TogglePlayButton() => Helpers.SetActive_Toggle(playNavButton.gameObject);

    public void AddListenersToButtons(bool isGameLaunchedFirstTime)
    {
        if (isGameLaunchedFirstTime)
            playNavButton.onClick.AddListener(ToggleTutorial);
        else
            playNavButton.onClick.AddListener(ToggleMainMenuElementsContainer);

    }

    public void OnGameSceneLoaded() => mainMenuElementsContainer.SetActive(false);

    public void OnGameSceneUnloaded()
    {
        if (mainMenuElementsContainer.activeInHierarchy)
            mainMenuElementsContainer.SetActive(false);
    }

    public void SetActive_MainMenuElementsContainer(bool active) => mainMenuElementsContainer.SetActive(active);

    public void ToggleMainMenuElementsContainer()
    {
        PrintDebugLog($"Pressed {playNavButton.name}");
        mainMenuElementsContainer.SetActive(!mainMenuElementsContainer.activeInHierarchy);
        uI_MenuController.ToggleMapMenuElementsContainer();
    }

    protected override void GetObjectsAndButtons()
    {
        var canvas = transform.Find("canvas");
        mainMenuElementsContainer = canvas.Find("mainMenuElementsContainer").gameObject;
        logo = mainMenuElementsContainer.transform.Find("logo").gameObject;
        playNavButton = mainMenuElementsContainer.transform.Find("playNavButton").GetComponent<Button>();
    }

    protected override void AddListeners()
    {
        return;
    }

    private void ToggleTutorial()
    {
        PrintDebugLog($"Toggled tutorial");
        uI_MenuController.ToggleTutorial();
    }
}
