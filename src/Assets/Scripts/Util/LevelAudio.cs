using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudio : MonoBehaviour
{
    public AudioSource Source = null;
    public int priority = 64;
    public float volume = 0.55f;
    #region Level audio clips
    public AudioClip musicIntro = null;
    public AudioClip musicScore = null;
    public AudioClip musicDeath = null;
    public AudioClip musicWin = null;

    #endregion


    public void Initialize()
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
