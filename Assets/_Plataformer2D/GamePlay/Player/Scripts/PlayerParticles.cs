using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _particlesSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _particlesSystem.ForEach(i => i.Play());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
