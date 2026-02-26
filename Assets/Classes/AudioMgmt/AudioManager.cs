using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject audioPlayer;
    [SerializeField] private List<AudioEffect> audioEffects = new();
    private readonly Dictionary<string, AudioSource> privateDict = new();

    public void InitializeAudioDictionary()
    {
        if (audioEffects.Count == 0) return;

        foreach (AudioEffect audioEffect in audioEffects)
        {
            AudioSource player = audioPlayer.AddComponent<AudioSource>();

            player.playOnAwake = false;
            player.loop = audioEffect.loop;
            player.pitch = audioEffect.pitch;
            player.clip = audioEffect.clip;
            player.volume = audioEffect.volume;
            player.spatialBlend = audioEffect.spatialBlend;

            if (player.spatialBlend > 0){
                player.minDistance = audioEffect.minDistance;
                player.maxDistance = audioEffect.maxDistance;
                player.spread = 360f;
                player.dopplerLevel = 0f;
            }

            privateDict.Add(audioEffect.name, player);
        }
    }

    public void PlayAudioSource(string name, float startTime = 0.0f)
    {
        AudioSource player = GetAudioSource(name);
        if (player == null) return;

        if (startTime == 0.0f || (startTime > 0 && !player.isPlaying))
        {
            player.time = startTime;
            player.Play();
        }
    }

    public void PauseAudioSource(string name)
    {
        AudioSource player = GetAudioSource(name);
        if (player != null)
            player.Pause();
    }

    public void UnPauseAudioSource(string name)
    {
        AudioSource player = GetAudioSource(name);
        if (player != null)
            player.UnPause();
    }

    public void StopAudioSource(string name)
    {
        AudioSource player = GetAudioSource(name);
        if (player != null && player.isPlaying)
            player.Stop();
    }
    
    public AudioSource GetAudioSource(string name)
    {
        if (!privateDict.ContainsKey(name))
        {
            Debug.Log($"{name} not found in audio dictionary");
            return null;
        }

        return privateDict[name];
    }
}
