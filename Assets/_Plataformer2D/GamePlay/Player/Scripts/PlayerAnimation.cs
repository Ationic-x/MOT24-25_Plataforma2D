using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _animator;
    private SpriteRenderer _sr;
    private PlayerStates _ps;

    [SerializeField] private GameObject _slideDust;
    [SerializeField] private Transform _frontWallCheckPoint;

    // Start is called before the first frame update
    void Awake()
    {
        _ps = GetComponentInParent<PlayerStates>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Grounded", _ps.IsGrounded);
        _animator.SetBool("Jump", _ps.IsJumping || _ps.IsWallJumping);
        _animator.SetFloat("AirSpeedY", _ps.IsJumpFalling ? -1 : 0);
        _animator.SetBool("WallSlide", _ps.IsSliding);
        _animator.SetInteger("AnimState", _ps.IsRunning ? 1 : 0);
        _animator.SetBool("Roll", _ps.IsDashing);
    }

    // Animation Event to spawn a dust cloud when slide
    private void AE_SlideDust()
    {
        GameObject dust = Instantiate(_slideDust, _frontWallCheckPoint.position, gameObject.transform.localRotation);
        dust.transform.localScale = new Vector3(_ps.IsFacingRight ? 1 : -1, 1, 1);
    }
}

