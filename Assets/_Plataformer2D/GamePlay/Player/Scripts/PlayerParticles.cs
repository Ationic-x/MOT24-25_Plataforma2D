using System;
using System.Collections.Generic;
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

    public void AE_SlideDust()
    {
        _particleController.SetCollection(ParticleCollection.CollectionID.PlayerDust);
        _particleController.CurrentCollection.SetParticleEffect(PlayerDust.DustID.WallDust);
        
        if (orientation != _ps.IsFacingRight)
        {
            orientation = _ps.IsFacingRight;
            _particleController.CurrentCollection.HorizontalFlip();
        }

        _particleController.Play();
    }
}
