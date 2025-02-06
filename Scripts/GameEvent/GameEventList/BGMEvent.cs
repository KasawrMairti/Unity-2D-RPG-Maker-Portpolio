using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMEvent : GameEvent
{
    public AudioClip clip; // À½¾Ç ÆÄÀÏ

    private AudioSource audioSource;

    protected override void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override IEnumerator Event()
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

        yield return null;

        IsEventEnd = true;
    }
}
