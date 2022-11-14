using System;
using UnityEngine;

public interface ISpell
{
    public event Action OnSpellReset;

    public void ThrowSpell(Vector2 position);

    public void ResetSpell(float delay);
}
