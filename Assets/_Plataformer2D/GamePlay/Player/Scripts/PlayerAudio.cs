using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

// FIXXME: SHOULD BE CHANGED WITH EVENTS
public class PlayerAudio : MonoBehaviour
{
    // [SerializeField] private AudioManager _am;
    private PlayerController _playerController;
    private PlayerStates _playerStates => _playerController.PlayerStates;

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
        _playerController = GetComponent<PlayerController>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!_audioJumping && (_playerStates.IsJumping || _playerStates.IsFalling))
		{
            _audioJumping = true;
            PlaySoundOnce("JumpStart", GetAudioClip(Sound.JumpStart), 1);
		}

        if (!_audioWallJumping && _playerStates.IsWallJumping)
		{
            _audioWallJumping = true;
            _audioJumping = true;
            PlaySoundOnce("JumpStart", GetAudioClip(Sound.JumpStart), 1);
        }

        if (_audioJumping && _playerStates.IsGrounded)
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
