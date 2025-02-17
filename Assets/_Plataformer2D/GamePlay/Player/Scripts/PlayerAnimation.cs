using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _animator;
    private SpriteRenderer _sr;
    private PlayerController _pc;
    private PlayerStates _ps;
    private GhosTrail _gt;

    [SerializeField] private GameObject _slideDust;
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    
    private float _ghostTrailDelay;

    // Start is called before the first frame update
    void Awake()
    {
        _pc = GetComponentInParent<PlayerController>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _gt = GetComponent<GhosTrail>();
    }

	private void OnEnable()
	{
        _ps = _pc.PS;

        _ps.OnFacingRightChanged += OnFacingRightChanged;
    }

	private void OnDisable()
	{
        _ps.OnFacingRightChanged -= OnFacingRightChanged;
    }

	private void OnFacingRightChanged(bool isFacingRight)
	{
        _sr.flipX = !isFacingRight;
	}

	// FIXXME: SHOULD BE CHANGED WITH EVENTS
	void Update()
    {
        _animator.SetBool("Grounded", _ps.IsGrounded);
        _animator.SetBool("Jump", _ps.IsJumping || _ps.IsWallJumping);
        _animator.SetFloat("AirSpeedY", _ps.IsFalling ? -1 : 0);
        _animator.SetBool("WallSlide", _ps.IsSliding);
        _animator.SetInteger("AnimState", _ps.IsRunning ? 1 : 0);
        if (_ps.IsDashing)
		{
            _animator.SetBool("Roll", true);
            _gt.Available = true;
            _ghostTrailDelay = Time.time + 0.3f;
        }

        if (Time.time - _ghostTrailDelay > 0)
		{
            _gt.Available = false;
		}
    }

    // Animation Event to spawn a dust cloud when slide
    //private void AE_SlideDust()
    //{
    //    bool orientation = _ps.IsFacingRight;
        
    //    //GameObject dust = Instantiate(_slideDust, orientation ? _frontWallCheckPoint.position : _backWallCheckPoint.position, gameObject.transform.localRotation);
    //    //dust.transform.localScale = new Vector3(orientation ? 1 : -1, 1, 1);
    //}
}

