using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressDataController : MonoBehaviour
{
    public event Action<int> OnGoldChangeEvent;
    public event Action<int> UpgradePointsAmountChanged;

    private Player_SO player_SO;
    private PlayerDataStructure playerDataStructure;

    public Player_SO Player_SO => player_SO;

    private void Awake()
    {
        player_SO = ResourceLoader.LoadPlayer_SO();
        LoadGame();
    }


    public void InvokeUpgradePointsAmountChanged(int upgradePointsAmount) => UpgradePointsAmountChanged?.Invoke(upgradePointsAmount);

    public void SaveGame()
    {
        if (playerDataStructure == null)
            playerDataStructure = CreateNewPlayerDataStructure();
        MapPlayer_SO_ToPlayerDataStructure();
        SaveSystem.SavePlayer(playerDataStructure);
    }

    public void LoadGame()
    {
        if (playerDataStructure == null)
            playerDataStructure = SaveSystem.LoadPlayer();

        if (playerDataStructure == null)
            playerDataStructure = CreateNewPlayerDataStructure();

        MapPlayerDataStructureToPlayer_SO();
        UpgradePointsAmountChanged?.Invoke(player_SO.upgradePointsAvailable);
    }

    public void ResetPlayerProgress()
    {
        SaveSystem.ResetPlayerData();
        player_SO.ResetPlayerData();
        playerDataStructure = CreateNewPlayerDataStructure();
    }

    public void AddUpgradePoints(int starsAmount)
    {
        player_SO.upgradePointsAvailable += starsAmount;
        UpgradePointsAmountChanged?.Invoke(player_SO.upgradePointsAvailable);
    }

    public void DecrementUpgradePoints()
    {
        if (player_SO.upgradePointsAvailable == 0)
            return;

        player_SO.upgradePointsAvailable--;
        UpgradePointsAmountChanged?.Invoke(player_SO.upgradePointsAvailable);
    }

    private void MapPlayer_SO_ToPlayerDataStructure()
    {
        playerDataStructure.isPlayingFirstTime = Player_SO.isPlayingFirstTime;
        playerDataStructure.gold = Player_SO.gold;
        playerDataStructure.stars = Player_SO.stars;
        playerDataStructure.upgradePointsAvailable = Player_SO.upgradePointsAvailable;
        playerDataStructure.feedbackPoppedUpOnce = Player_SO.feedbackPoppedUpOnce;
    }

    private void MapPlayerDataStructureToPlayer_SO()
    {
        Player_SO.isPlayingFirstTime = playerDataStructure.isPlayingFirstTime;
        Player_SO.gold = playerDataStructure.gold;
        Player_SO.stars = playerDataStructure.stars;
        player_SO.upgradePointsAvailable = playerDataStructure.upgradePointsAvailable;
        player_SO.feedbackPoppedUpOnce = playerDataStructure.feedbackPoppedUpOnce;
    }

    private PlayerDataStructure CreateNewPlayerDataStructure()
    {
        var newPlayerData = new PlayerDataStructure();
        newPlayerData.isPlayingFirstTime = Player_SO.isPlayingFirstTime;
        newPlayerData.gold = Player_SO.gold;
        newPlayerData.stars = Player_SO.stars;
        newPlayerData.upgradePointsAvailable = Player_SO.upgradePointsAvailable;
        newPlayerData.feedbackPoppedUpOnce = Player_SO.feedbackPoppedUpOnce;

        return newPlayerData;
    }
}

[System.Serializable]
public class PlayerDataStructure
{
    public bool isPlayingFirstTime;
    public bool feedbackPoppedUpOnce;
    public int gold;
    public int[] stars;

    public int upgradePointsAvailable;

    public List<SpellUpgradesDataStructure> spellUpgradesDataStructure;
}

[System.Serializable]
public class SpellUpgradesDataStructure
{
    public SpellTreeTypes spellTreeType;
    public SpellTypes spellType;   
    public List<SpellUpgradeDataStructure> spellUpgrades;
}

[System.Serializable]
public class SpellUpgradeDataStructure
{
    public SpellEffects spellUpgradeType;
    public string description;
    public int maxUpgradeLevel;
    public int currentUpgradeLevel;
    public int upgradeValuePerLevel;
    public int childSpellUpgradeLevelThreshold;
    public bool unlocked;
    public bool isHead;
}