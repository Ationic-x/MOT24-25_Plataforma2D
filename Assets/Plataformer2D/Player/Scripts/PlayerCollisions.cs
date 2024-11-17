using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerStates ps;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private Transform frontWallCheckPoint;
    [SerializeField] private Transform backWallCheckPoint;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1f);
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        // False default values
        bool isGrounded = false, isCollidedRight = false, isCollidedLeft = false;

        // Exceptions
        if (!ps.IsDashing && !ps.IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
                isGrounded = true;
        }

        if (!ps.IsWallJumping)
        {
            bool frontCollider = Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer);
            bool backCollider = Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer);

            //Right Wall Check
            isCollidedRight = ps.IsFacingRight ? frontCollider : backCollider;

            //Left Wall Check
            isCollidedLeft = ps.IsFacingRight ? backCollider : frontCollider;
        }


        ps.IsGrounded = isGrounded;
        ps.IsCollidedRight = isCollidedRight;
        ps.IsCollidedLeft = isCollidedLeft;
    }
}
