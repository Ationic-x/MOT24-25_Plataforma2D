using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

public class PlayerMove : MonoBehaviour
{
    private float inputX;
    [SerializeField] private float speedX = 5f;
    private SpriteRenderer sr;
    private PlayerStates ps;


    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<PlayerStates>();
        sr = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        ps.IsMoving = (inputX != 0);

        MovesPlayer();
        Flip();
    }

    private void MovesPlayer()
    {
        if (inputX < 0 && ps.LeftCollision || inputX > 0 && ps.RightCollision)
            return;

        transform.position += Vector3.right * inputX * speedX * Time.deltaTime;
    }

    private void Flip() 
    {
        if (inputX == 0)
            return;

        sr.flipX = inputX < 0 ? true : false;
    }
}
