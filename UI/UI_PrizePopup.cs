using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PrizePopup : UI_BaseClass
{
    private GameObject container;
    private TMP_Text upgradePointsAmountText;
    private Button okButton;

    protected override void Awake() => base.Awake();

    protected override void AddListeners() => okButton.onClick.AddListener(ToggleContainer);

    public void ToggleContainer()
    {
        if (!container.activeInHierarchy)
            GameManager.Instance.PlayerProgressDataController.AddUpgradePoints(3);

        //+3 upgrade points!
        Helpers.SetActive_Toggle(container);
    }

    protected override void GetObjectsAndButtons()
    {
        container = transform.Find("canvas").Find("container").gameObject;
        upgradePointsAmountText = container.transform.Find("valueContainer").Find("valuePlaceholder").GetComponent<TMP_Text>();
        okButton = container.transform.Find("okButton").GetComponent<Button>();
    }
}
