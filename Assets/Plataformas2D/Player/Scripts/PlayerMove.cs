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
    // Start is called before the first frame update
    void Start()
    {
        
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

        Vector3 currentScale = transform.localScale;

        transform.localScale = new Vector3(
            inputX < 0 ? Math.Abs(currentScale.x) * - 1 : Math.Abs(currentScale.x), 
            currentScale.y, 
            currentScale.z
            );
    }
}
