using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager reference;
    AudioSource source;
    [SerializeField] //order is the same as in the sound effect folder (excluding (dis)connection, lose and win sfx)
    List<AudioClip> soundEffects = new List<AudioClip>();

    void Start()
    {
        reference = this;
        source = GetComponent<AudioSource>();
        Ball.reference.Disable.AddListener(() => Play(4));
    }

    public void Play(int audioIndex)
    {
        source.PlayOneShot(soundEffects[audioIndex]);
    }

    public void SetVolume(float volume)
    {
        source.volume = Mathf.Clamp01(volume);
    }
}
