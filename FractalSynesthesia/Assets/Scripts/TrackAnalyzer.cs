﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pulls spectrum data from a given audio track.
/// Also handles playing of tracks.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class TrackAnalyzer : MonoBehaviour
{
    // audio support variables
    AudioSource myAudioSource;          // audio source to play tracks from

    // pseudo-singleton support
    static TrackAnalyzer instance;

    /// <summary>
    /// Read-access property returning this instance of track analyzer
    /// </summary>
    public static TrackAnalyzer Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// Read-access property returning the length of the track in seconds
    /// </summary>
    public float TrackLength
    {
        get
        {
            // get audio source component if null
            if (!myAudioSource)
                myAudioSource = GetComponent<AudioSource>();

            // return track length
            return myAudioSource.clip.length;
        }
    }

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // set universally-retrievable instance to self
        instance = this;

        // retrieve and initialize audio source
        if (!myAudioSource) 
            myAudioSource = GetComponent<AudioSource>();
        if (!myAudioSource.clip)
            myAudioSource.clip = Resources.Load<AudioClip>("Gorillaz - On Melancholy Hill");

        // start track
        myAudioSource.Play();
    }
}
