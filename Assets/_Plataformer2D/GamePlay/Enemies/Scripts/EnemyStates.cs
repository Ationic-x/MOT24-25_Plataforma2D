using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStates
{
    // Logic States, define what action is doing
    public enum LogicState
    {
        None = 0,
        Patrol,
    }

    [SerializeField] private LogicState _currentState;

    // Events
    public event Action<LogicState> OnLogicStateChanged;

    // PhysicState, define in which situation/s is
    
    // General States
    [SerializeField] private bool _isFacingRight = true; // By default the character is facing right
    
    // Events
    public event Action<bool> OnFacingRightChanged;


    #region Logic States
    public LogicState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnLogicStateChanged?.Invoke(_currentState);
            }
        }
    }
    #endregion

    #region Physic States
    public bool IsFacingRight
    {
        get => _isFacingRight;
        set
        {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                OnFacingRightChanged?.Invoke(_isFacingRight);
            }
        }
    }
    #endregion
}
