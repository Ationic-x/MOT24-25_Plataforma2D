using System;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected Stats stats;
    protected Attack currentAttack;
    protected ArrayQueue<float> damageBuffer = new ArrayQueue<float>(16);

    public virtual Stats Stats { get => stats; }
    public virtual ArrayQueue<float> DamageBuffer { get => damageBuffer; }

    public void SetAttack(Attack attack) 
    {
        currentAttack = attack; 
    }

    public void Heal(int hl)
    {
       Stats.HP += hl;
    }

    public void TakeDamage(int dmg)
    {
        Stats.HP -= dmg;
    }

    public Attack GetAttack()
    {
        return currentAttack;
    }

}
