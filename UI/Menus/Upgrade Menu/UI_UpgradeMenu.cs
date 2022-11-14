using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(3)]
public class UI_UpgradeMenu : UI_BaseClass
{
    private UI_SpellUpgrade selectedSpellUpgrade;

    private GameObject upgradeMenuElementsContainer;
    private Button upgradeMenu_BackNavButton;
    private Button upgradeMenu_SpellUpgradeButton;
    private Button bonusUpgradePointButton;
    private TMP_Text upgradeMenu_UpgradePointsCounterText;

    protected override void Awake()
    {
        base.Awake();
        InitializeUpgradePointsAmountForUpgradeMenu();
    }

    public void SetSelectedSpellUpgrade(UI_SpellUpgrade spellUpgrade) => selectedSpellUpgrade = spellUpgrade;

    public void UpdateUpgradePointsCounter(int currentUpgradePointsAmount) => upgradeMenu_UpgradePointsCounterText.text = $"Points: {currentUpgradePointsAmount}";

    public void OnGameSceneUnloaded()
    {
        if (upgradeMenuElementsContainer.activeInHierarchy)
            upgradeMenuElementsContainer.SetActive(false);
    }

    public void OnGameSceneLoaded()
    {
        if (upgradeMenuElementsContainer.activeInHierarchy)
            upgradeMenuElementsContainer.SetActive(false);
    }

    public void InitializeUpgradePointsAmountForUpgradeMenu() => uI_MenuController.InitializeUpgradePointsAmountForUpgradeMenu();

    public void ToggleUpgradeMenuElementsContainer() => Helpers.SetActive_Toggle(upgradeMenuElementsContainer);

    protected override void GetObjectsAndButtons()
    {
        upgradeMenuElementsContainer = transform.Find("canvas").Find("upgradeMenuElementsContainer").gameObject;
        var upgradeTree = upgradeMenuElementsContainer.transform.Find("upgradeTree");
        upgradeMenu_BackNavButton = upgradeTree.Find("backNavButton").GetComponent<Button>();
        bonusUpgradePointButton = upgradeTree.Find("bonusUpgradePointButton").GetComponent<Button>();
        upgradeMenu_UpgradePointsCounterText = upgradeTree.Find("upgradePointsCounterText").GetComponent<TMP_Text>();
        upgradeMenu_SpellUpgradeButton = upgradeTree.Find("spellUpgradeButton").GetComponent<Button>();
    }

    protected override void AddListeners()
    {
        upgradeMenu_BackNavButton.onClick.AddListener(ToggleUpgradeMenuElementsContainer);
        upgradeMenu_BackNavButton.onClick.AddListener(ToggleMapMenuElementsContainer);
        upgradeMenu_SpellUpgradeButton.onClick.AddListener(UpgradeSpell);
        bonusUpgradePointButton.onClick.AddListener(ShowAd);
    }

    private void ShowAd()
    {
        GameManager.Instance.ShowAd();
        GameManager.Instance.AddUpgradePoints(1);
    }

    private void UpgradeSpell() => selectedSpellUpgrade.Upgrade();

    private void ToggleMapMenuElementsContainer() => uI_MenuController.ToggleMapMenuElementsContainer();
}
