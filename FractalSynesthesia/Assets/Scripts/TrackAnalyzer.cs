using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pulls spectrum data from a given audio track.
/// Also handles playing of tracks.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TrackAnalyzer : MonoBehaviour
{
    // audio support variables
    AudioSource myAudioSource;          // audio source to play tracks from

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // retrieve and initialize audio source
        myAudioSource = GetComponent<AudioSource>();
        if (!myAudioSource.clip)
            myAudioSource.clip = Resources.Load<AudioClip>("Gorillaz - On Melancholy Hill");

        // play track
        myAudioSource.Play();
    }
}
