
public class HPMPControlLogic
{
    public float currentHP;
    public bool dead;

    public HPMPControlLogic(float currentHP) => this.currentHP = currentHP;

    public void TakeDamage(float damage, bool godMode)
    {
        currentHP -= damage;
        // Get damage animation effects
        if (godMode && currentHP < 20)      
            currentHP = 20;      
        else if (currentHP <= 0) Die();
    }

    private void Die() => dead = true;
}

