using UnityEngine;

public abstract class BaseEnemyControl : MonoBehaviour
{
    [SerializeField] protected bool isMovingEnemy = true;
    [HideInInspector] public bool overrideMovementSpeed = false;
    [HideInInspector] public float newMovementSpeed = 1f;
}

