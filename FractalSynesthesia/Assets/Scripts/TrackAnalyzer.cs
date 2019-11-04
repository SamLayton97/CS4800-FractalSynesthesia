﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    int[] sampleCounts = new int[8];                // array storing amount of samples covered by each band
    float frequencyMagnifier = 10f;                 // simple scalar to magnify small values of frequency bands

    // analysis variables
    float bandDevScale = 0f;                        // scale of deviation among frequency bands
    float dominantRange = 0f;                       // dominant range of frequency bands, ranging from 0 to 1
    float leadDominance = 0f;                       // measures how much lead voice is dominant over accompaniment

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

    /// <summary>
    /// Read-access property returning scale of
    /// deviation among frequency bands
    /// </summary>
    public float BandDeviationScale
    {
        get { return bandDevScale; }
    }

    /// <summary>
    /// Read-access property returning dominant
    /// range of frequency bands
    /// </summary>
    public float DominantRange
    {
        get { return dominantRange; }
    }

    /// <summary>
    /// Read-access property returning how dominant
    /// lead voice is over its accompaniment
    /// </summary>
    public float LeadVoiceDominance
    {
        get { return leadDominance; }
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

        // update standard deviation of frequency bands
        float bandAverage = frequencyBands.Average();
        bandDevScale = Mathf.Sqrt(frequencyBands.Select(x => (x - bandAverage) * (x - bandAverage)).Sum() / frequencyBands.Length) /
            Mathf.Max(frequencyBands.Max() - frequencyBands.Average(), frequencyBands.Average() - frequencyBands.Min());

        // update lead frequency range/voice and its dominance
        float maxBand = frequencyBands.Max();
        dominantRange = (float)System.Array.IndexOf(frequencyBands, maxBand) / (frequencyBands.Length - 1);
        leadDominance = 1 - (bandAverage / maxBand);
    }

    #endregion

}
