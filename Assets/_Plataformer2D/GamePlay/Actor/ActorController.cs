using System;
using UnityEngine;
using static DamageBuffer;

public class DamageBuffer
{
    public struct Damage
    {
        public uint id;
        public float time;


        public Damage(uint newId, float newTime)
        {
            id = newId;
            time = newTime;
        }
    }

    Damage defaultDamage = default;
    private readonly Damage[] _damageArray;
    private readonly int _size;
    private int _index;


    public DamageBuffer(int size)
    {
        _damageArray = new Damage[size];
        _size = size;
    }

    public ref Damage Find(uint id)
    {
        for (int i = 0; i < _size; i++)
        {
            if (_damageArray[i].id == id) return ref _damageArray[i];
        }
        return ref defaultDamage;
    }
    
    public void Push(Damage damage)
    {
        _damageArray[_index = (_index + 1) % _size] = damage;
    }
}


public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected Stats stats;
    protected Attack currentAttack;
    protected DamageBuffer damageBuffer = new DamageBuffer(16);

    public virtual Stats Stats { get => stats; }
    public virtual DamageBuffer DamageBuffer { get => damageBuffer; }

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
