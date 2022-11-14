using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.Scripting.APIUpdating;
using System;
using System.Collections.Generic;
using System.Collections;

[MovedFrom("UI_SpellUpgrade")]
public class UI_SpellUpgrade : DebuggableBaseClass
{
    public static event Action SpellUpgraded;
    public static List<GameObject> all_UI_SpellUpgrades;

    public static UI_SpellUpgrade selectedSpellUpgrade;
    [SerializeField, FormerlySerializedAs("parentSpellUpgrade")] private UI_SpellUpgrade parentSpellUpgrade;

    [SerializeField, FormerlySerializedAs("spellUpgrades")] SpellUpgrades_SO spellUpgrades;
    [SerializeField, FormerlySerializedAs("spellUpgradeType")] private SpellEffects spellUpgradeType;
    [SerializeField, FormerlySerializedAs("upgradeLevelText")] private TMP_Text upgradeLevelText;
    [SerializeField, FormerlySerializedAs("lockedIcon")] private GameObject lockedIcon;

    private SpellUpgrade spellUpgrade;
    private GameObject popupUpgradeDescription;
    private TMP_Text perLevelValueText;
    private TMP_Text currentBonusValueText;

    private Image upgradeBoxIcon;
    private Color defaultUpgradeBoxIconColor;
    private Color selectedUpgradeBoxIconColor;

    public bool Unlocked => spellUpgrade.unlocked;
    public bool IsChildSpellUpgradeAvailable => spellUpgrade.IsChildSpellUpgradeAvailable;
    public int CurrentUpgradeLevel => spellUpgrade.currentUpgradeLevel;
    public SpellUpgrade SpellUpgrade => spellUpgrade;

    private void OnEnable() => SpellUpgraded += OnSpellUpgraded;

    private void OnDisable() => SpellUpgraded -= OnSpellUpgraded;

    private void Awake()
    {
        all_UI_SpellUpgrades = new List<GameObject>();
        spellUpgrade = spellUpgrades.SpellUpgrades.FirstOrDefault(x => x.spellUpgradeType == this.spellUpgradeType);

        GameManager.Instance.InvokeLoadPlayerData();

        SetHeadOfSkillTreeToUnlocked();
        RefreshUpgradeLevelText();
        StartCoroutine(DelaySetLockedIcon());
        LoadUpgradeDescriptionTextObjects();
        UpdateDescriptionTextValues();
        GetComponent<Button>().onClick.AddListener(OnSelect);

        UI_SpellUpgrade.all_UI_SpellUpgrades.Add(gameObject);
    }

    private IEnumerator DelaySetLockedIcon()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SetLockedIcon();
    }

    public void RefreshUpgradeLevelText() => upgradeLevelText.text = spellUpgrade.currentUpgradeLevel.ToString();

    public void OnSelect()
    {
        if (selectedSpellUpgrade == this)
        {
            OnUnselect();
            selectedSpellUpgrade = null;
            return;
        }

        if (selectedSpellUpgrade == null)
            selectedSpellUpgrade = this;

        if (selectedSpellUpgrade != null && selectedSpellUpgrade != this)
        {
            selectedSpellUpgrade.OnUnselect();
            selectedSpellUpgrade = this;
        }

        upgradeBoxIcon.color = selectedUpgradeBoxIconColor;
        popupUpgradeDescription.SetActive(true);
        UI_Manager.Instance.UpgradeButtonSelected(this);
    }

    public void OnUnselect()
    {
        popupUpgradeDescription.SetActive(false);
        upgradeBoxIcon.color = defaultUpgradeBoxIconColor;
    }

    public void Upgrade()
    {
        if (parentSpellUpgrade == null && GameManager.Instance.PlayerProgressDataController.Player_SO.upgradePointsAvailable == 0)
            return;

        if (parentSpellUpgrade != null &&
            (GameManager.Instance.PlayerProgressDataController.Player_SO.upgradePointsAvailable == 0
            || !parentSpellUpgrade.IsChildSpellUpgradeAvailable))
            return;

        spellUpgrade.Upgrade();
        SetLockedIcon();
        GameManager.Instance.InvokeSavePlayerData();
        RefreshUpgradeLevelText();
        UpdateDescriptionTextValues();

        SpellUpgraded?.Invoke();
        PrintDebugLog("Spell upgraded!");
    }

    private void OnSpellUpgraded() => CheckIfCurrentSpellUpgradeIsAvailable();

    private void CheckIfCurrentSpellUpgradeIsAvailable()
    {
        if (parentSpellUpgrade == null)
            return;

        if (parentSpellUpgrade.IsChildSpellUpgradeAvailable)
            spellUpgrade.unlocked = true;

        SetLockedIcon();
    }

    private void SetHeadOfSkillTreeToUnlocked()
    {
        if (parentSpellUpgrade == null)
            spellUpgrade.unlocked = true;
    }

    private void UpdateDescriptionTextValues()
    {
        perLevelValueText.text = spellUpgrade.upgradeValuePerLevel.ToString();
        currentBonusValueText.text = (spellUpgrade.currentUpgradeLevel * spellUpgrade.upgradeValuePerLevel).ToString();
    }

    private void LoadUpgradeDescriptionTextObjects()
    {
        var popupDesc = transform.Find("PopupUpgradeDescription");
        popupUpgradeDescription = popupDesc.gameObject;
        perLevelValueText = popupDesc.Find("PerLevelValueText").GetComponent<TMP_Text>();
        currentBonusValueText = popupDesc.Find("CurrentBonusValueText").GetComponent<TMP_Text>();

        var upgrBoxIcon = transform.Find("UpgradeBoxIcon");
        upgradeBoxIcon = upgrBoxIcon.GetComponent<Image>();
        defaultUpgradeBoxIconColor = upgradeBoxIcon.color;
        selectedUpgradeBoxIconColor = Color.red;
    }

    private void ResetValues()
    {
        spellUpgrade.currentUpgradeLevel = 0;
        spellUpgrade.unlocked = false;
    }

    private void SetLockedIcon()
    {
        if (parentSpellUpgrade == null || parentSpellUpgrade.IsChildSpellUpgradeAvailable)
            lockedIcon.SetActive(false);
        else
            lockedIcon.SetActive(true);
    }
}
