using System;
using UnityEngine;

public class SpellCooldownLogic
{
    public event Action CooldownActivated;
    public event Action CooldownDeactivated;

    private float cooldownTime;
    private float nextSpellThrowTime;

    public bool CooldownActive { get; private set; } = false;
    public float CurrentCooldown => nextSpellThrowTime - Time.time;
    public SpellCooldownLogic(float cooldownTime) => this.cooldownTime = cooldownTime;

    public void ProcessCooldown()
    {
        if (CooldownActive)
            return;

        nextSpellThrowTime = Time.time + cooldownTime;
        SetCooldown(true);
        CooldownActivated?.Invoke();
    }

    public void CheckCooldownTime()
    {
        if (Time.time > nextSpellThrowTime)
        {
            SetCooldown(false);
            CooldownDeactivated?.Invoke();
        }
    }

    private void SetCooldown(bool active) => CooldownActive = active;
}
