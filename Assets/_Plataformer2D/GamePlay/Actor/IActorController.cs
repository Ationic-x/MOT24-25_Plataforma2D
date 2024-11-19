using UnityEngine;

public interface IActorController
{
    public IStats Stats { get; }
    public void TakeDamage(float dmg, GameObject src);
    public void Heal(float hl);
}
