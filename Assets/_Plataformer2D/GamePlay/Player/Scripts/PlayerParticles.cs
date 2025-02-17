using System;
using System.Collections.Generic;
using UnityEngine;
using static ParticleCollection;

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
        _particleController.SetCollection(CollectionID.PlayerDust);
        _particleController.SetParticleEffect();
        _particleController.Play();
    }
}
