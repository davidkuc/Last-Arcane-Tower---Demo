using UnityEngine;
using UnityEngine.UI;

public class UI_StatsMenu : UI_BaseClass
{
    private GameObject statsMenuElementsContainer;
    private Button statsMenu_BackNavButton;

    protected override void Awake() => base.Awake();

    public void ToggleStatsMenuElementsContainer() => statsMenuElementsContainer.SetActive(!statsMenuElementsContainer.activeInHierarchy);

    protected override void GetObjectsAndButtons()
    {
        statsMenuElementsContainer = transform.Find("canvas").Find("statsMenuElementsContainer").gameObject;
        statsMenu_BackNavButton = statsMenuElementsContainer.transform.Find("table").Find("backNavButton").GetComponent<Button>();
    }

    protected override void AddListeners()
    {
        statsMenu_BackNavButton.onClick.AddListener(ToggleStatsMenuElementsContainer);
        statsMenu_BackNavButton.onClick.AddListener(ToggleMapMenuElementsContainer);
    }

    private void ToggleMapMenuElementsContainer() => uI_MenuController.ToggleMapMenuElementsContainer();
}
