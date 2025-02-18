using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private ParticleController _particleController;
    private PlayerController _pc;
    private PlayerStates _ps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AE_SlideDust()
    {
        _particleController.SetCollection(ParticleCollection.CollectionID.PlayerDust);
        _particleController.CurrentCollection.SetParticleEffect(PlayerDust.DustID.WallDust);
        _particleController.Play();
    }
}
