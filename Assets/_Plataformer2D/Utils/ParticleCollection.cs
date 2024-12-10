using UnityEngine;

public abstract class ParticleCollection : MonoBehaviour
{
	private ParticleSystem Ps { get; set; }

	protected void SetParticleSystem(ParticleSystem particleSystem)
	{
		Ps = particleSystem;
	}

	private void Play()
	{
		Ps.Play();
	}

	private void Stop()
	{
		Ps.Stop();
	}
	
	private void Pause()
	{
		Ps.Pause();
	}
}
