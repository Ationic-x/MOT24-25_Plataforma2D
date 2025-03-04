using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Attack Data", menuName = "Mechanics/Attack Data")]
public class AttackData : ScriptableObject
{
	public float physicalDamage; // Ammount of damage dealt with the physical attribute, this value is a multiplication so 2 literally means 200%
	public float magicalDamageScale; // Ammount of damage dealt with the magic attribute, this value is a multiplication so 2 literally means 200%
	public float pushForceScale; // How much is pushed the enemy, the idea is deppending of the weapon and some kind of perks you can push harder the enmies, and also enemies have attributes to resist
	public float iframes; // Amount of time that the enemy is immune to the same attack (this is not fully used in the player)
	public bool isRanged; // Display the option/s below
	[ConditionalHide("isRanged")] public GameObject prefabToInstantiate; // Ranged Attack to instantiate
}
