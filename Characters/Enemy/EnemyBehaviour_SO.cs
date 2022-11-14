using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO_", menuName = "Scriptable Object/EnemyBehaviour")]
public class EnemyBehaviour_SO : ScriptableObject
{
    public float movementSpeeed;
    public float damage;
    [Header(" Time beetweeen atack [s]")]
    public float timeBetweenAtack;

    [Header(" HP stats, Regeneration per second")]
    public float maxHP;
    public float regHP;

    [Header("Gold award")]
    public int minGoldAward;
    public int maxGoldAward;
}

