using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    // Collision States
    [SerializeField] private bool leftCollision;
    [SerializeField] private bool rightCollision;
    [SerializeField] private bool upCollision;
    [SerializeField] private bool downCollision;

    // Cinematic States
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isMoving;

    public bool LeftCollision { get; set; }
    public bool RightCollision { get; set; }
    public bool UpCollision { get; set; }
    public bool DownCollision { get; set; }

    public bool IsGrounded { get; set; }
    public bool IsMoving { get; set; }
}
