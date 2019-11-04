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
    public float[] frequencyBands = new float[8];          // array storing amplitudes of simplified frequency bands
    int[] sampleCounts = new int[8];                // array storing amount of samples covered by each band
    float frequencyMagnifier = 10f;                 // simple scalar to magnify small values of frequency bands

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

        // initialize sample count for each frequency band
        // NOTE: hz covered by each frequency band based
        // on: https://www.youtube.com/watch?v=mHk3ZiKNH48
        for (int i = 0; i < frequencyBands.Length; i++)
        {
            sampleCounts[i] = (int)Mathf.Pow(2, i) * 2;
            if (i == frequencyBands.Length - 1)
                sampleCounts[i] += 2;

            Debug.Log(sampleCounts[i]);
        }

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

        // update frequency bands
        // NOTE: calculations for each band based on: https://www.youtube.com/watch?v=mHk3ZiKNH48
        int currSample = 0;
        for (int i = 0; i < frequencyBands.Length; i++)
        {
            // calculate average value over sample range
            float sampleSum = 0;
            for (int j = 0; j < sampleCounts[i]; j++)
            {
                sampleSum += samples[currSample] * (currSample + 1);
                currSample++;
            }
            frequencyBands[i] = sampleSum / currSample * frequencyMagnifier;
        }

    }

    #endregion

}
