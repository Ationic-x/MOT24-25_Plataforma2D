using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private ParticleController _particleController;
    private PlayerController _pc;
    private PlayerStates _ps;
    private bool orientation;

    void Awake()
    {
        _pc = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        _ps = _pc.PS;
        orientation = _ps.IsFacingRight;
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
        
        if (orientation != _ps.IsFacingRight)
        {
            orientation = _ps.IsFacingRight;
            _particleController.CurrentCollection.HorizontalFlip();
        }


		_particleController.Emit(currentPostion);
	}
}
