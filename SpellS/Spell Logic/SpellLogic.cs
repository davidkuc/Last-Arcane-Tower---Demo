using System;
using UnityEngine;

public abstract class SpellLogic : MonoBehaviour, ISpell
{
    public event Action OnSpellReset;

    protected virtual void InvokeOnSpellReset() => OnSpellReset?.Invoke();

    public abstract void ResetSpell(float delay);

    public abstract void ThrowSpell(Vector2 position);
}
