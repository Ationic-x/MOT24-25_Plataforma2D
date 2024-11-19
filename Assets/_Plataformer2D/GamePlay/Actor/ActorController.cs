using UnityEngine;

public abstract class ActorController : MonoBehaviour, IActorController
{
    public virtual IStats Stats
    {
        get => throw new System.NotImplementedException();
    }

    public void Heal(float hl)
    {
       Stats.HP += hl;
    }

    public void TakeDamage(float dmg, GameObject src)
    {
        Stats.HP -= dmg;
    }
}
