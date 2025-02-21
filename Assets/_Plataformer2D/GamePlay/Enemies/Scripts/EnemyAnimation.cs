using UnityEngine;
using static EnemyStates;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sr;
    private EnemyController _ec;
    private EnemyStates _es;

    // Start is called before the first frame update
    void Awake()
    {
        _ec = GetComponentInParent<EnemyController>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _es = _ec.ES;

        _es.OnLogicStateChanged += OnLogicStateChanged;
        _es.OnFacingRightChanged += OnFacingRightChanged;
        
        // When we're far the enemy is disabled and the animator is "resetted"
        // that is we force to use the currentstate if was changed before
        OnLogicStateChanged(_es.CurrentState);
    }

    private void OnDisable()
    {
        _es.OnLogicStateChanged -= OnLogicStateChanged;
        _es.OnFacingRightChanged -= OnFacingRightChanged;
    }

    private void OnLogicStateChanged(LogicState logicState)
    {
        switch (logicState) 
        { 
            case LogicState.None:
                break;
            case LogicState.Patrol:
                _animator.SetInteger("AnimState", 2);
                break;
            default: 
                break;
        }
    }
    private void OnFacingRightChanged(bool isFacingRight)
    {
        _sr.flipX = !isFacingRight;
    }
}
