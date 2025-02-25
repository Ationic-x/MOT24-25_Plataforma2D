using System;
using System.Collections.Generic;
using UnityEngine;
using static ParticleCollection;

public class ParticleController
{
	private readonly List<ParticleCollection> _listParticles;
	private ParticleCollection _currentCollection;


	public CollectionID CurrentCollectionID { get => _currentCollection.ID; }
	public ParticleCollection CurrentCollection { get => _currentCollection; }

	public ParticleController(List<ParticleCollection> listParticles)
	{
		_listParticles = listParticles;
	}

	public void SetCollection(CollectionID id)
	{
		if (_currentCollection != null && _currentCollection.ID.Equals(id)) return;
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

		SetCollection(tempID);
	}

	public void Play()
	{
		_currentCollection.Play();
	}

	public void PlayInLoop()
	{
		_currentCollection.PlayInLoop();
	}

	public void Emit(Vector3 position)
	{
		_currentCollection.Emit(position);
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
