using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Sound
    {
        JumpStart,
        JumpLand,
    }

    [SerializeField] private List<AudioClip> audioClips;


    public void PlaySoundOnce(Sound sound, float volume)
    {
        string audioName = $"Sound_{sound}";
        
        // Avoid multiple instances
        GameObject oldAudio = GameObject.Find(audioName);
        if (oldAudio)
            Destroy(oldAudio); 
		GameObject soundGameObject = new GameObject(audioName);
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.clip = GetAudioClip(sound);
        audioSource.volume = volume;
        audioSource.Play();

        // Destroy when audio end
        Destroy(soundGameObject, audioSource.clip.length);
    }


    public void PlaySoundLoop(Sound sound, float volume, bool loop)
    {
        GameObject soundGameObject = new GameObject($"Sound_{sound}");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.clip = GetAudioClip(sound);
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        // Destroy if the object dont have loop
        if (!loop)
        {
            Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        switch (sound)
        {
            case Sound.JumpStart:
                int randomIndex = Random.Range(0, 1);
                return audioClips[randomIndex];
            case Sound.JumpLand:
                return audioClips[2];
        }

        Debug.LogWarning($"Audio not found: {sound}");
        return null;
    }
}
