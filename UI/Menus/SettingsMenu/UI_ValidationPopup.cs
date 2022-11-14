using UnityEngine;
using UnityEngine.UI;

public class UI_ValidationPopup : UI_BaseClass
{
    private UI_SettingsMenu uI_SettingsMenu;

    private GameObject validationPopupContainer;
    private Button yesButton;
    private Button noButton;

    protected override void Awake() => base.Awake();

    public void ToggleValidationPopupContainer() => Helpers.SetActive_Toggle(validationPopupContainer);

    protected override void AddListeners()
    {
        yesButton.onClick.AddListener(uI_SettingsMenu.ResetPlayerProgress);
        noButton.onClick.AddListener(ToggleValidationPopupContainer);
    }

    protected override void GetObjectsAndButtons()
    {
        uI_SettingsMenu = transform.parent.parent.parent.parent.GetComponent<UI_SettingsMenu>();
        validationPopupContainer = transform.Find("canvas").Find("validationPopupContainer").gameObject;
        yesButton = validationPopupContainer.transform.Find("yesButton").GetComponent<Button>();
        noButton = validationPopupContainer.transform.Find("noButton").GetComponent<Button>();
    }
}
