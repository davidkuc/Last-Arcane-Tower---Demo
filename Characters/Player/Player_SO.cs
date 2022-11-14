using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_SO_", menuName = "Scriptable Object/Player")]
public class Player_SO : ScriptableObject
{
    public bool isPlayingFirstTime;
    public bool feedbackPoppedUpOnce;

    public int gold;
    public int[] stars;

    [Header("Spell Upgrades")]
    public int upgradePointsAvailable;

    public int StarsSum
    {
        get
        {
            int value = 0;
            if (stars != null)
                foreach (var star in stars)
                    value += star;

            return value;
        }
    }

    public List<SpellUpgrades_SO> spellUpgrades_SOs;

    [ContextMenu("Reset Player Data")]
    public void ResetPlayerData()
    {
        SaveSystem.ResetPlayerData();

        isPlayingFirstTime = true;
        feedbackPoppedUpOnce = false;
        gold = 0;
        stars = new int[20];
        upgradePointsAvailable = 0;
        foreach (var spellUpgrade_SO in spellUpgrades_SOs)
            spellUpgrade_SO.ResetSpellUpgrades();
    }

    public int GetLevelStarsAmount(int index) => stars[index];

    public void ChangeStarsAmount(int index, int stars)
    {
        if (this.stars[index] >= stars) return;
        this.stars[index] = stars;
    }
}

