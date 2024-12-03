using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float _patrolDistance = 0.1f;

    private Vector2 _positionA, _positionB;

    private EnemyController _ec;
    private Rigidbody2D _rb;
    private bool _reachA = true, _reachB;

    [SerializeField] private GameObject _pointA, _pointB;
    [SerializeField] private PlayerData _pd;
    private Animator _animator;
    private SpriteRenderer _sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _ec = GetComponent<EnemyController>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _positionA = _pointA.transform.position;
        _positionB = _pointB.transform.position;
    }

    private void Start()
    {
        _animator.SetInteger("AnimState", 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int direction = CheckTarget();
        SwapFacingPosition(direction);
        Move(direction);
    }

    public void SwapFacingPosition(int direction)
    {
        _sr.flipX = direction > 0 ? true : false;
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
        float targetSpeed = _pd.runMaxSpeed * direction;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _pd.runAccelAmount : _pd.runDeccelAmount;
        float speedDif = targetSpeed - _rb.linearVelocity.x;
        float movement = speedDif * accelRate;
        _rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
