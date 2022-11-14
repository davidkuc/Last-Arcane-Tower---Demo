using System;
using TMPro;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    private Difficulty currentLevelDifficulty;

    public Difficulty CurrentLevelDifficulty => currentLevelDifficulty;

    public void SetLevelDifficulty(Difficulty levelDifficulty) => currentLevelDifficulty = levelDifficulty;
}
