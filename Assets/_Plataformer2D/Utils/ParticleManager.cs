using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ParticleCollection;

public class ParticleManager : MonoBehaviour
{
	[SerializeField] private List<ParticleCollection> _listParticles;
	[SerializeField] private ParticleCollection _currentCollection;
	[SerializeField] private int _currentParticleEffect;


	public CollectionID CurrentCollectionID { get => _currentCollection.ID; }

	public void SetCollection(CollectionID id)
	{
		foreach (var item in _listParticles)
		{
			if (item.ID == id)
			{
				_currentCollection = item;
			}
		}
	}
	public void SetCollection(int id)
	{
		CollectionID tempID;
		if (!Enum.IsDefined(typeof(CollectionID), id)) 
		{
			Debug.Log($"The ID: {id} is not inside the list");
			return;
		}
		else tempID = (CollectionID)id;

		foreach (var item in _listParticles)
		{
			if (item.ID == tempID)
			{
				_currentCollection = item;
			}
		}
	}
	public void Play()
	{
		_currentCollection.Play();
	}

	public void PlayInLoop()
	{
		_currentCollection.PlayInLoop();
	}
	public void Stop()
	{
		_currentCollection.Stop();
	}
	public void Pause()
	{
		_currentCollection.Pause();
	}
}
