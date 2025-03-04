using System;
using UnityEngine;

[System.Serializable]
public class EnemyStates: ActorStates<Enum>
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

    // PhysicState, define in which situation/s


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
}
