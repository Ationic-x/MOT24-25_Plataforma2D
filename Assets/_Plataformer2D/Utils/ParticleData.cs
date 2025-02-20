using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Particles Data", menuName = "Utils/Particles Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class ParticleData : ScriptableObject
{
	public int id;
	public float lifeTime;
	public float delayBetweenParticles;
	public int frames;
	public int row;
	public Texture texture;
	public bool isLocal;
	public bool flipX;
	public bool flipY;
	public Vector2 offset;
}