using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private GameObject _particlesCollection;
    private ParticleController _particleController;
    private PlayerController _playerController;
    private PlayerStates _playerStates;
    private bool orientation;

    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
		List<ParticleCollection> listParticles = _particlesCollection.GetComponents<ParticleCollection>().ToList();
		_particleController = new ParticleController(listParticles);
    }

    private void OnEnable()
    {
        _playerStates = _playerController.PlayerStates;
        orientation = _playerStates.IsFacingRight;
    }

    // FIXME: AE_SLIDEUST() Event is used in animation clip
    // Maybe is more insteresting using this kind of codes in loops controlled by script
    public void AE_SlideDust()
    {
        // We use emit instead start because ParticleSystem.play() is slower than
        // all the update and physics update, so there are some kind of behaviours
        // like throwing the particle when the character is not even more sliding in the wall
        Vector3 currentPostion = transform.position;

        _particleController.SetCollection(ParticleCollection.CollectionID.PlayerDust);
        _particleController.CurrentCollection.SetParticleEffect(DustCollection.DustID.WallDust);
        
        if (orientation != _playerStates.IsFacingRight)
        {
            orientation = _playerStates.IsFacingRight;
            _particleController.CurrentCollection.HorizontalFlip();
        }

		_particleController.Emit(currentPostion);
	}
}
