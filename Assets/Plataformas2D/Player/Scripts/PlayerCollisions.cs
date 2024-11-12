using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private BoxCollider2D bc;
    [SerializeField] private float maxDistance = 0f;
    [SerializeField] private bool debug = false;
    private PlayerStates ps;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        ps = GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        Vector3 center = bc.bounds.center;
        Vector2 size = bc.size;

        RaycastHit2D hitUp = Physics2D.BoxCast(center, size, 0f, Vector2.up, 0);
        RaycastHit2D hitDown = Physics2D.BoxCast(center, size, 0f, Vector2.down, 0);
        RaycastHit2D hitLeft = Physics2D.BoxCast(center, size, 0f,Vector2.left, 0);
        RaycastHit2D hitRight = Physics2D.BoxCast(center, size, 0f, Vector2.right, 0);

        ps.UpCollision = hitUp.collider;
        ps.DownCollision = hitDown.collider;
        ps.LeftCollision = hitLeft.collider;
        ps.RightCollision = hitRight.collider;
        Debug.Log(hitDown.point);
        if (debug)
        {
            Vector3 bottomLeft = center + new Vector3(-1,-1,0) * maxDistance + new Vector3(-size.x / 2, -size.y / 2, 0);
            Vector3 bottomRight = center + new Vector3(1, -1, 0) * maxDistance + new Vector3(size.x / 2, -size.y / 2, 0);
            Vector3 topLeft = center + new Vector3(-1, 1, 0) * maxDistance + new Vector3(-size.x / 2, size.y / 2, 0);
            Vector3 topRight = center + new Vector3(1, 1, 0) * maxDistance + new Vector3(size.x / 2, size.y / 2, 0);

            Debug.DrawLine(bottomLeft, bottomRight, hitDown.collider ? Color.red : Color.green);
            Debug.DrawLine(bottomLeft, topLeft, hitLeft.collider ? Color.red : Color.green);
            Debug.DrawLine(topLeft, topRight, hitUp.collider ? Color.red : Color.green);
            Debug.DrawLine(bottomRight, topRight, hitRight.collider ? Color.red : Color.green);
        }
    }
}
