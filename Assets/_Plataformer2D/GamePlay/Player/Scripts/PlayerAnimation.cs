using System;
using UnityEditor;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _animator;
    private PlayerController _playerController;
    private PlayerStates _playerStates;
    private GhosTrail _ghostTrail;
    private SpriteRenderer _spriteRenderer;
	[SerializeField] private string _currentSprite;
	[SerializeField] private string _currentClip;

	private float _ghostTrailDelay;

	// Events
	public event Action<Tuple<string, string>> OnSpriteChange;

	// Start is called before the first frame update
	void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _animator = GetComponent<Animator>();
        _ghostTrail = GetComponent<GhosTrail>();
    }

	private void OnEnable()
	{
        _playerStates = _playerController.PlayerStates;
        _spriteRenderer = GetComponent<SpriteRenderer>();
		_currentSprite = _spriteRenderer.sprite.name;
	}

	// FIXME: SHOULD BE CHANGED WITH EVENTS
	void Update()
    {
		string spriteName = _spriteRenderer.sprite.name;
        string clipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
		if (!_currentSprite.Equals(spriteName) || !_currentClip.Equals(clipName))
		{
			_currentClip = clipName;
			_currentSprite = spriteName;
			OnSpriteChange?.Invoke(Tuple.Create(_currentSprite, _currentClip));
		}

		_animator.SetBool("Grounded", _playerStates.IsGrounded);
        _animator.SetBool("Jump", _playerStates.IsJumping || _playerStates.IsWallJumping);
        _animator.SetFloat("AirSpeedY", _playerStates.IsFalling ? -1 : 0);
        _animator.SetBool("WallSlide", _playerStates.IsSliding);
        _animator.SetBool("Attack1", _playerStates.IsAttacking);
        _animator.SetInteger("AnimState", _playerStates.IsRunning ? 1 : 0);
        if (_playerStates.IsDashing)
		{
            _animator.SetBool("Roll", true);
            _ghostTrail.Available = true;
            _ghostTrailDelay = Time.time + 0.3f;
        }

        if (Time.time - _ghostTrailDelay > 0)
		{
            _ghostTrail.Available = false;
		}
    }
}

