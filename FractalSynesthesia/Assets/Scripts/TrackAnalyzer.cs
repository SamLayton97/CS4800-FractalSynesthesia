using System.Collections;
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
    AudioSource myAudioSource;                      // audio source to play tracks from
    float[] samples = new float[1024];              // array of audio samples
    float[] frequencyBands = new float[8];          // array storing amplitudes of simplified frequency bands

    // pseudo-singleton support
    static TrackAnalyzer instance;

    #region Properties

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

    #endregion

    #region Unity Methods

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

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // retrieve spectrum data of audio clip
        myAudioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        // TODO: make frequency bands of spectrum data

    }

    #endregion

}
