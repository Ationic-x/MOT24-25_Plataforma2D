using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HHBoxesController
{
    private Transform _target;
    private Dictionary<string, HHBox> _hitbox;
    private Dictionary<string, HHBox> _hurtbox;

    // Temporal just for debug
    private HHBox _currentBox;

    public HHBoxesController(IHHBoxesDictionary dictionary, Transform target)
    {
        _target = target;
        _hitbox = dictionary.Hitbox;
        _hurtbox = dictionary.Hurtbox;
    }

    public void UpdateBoxes(string sprite)
    {
        if (_hitbox.TryGetValue(sprite, out HHBox box))
        {
            Physics2D.OverlapBox(_target.position + box.offset, box.size, 0);
            _currentBox = box;
        }
        else
        {
            Debug.LogWarning("Hitbox not foound in the sprite : " + sprite);
        }
    }

    public HHBox CurrentBox { get => _currentBox; }
}
