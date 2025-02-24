using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class HHBoxesController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private IHHBoxesDictionary _hhBoxesDictionary;
    [SerializeField] private bool _drawGizmo;
    private Dictionary<string, HHBox> _hitbox;
    private Dictionary<string, HHBox> _hurtbox;
    
    // Temporal just for debug
    private HHBox _currentBox;


    public void OnEnable()
    {
        _hhBoxesDictionary = GetComponent<IHHBoxesDictionary>();
        _hitbox = _hhBoxesDictionary.Hitbox;
        _hitbox = _hhBoxesDictionary.Hurtbox;
    }

    public void UpdateBoxes(string sprite)
    {
        Debug.Log(sprite);
        Debug.Log(_hitbox.Count);
        foreach (var key in _hitbox.Keys)
        {
            Debug.Log("++"+key);
        }
        if (_hitbox.TryGetValue(sprite, out HHBox box))
        {
            Physics2D.OverlapBox(_target.position + box.offset, box.size, 0);
            _currentBox = box;
            Debug.Log(_currentBox.offset + "--" + _currentBox.size);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (_drawGizmo)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(_target.position + _currentBox.offset, _currentBox.size);

            Gizmos.color = Color.red;
        }
    }
}
