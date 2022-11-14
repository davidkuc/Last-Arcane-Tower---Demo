using System;

[Serializable]
public class SpellUpgrade 
{
    public static Action OnUpgrade;

    public SpellEffects spellUpgradeType;
    public string description;
    public int maxUpgradeLevel;
    public int currentUpgradeLevel;
    public int upgradeValuePerLevel;
    public int childSpellUpgradeLevelThreshold;
    public bool unlocked;
    public bool isHead;

    public bool IsChildSpellUpgradeAvailable => currentUpgradeLevel >= childSpellUpgradeLevelThreshold;

    public void Upgrade()
    {
        if (currentUpgradeLevel == maxUpgradeLevel)
            return;

        if (!unlocked)
            unlocked = true;

        currentUpgradeLevel++;

        if (OnUpgrade != null) OnUpgrade();
    }

    public void ResetUpgradeData()
    {
        currentUpgradeLevel = 0;
        unlocked = false;
    }
}
