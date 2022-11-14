using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsMenu : UI_BaseClass
{
    private UI_ValidationPopup uI_ValidationPopup;

    private GameObject settingsMenuContainer;
    private Button settingsMenu_BackNavButton;
    private Button settingsMenu_ResetPlayerProgressButton;

    protected override void Awake() => base.Awake();

    public void ToggleSettingsMenuContainer() => settingsMenuContainer.SetActive(!settingsMenuContainer.activeInHierarchy);

    public void ResetPlayerProgress() => uI_MenuController.ResetPlayerProgress();

    protected override void AddListeners()
    {
        settingsMenu_ResetPlayerProgressButton.onClick.AddListener(ToggleValidationPopup);
        settingsMenu_BackNavButton.onClick.AddListener(ToggleSettingsMenuContainer);
    }

    private void ToggleValidationPopup() => uI_ValidationPopup.ToggleValidationPopupContainer();

    protected override void GetObjectsAndButtons()
    {

        settingsMenuContainer = transform.Find("canvas").Find("settingsMenuElementsContainer").gameObject;
        var table = settingsMenuContainer.transform.Find("table");
        uI_ValidationPopup = table.Find("uI_ValidationPopup").GetComponent<UI_ValidationPopup>();
        settingsMenu_BackNavButton = table.Find("backNavButton").GetComponent<Button>(); ;
        settingsMenu_ResetPlayerProgressButton = table.Find("resetPlayerProgressButton").GetComponent<Button>();
    }
}
