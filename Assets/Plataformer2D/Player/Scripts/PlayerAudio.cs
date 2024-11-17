using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioManager am;
    private PlayerStates ps;

    // States to avoid reproduce multiple times
    private bool audioJumping;
    private bool audioWallJumping;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioJumping && (ps.IsJumping || ps.IsJumpFalling))
		{
            audioJumping = true;
            am.PlaySoundOnce(AudioManager.Sound.JumpStart, 1);
		}

        if (!audioWallJumping && ps.IsWallJumping)
		{
            audioWallJumping = true;
            audioJumping = true;
            am.PlaySoundOnce(AudioManager.Sound.JumpStart, 1);
        }

        if (audioJumping && ps.IsGrounded)
		{
            audioJumping = false;
            audioWallJumping = false;
            am.PlaySoundOnce(AudioManager.Sound.JumpLand, 1);
        }
    }
}
