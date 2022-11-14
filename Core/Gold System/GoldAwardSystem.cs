using UnityEngine;

public class GoldAwardSystem : MonoBehaviour
{
    [SerializeField] ScenesSettings_SO scenesSettings;
    [SerializeField] EnemyBehaviour_SO enemyBehaviour;
    DifficultyScaling enemy;

    private void Start() => enemy = new DifficultyScaling(GameManager.Instance.StageDifficulty, enemyBehaviour);

    public void RewardPlayerWithGold()
    {
        var amountOfGold = CalculateAmountOfGold();
        scenesSettings.levelScore += amountOfGold;
        GameManager.Instance.PlayerProgressDataController.Player_SO.gold += amountOfGold;
    }

    int CalculateAmountOfGold() => Random.Range(enemy.minGold, enemy.maxGold);
}
