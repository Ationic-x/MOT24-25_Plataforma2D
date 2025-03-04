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
        SetupStats(baseHP, baseHP, basePhysic, basePhysic, baseMagic, baseMagic, baseDefense, baseDefense, basePush, basePush);
    }
    
    public void SetupStats(int currentHP, int initialHP, int currentPhysic, int initialPhysic, int currentMagic, int initialMagic, int currentDefense, int initialDefense, int currentPush, int initialPush)
    {
        hp.initialValue = initialHP;
        hp.maxValue = initialHP;
        hp.CurrentValue = currentHP;

        physic.initialValue = initialPhysic;
		physic.CurrentValue = currentPhysic;

        magic.initialValue = initialMagic;
		magic.CurrentValue = currentMagic;

        defense.initialValue = initialDefense;
		defense.CurrentValue = currentDefense;

        push.initialValue = initialPush;
		push.CurrentValue = currentPush;
    }
}
