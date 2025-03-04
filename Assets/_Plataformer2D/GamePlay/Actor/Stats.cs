using UnityEngine;
using UnityEngine.PlayerLoop;

[System.Serializable]
public class Stats : IStats
{
    [SerializeField] protected RangedSmartFloat hp;
    [SerializeField] protected SmartVariable<int> physic;
    [SerializeField] protected SmartVariable<int> magic;
    [SerializeField] protected SmartVariable<int> defense;
    [SerializeField] protected SmartVariable<int> push;

    public float HP { get => hp.CurrentValue; set => hp.CurrentValue = value; }
	public int Physic { get => physic.CurrentValue; set => physic.CurrentValue = value; }
	public int Magic { get => magic.CurrentValue; set => magic.CurrentValue = value; }
	public int Defense { get => defense.CurrentValue; set => defense.CurrentValue = value; }
	public int Push { get => push.CurrentValue; set => push.CurrentValue = value; }

    public void SetupStats(int baseHP, int basePhysic, int baseMagic, int baseDefense, int basePush)
    {
        SetupStats(baseHP, baseHP, basePhysic, baseMagic, baseDefense, basePush);
    }
    
    public void SetupStats(int currentHP, int maxHP, int currentPhysic, int currentMagic, int currentDefense, int currentPush)
    {
        hp.Update(currentHP);
        hp.maxValue = maxHP;
        physic.Update(currentPhysic);
        magic.Update(currentMagic);
        defense.Update(currentDefense);
        push.Update(currentPush);
    }
}
