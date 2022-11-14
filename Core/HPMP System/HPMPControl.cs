
using UnityEngine;

public class HPMPControl : MonoBehaviour, IDamagable
{
    [SerializeField] private HPMP_SO hpMP_SO;

    private HPMPControlLogic hpMpControlLogic;
    private float currentHP;
    private bool isPlayer;
    private bool playerDead;
    private bool godMode;

    public HPMP_SO HPMP_SO => hpMP_SO;
    public bool PlayerDead => playerDead;

    private void Start()
    {
        hpMP_SO.CurrentHP = hpMP_SO.MaxHP;
        hpMpControlLogic = new HPMPControlLogic(hpMP_SO.MaxHP);
        currentHP = hpMP_SO.MaxHP;

        CheckIfPlayer();
    }

    private void CheckIfPlayer()
    {
        var playerScript = GetComponent<PlayerManager>();
        if (playerScript == null)
            isPlayer = false;
        else
            isPlayer = true;
    }

    public void TakeDamage(float damage)
    {
        currentHP = hpMpControlLogic.currentHP;
        hpMpControlLogic.TakeDamage(damage, godMode);

        if (currentHP > 0) hpMP_SO.CurrentHP = currentHP;
        else hpMP_SO.CurrentHP = 0;

        if (hpMpControlLogic.dead)
        {
            Die();
        }
    }

    public float GetCurrentHP() => currentHP;

    public void GodMode(bool value) => godMode = value;

    [ContextMenu("Trigger Death")]
    void Die()
    {
        if (isPlayer)
        {
            if (playerDead)
                return;

            playerDead = true;
            PlayerManager.Instance.Death();
            return;
        }

        Destroy(gameObject);
    }
}

