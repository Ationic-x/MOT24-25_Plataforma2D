using UnityEngine;

public static class AudioManager
{
    public static void PlaySoundOnce(string name, AudioClip audioClip, float volume)
    {
        string audioName = $"Sound_{name}";
        
        // Avoid multiple instances
        GameObject oldAudio = GameObject.Find(audioName);
        if (oldAudio)
            Object.Destroy(oldAudio); 

		GameObject soundGameObject = new GameObject(audioName);
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        // Destroy when audio end
        Object.Destroy(soundGameObject, audioSource.clip.length);
    }
}
