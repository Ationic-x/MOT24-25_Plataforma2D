using System;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected Stats stats;

    public virtual Stats Stats { get => stats; }

    public void Heal(int hl)
    {
       Stats.HP += hl;
    }

    public void TakeDamage(int dmg)
    {
        Stats.HP -= dmg;
    }
}
