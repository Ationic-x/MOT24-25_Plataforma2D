using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HHBoxesController
{
    private readonly Transform _origin;
    private readonly BoxCollider2D _originCollider;
    private readonly Dictionary<Tuple<string, string>, HHBox> _hitbox;
    private readonly Dictionary<Tuple<string, string>, HHBox[]> _hurtbox;
    private LayerMask _layerMask;

    // Temporal just for debug
    private HHBox _currentHitbox;
    private HHBox[] _currentHurtboxes;

    public HHBoxesController(IHHBoxesData dictionary, Transform origin, LayerMask layerMask)
    {
        _origin = origin;
        _originCollider = _origin.GetComponent<BoxCollider2D>();
        _hitbox = dictionary.Hitbox;
        _hurtbox = dictionary.Hurtbox;
        _layerMask = layerMask;
    }

    public void UpdateBoxes(Tuple<string, string> animationRef)
    {
        if (_hitbox.TryGetValue(animationRef, out HHBox hitbox))
        {
            if (!_currentHitbox.Equals(hitbox)) {
                _currentHitbox = hitbox;
                _originCollider.size = _currentHitbox.size;
                _originCollider.offset = _currentHitbox.offset;
            }
        }

        if (_hurtbox.TryGetValue(animationRef, out HHBox[] hurtboxes))
        {
			if (_currentHurtboxes == null || !_currentHurtboxes.SequenceEqual(hurtboxes)) _currentHurtboxes = hurtboxes;

			foreach (HHBox hurtbox in _currentHurtboxes)
            {
                CheckCollisions(hurtbox);
            }
        }
        else _currentHurtboxes = null;
    }

	private void CheckCollisions(HHBox hurtbox)
	{
		Collider2D[] colliders = Physics2D.OverlapBoxAll(_origin.position + hurtbox.offset, hurtbox.size, 0, _layerMask);
		foreach (Collider2D collider in colliders)
		{
            ActorController target = collider.GetComponent<ActorController>();
			Attack tmpAttack = _origin.GetComponent<ActorController>().GetAttack();
            // Store the ID and avoid multiple calls
            if (!target.DamageBuffer.Contains(tmpAttack.id)){
                target.DamageBuffer.Push(tmpAttack.id);
				Debug.Log(tmpAttack.id);
            }
		}
	}

	public HHBox CurrentHitbox { get => _currentHitbox; }
    public HHBox[] CurrentHurtboxes { get => _currentHurtboxes; }
}
