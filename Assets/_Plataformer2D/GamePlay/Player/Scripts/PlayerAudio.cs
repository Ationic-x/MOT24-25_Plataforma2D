using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class PlayerAudio : MonoBehaviour
{
    // [SerializeField] private AudioManager _am;
    private PlayerController _pc;
    private PlayerStates _ps => _pc.PS;

    // States to avoid reproduce multiple times
    private bool _audioJumping;
    private bool _audioWallJumping;
    [SerializeField] private List<AudioClip> _playerClips;

    public enum Sound
    {
        JumpStart,
        JumpLand,
    }

    // Start is called before the first frame update
    void Awake()
    {
        _pc = GetComponent<PlayerController>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!_audioJumping && (_ps.IsJumping || _ps.IsFalling))
		{
            _audioJumping = true;
            PlaySoundOnce("JumpStart", GetAudioClip(Sound.JumpStart), 1);
		}

        if (!_audioWallJumping && _ps.IsWallJumping)
		{
            _audioWallJumping = true;
            _audioJumping = true;
            PlaySoundOnce("JumpStart", GetAudioClip(Sound.JumpStart), 1);
        }

        if (_audioJumping && _ps.IsGrounded)
		{
            _audioJumping = false;
            _audioWallJumping = false;
            PlaySoundOnce("JumpLand", GetAudioClip(Sound.JumpLand), 1);
        }
    }


    private AudioClip GetAudioClip(Sound sound)
    {
        switch (sound)
        {
            case Sound.JumpStart:
                int randomIndex = Random.Range(0, 1);
                return _playerClips[randomIndex];
            case Sound.JumpLand:
                return _playerClips[2];
        }

        Debug.LogWarning($"Audio not found: {sound}");
        return null;
    }
}
