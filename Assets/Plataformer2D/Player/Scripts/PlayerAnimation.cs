using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField] private GameObject slideDust;
    [SerializeField] private PlayerStates ps;
    [SerializeField] private Transform frontWallCheckPoint;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Grounded", ps.IsGrounded);
        animator.SetBool("Jump", ps.IsJumping || ps.IsWallJumping);
        animator.SetFloat("AirSpeedY", ps.IsJumpFalling ? -1 : 0);
        animator.SetBool("WallSlide", ps.IsSliding);
        animator.SetInteger("AnimState", ps.IsRunning ? 1 : 0);
        animator.SetBool("Roll", ps.IsDashing);
    }

    // Animation Event to spawn a dust cloud when slide
    private void AE_SlideDust()
    {
        GameObject dust = Instantiate(slideDust, frontWallCheckPoint.position, gameObject.transform.localRotation);
        dust.transform.localScale = new Vector3(ps.IsFacingRight ? 1 : -1, 1, 1);
    }
}

