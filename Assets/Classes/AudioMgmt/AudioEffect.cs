using UnityEngine;

[CreateAssetMenu(fileName = "AudioEffect", menuName = "Scriptable Objects/AudioEffect")]
public class AudioEffect : ScriptableObject
{
    [Header("==AudioEffect Properties==")]
    public AudioClip clip;
    public float volume = 0.4f;
    public float pitch = 1f;
    public bool loop = false;

    [Header("==Distance-Based Audio Settings==")]
    public float spatialBlend = 0f;
    public float minDistance = 0f;
    public float maxDistance = 0f;
}
