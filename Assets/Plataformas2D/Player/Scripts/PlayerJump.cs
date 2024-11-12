using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private float jumpForce = 8f;
    private float gravity = -9.81f;
    [SerializeField] private float impulse;

    private BoxCollider2D bc;
    private PlayerStates ps;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        ps = GetComponent<PlayerStates>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchingGround();  
        MovesPlayer();
    }

    private void MovesPlayer()
    {
        transform.Translate(new Vector3(0, impulse, 0) * Time.deltaTime);
        //rb.velocity = new Vector2(rb.velocity.x, impulse);

        if (!ps.IsGrounded)
        {
            impulse += impulse > 0 ? gravity * Time.deltaTime : gravity * 2 * Time.deltaTime;
        }

        if (ps.IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            impulse = jumpForce;
        }
    }

    private void TouchingGround()
    {
        if (ps.DownCollision && impulse < 0)
        {
            transform.Translate(new Vector3(0, -impulse, 0) * Time.deltaTime);
            impulse = 0;
            ps.IsGrounded = true;
        }
        else if (!ps.DownCollision)
        {
            ps.IsGrounded = false;
        }
    }
}
