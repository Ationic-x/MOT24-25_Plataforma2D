using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HHBoxesController
{
    private readonly Transform _target;
    private readonly BoxCollider2D _targetCollider;
    private readonly Dictionary<string, HHBox> _hitbox;
    private readonly Dictionary<string, HHBox[]> _hurtbox;
    private LayerMask _layerMask;

    // Temporal just for debug
    private HHBox _currentHitbox;
    private HHBox[] _currentHurtboxes;

    public HHBoxesController(IHHBoxesDictionary dictionary, Transform target, LayerMask layerMask)
    {
        _target = target;
        _targetCollider = _target.GetComponent<BoxCollider2D>();
        _hitbox = dictionary.Hitbox;
        _hurtbox = dictionary.Hurtbox;
        _layerMask = layerMask;
    }

    public void UpdateBoxes(Tuple<string, string> animationRef)
    {
        if (_hitbox.TryGetValue(animationRef.Item1, out HHBox hitbox))
        {
            if (!_currentHitbox.Equals(hitbox)) {
                _targetCollider.size = hitbox.size;
                _targetCollider.offset = hitbox.offset;
                _currentHitbox = hitbox;
            }
        }

        if (_hurtbox.TryGetValue(animationRef.Item1, out HHBox[] hurtboxes))
        {
			if (_currentHurtboxes == null || !_currentHurtboxes.Equals(hurtboxes)) _currentHurtboxes = hurtboxes;

			foreach (HHBox hurtbox in _currentHurtboxes)
            {
                // in future we will handle
                Collider2D[] colliders = Physics2D.OverlapBoxAll(_target.position + hurtbox.offset, hurtbox.size, 0, _layerMask);
                foreach (Collider2D collider in colliders)
                {
                    Debug.Log(_target.name + "--" + collider.name);
                }
            }
        }
        else _currentHurtboxes = null;
    }

    public HHBox CurrentHitbox { get => _currentHitbox; }
    public HHBox[] CurrentHurtboxes { get => _currentHurtboxes; }
}
