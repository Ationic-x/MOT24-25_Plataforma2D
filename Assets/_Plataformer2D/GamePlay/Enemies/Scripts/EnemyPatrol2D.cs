using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyStates;

public class EnemyPatrol2D : MonoBehaviour
{
    [SerializeField] private float _patrolDistance = 0.1f;

    private Vector2 _positionA, _positionB;

    private EnemyController _ec;
    private Rigidbody2D _rb;
    private bool _reachA = true, _reachB;

    [SerializeField] private GameObject _pointA, _pointB;
    private EnemyData _data;
    private SpriteRenderer _sr;
    private EnemyStates _es;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _ec = GetComponent<EnemyController>();
        _data = _ec.Data;
        _rb = GetComponent<Rigidbody2D>();
        _es = _ec.ES;
        _positionA = _pointA.transform.position;
        _positionB = _pointB.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_es.CurrentState != LogicState.Patrol) return;
        
        int direction = CheckTarget();
        _ec.SwapFacingPosition(direction > 0);
        Move(direction);
    }

    private int CheckTarget()
    {
        Vector2 target = Vector2.zero;
        if (_reachA) target = _positionB;
        if (_reachB) target = _positionA;

        float distance = target.x - transform.position.x;
        if (Mathf.Abs(distance) < _patrolDistance)
        {
            _reachA = !_reachA;
            _reachB = !_reachB;
        }

        return distance > 0 ? 1 : -1;
    }

    private void Move(int direction)
    {
        float targetSpeed = _data.runMaxSpeed * direction;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data.runAccelAmount : _data.runDeccelAmount;
        float speedDif = targetSpeed - _rb.linearVelocity.x;
        float movement = speedDif * accelRate;
        _rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
