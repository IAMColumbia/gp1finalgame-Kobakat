using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource Source = null;

    #region Player audio clips
    public AudioClip jumpClip = null;
    public AudioClip dieClip = null;
    public AudioClip coinClip = null;
    public AudioClip stompClip = null;
    public AudioClip blockClip = null;
    public AudioClip skidClip = null;
    #endregion

    public int priority = 128;
    public float volume = 0.85f;
    private void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();

        Source.priority = priority;
        Source.volume = volume;
    }

    public void PlayClip(AudioClip clip)
    {
        Source.clip = clip;
        Source.PlayOneShot(Source.clip);
    }

    public void StopClip()
    {
        Source.Stop();
    }
}
