using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerStates _ps;

    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    [SerializeField] private LayerMask _groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        // False default values
        bool isGrounded = false, isCollidedRight = false, isCollidedLeft = false;

        // Exceptions
        if (!_ps.IsDashing && !_ps.IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
                isGrounded = true;
        }

        if (!_ps.IsWallJumping)
        {
            bool frontCollider = Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);
            bool backCollider = Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);

            //Right Wall Check
            isCollidedRight = _ps.IsFacingRight ? frontCollider : backCollider;

            //Left Wall Check
            isCollidedLeft = _ps.IsFacingRight ? backCollider : frontCollider;
        }


        _ps.IsGrounded = isGrounded;
        _ps.IsCollidedRight = isCollidedRight;
        _ps.IsCollidedLeft = isCollidedLeft;
    }
}
