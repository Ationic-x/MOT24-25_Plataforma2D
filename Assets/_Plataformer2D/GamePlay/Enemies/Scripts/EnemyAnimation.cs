using System;
using UnityEngine;
using static EnemyStates;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private EnemyController _enemyController;
    private EnemyStates _enemyStates;
    [SerializeField] private string _currentSprite;

    // Events
    public event Action<string> OnSpriteChange;

    // Start is called before the first frame update
    void Awake()
    {
        _enemyController = GetComponentInParent<EnemyController>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        string spriteName = _spriteRenderer.sprite.name;
        if (!_currentSprite.Equals(spriteName))
        {
            _currentSprite = spriteName;
            OnSpriteChange?.Invoke(_currentSprite);
        }
    }

    private void OnEnable()
    {
        _enemyStates = _enemyController.ES;

        _enemyStates.OnLogicStateChanged += OnLogicStateChanged;
        _enemyStates.OnFacingRightChanged += OnFacingRightChanged;

        _currentSprite = _spriteRenderer.sprite.name;
        
        // When we're far the enemy is disabled and the animator is "resetted"
        // that is we force to use the currentstate if was changed before
        OnLogicStateChanged(_enemyStates.CurrentState);
    }

    private void OnDisable()
    {
        _enemyStates.OnLogicStateChanged -= OnLogicStateChanged;
        _enemyStates.OnFacingRightChanged -= OnFacingRightChanged;
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
        _spriteRenderer.flipX = !isFacingRight;
    }
}
