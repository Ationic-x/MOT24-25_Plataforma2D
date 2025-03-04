using System;
using UnityEngine;
using static EnemyStates;

public class EnemyAnimation : ActorAnimation
{
    private EnemyController _enemyController;
    private EnemyStates _enemyStates;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        // FIXME: This should be modified
        _enemyController = GetComponentInParent<EnemyController>();
    }

    protected override void OnEnable()
    {

        // FIXME: This should be modified
        _enemyStates = _enemyController.ES;
        
        // When we're far the enemy is disabled and the animator is "resetted"
        // that is we force to use the currentstate if was changed before
        OnLogicStateChanged(_enemyStates.CurrentState);


        base.OnEnable();
    }

    private void OnLogicStateChanged(LogicState logicState)
    {
        switch (logicState) 
        { 
            case LogicState.None:
                break;
            case LogicState.Patrol:
                animator.SetInteger("AnimState", 2);
                break;
            default: 
                break;
        }
    }
    private void OnFacingRightChanged(bool isFacingRight)
    {
        // Should uses flip scale
        spriteRenderer.flipX = !isFacingRight;
    }

	protected override void SubscribeToEvents()
	{
		_enemyStates.OnLogicStateChanged += OnLogicStateChanged;
		_enemyStates.OnFacingRightChanged += OnFacingRightChanged;
	}

	protected override void UnsubscribeFromEvents()
	{
		_enemyStates.OnLogicStateChanged -= OnLogicStateChanged;
		_enemyStates.OnFacingRightChanged -= OnFacingRightChanged;
	}
}
