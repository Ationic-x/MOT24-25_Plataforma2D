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
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        transform.position += Vector3.right * inputX * speedX * Time.deltaTime;
        flip();
    }

    private void flip() 
    {
        if (inputX == 0)
            return;

        sr.flipX = inputX < 0 ? true : false;
    }
}
