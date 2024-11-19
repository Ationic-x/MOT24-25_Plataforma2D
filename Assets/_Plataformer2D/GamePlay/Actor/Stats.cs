using UnityEngine;

[System.Serializable]
public class Stats : IStats
{
    [SerializeField] protected float hp;

    public float HP 
    { 
        get => hp; 
        set => hp = value; 
    }
}
