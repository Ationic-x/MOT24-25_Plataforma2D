using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private float jumpForce = 10;
    private float gravity = -9.81f;
    [SerializeField] private float impulse;
    private bool isGrounded = false;
    private BoxCollider2D bc;
    private float maxDistance = 1f;


    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isGrounded) {
            impulse += gravity * Time.deltaTime;
            transform.Translate(new Vector3(0, impulse, 0) * Time.deltaTime);
        }


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            impulse = jumpForce;
        }


        RaycastHit2D hit = Physics2D.BoxCast(
            bc.bounds.center,
            bc.size,
            0f,
            Vector2.down,
            maxDistance
        );

        // Visualización del BoxCast
        DrawBoxCast(bc.bounds.center, bc.size, maxDistance, hit.collider != null);

        if (hit.collider && impulse < 0)
        {
            impulse = 0;
            isGrounded = true;
        }
    }

    private void DrawBoxCast(Vector3 center, Vector2 size, float distance, bool hit)
    {
        Color color = hit ? Color.red : Color.green;

        // Líneas que representan el BoxCast
        Vector3 bottomLeft = center + new Vector3(-size.x / 2, -size.y / 2, 0);
        Vector3 bottomRight = center + new Vector3(size.x / 2, -size.y / 2, 0);
        Vector3 topLeft = center + new Vector3(-size.x / 2, size.y / 2, 0);
        Vector3 topRight = center + new Vector3(size.x / 2, size.y / 2, 0);

        // Dibuja líneas para el BoxCast
        Debug.DrawLine(bottomLeft, bottomRight, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(bottomRight, topRight, color);

        // Dibuja líneas hacia abajo para representar la distancia del BoxCast
        Debug.DrawLine(bottomLeft, bottomLeft + Vector3.down * distance, color);
        Debug.DrawLine(bottomRight, bottomRight + Vector3.down * distance, color);
        Debug.DrawLine(topLeft, topLeft + Vector3.down * distance, color);
        Debug.DrawLine(topRight, topRight + Vector3.down * distance, color);
    }
}
