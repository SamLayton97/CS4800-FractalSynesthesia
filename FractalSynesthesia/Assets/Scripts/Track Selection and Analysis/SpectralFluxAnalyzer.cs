using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Analyzes spectral flux of the current moment
/// of a audio clip. Used for algorithmic beat
/// mapping.
/// Credit to Jesse of Giant Scam Industries
/// https://medium.com/giant-scam/algorithmic-beat-mapping-in-unity-real-time-audio-analysis-using-the-unity-api-6e9595823ce4
/// </summary>
public class SpectralFluxAnalyzer
{
    // analysis configuration variables
    [Range(15, 60)]
    [SerializeField] int thresholdWindowSize = 30;          // size of window within which analyzer compares beats with non-beats
    [Range(0.1f, 10f)]
    [SerializeField] float beatInsensitivity = 1f;          // multiplier of how insensitive beat mapping is -- higher value requires stronger beat

    // analysis support variables
    float[] prevSpectrum = new float[1024];                 // FFT spectrum of audio clip on previous frame
    float[] currSpectrum = new float[1024];                 // FFT spectrum of audio clip on current frame
    List<SpectralFluxInfo> fluxSamples =                    // collection of spectal flux samples used to compare beats with non-beats
        new List<SpectralFluxInfo>();

    /// <summary>
    /// Analyzes spectral flux of FFT spectrum data
    /// of an audio clip.
    /// </summary>
    /// <param name="spectrum">FFT spectrum data</param>
    /// <param name="time">temporal position in song (seconds)</param>
    public void AnalyzeSpectrum(float[] spectrum, float time)
    {
        // update previous spectrum data to find flux next frame
        currSpectrum.CopyTo(prevSpectrum, 0);
        spectrum.CopyTo(currSpectrum, 0);

        // get current spectral flux from spectrum
        SpectralFluxInfo newInfo = new SpectralFluxInfo();
        newInfo.time = time;
        newInfo.spectralFlux = CalculateRectifiedSpectralFlux();
        fluxSamples.Add(newInfo);

        // if sample collection is large enough to analyze beats from
        if (fluxSamples.Count >= thresholdWindowSize)
        {
            // get threshold to consider beats

        }

    }

    /// <summary>
    /// Finds aggregate positive difference between
    /// current and previous spectrum data.
    /// </summary>
    /// <returns>aggregate positive difference</returns>
    float CalculateRectifiedSpectralFlux()
    {
        // calculate aggregate positive difference in data
        float aggregate = 0;
        for (int i = 0; i < 1024; i++)
            aggregate += Mathf.Max(0, currSpectrum[i] - prevSpectrum[i]);

        // return sum of positive changes
        return aggregate;
    }

    /// <summary>
    /// Finds threshold flux must surpass to be considered
    /// an onset -- culls non-beat changes.
    /// </summary>
    /// <param name="spectralFluxIndex"></param>
    /// <returns></returns>
    float GetFluxThreshold(int spectralFluxIndex)
    {
        // determine how far forward and back to make threshold window
        int startIndex = Mathf.Max(0, spectralFluxIndex - thresholdWindowSize / 2);
        int endIndex = Mathf.Min(spectralFluxIndex - 1, spectralFluxIndex + thresholdWindowSize / 2);

        // find average spectal flux across window
        float threshold = 0f;
        for (int i = startIndex; i < endIndex; i++)
            threshold += fluxSamples[i].spectralFlux;
        threshold /= (endIndex - startIndex);

        // return threshold (average) multiplied by custom sensitivity
        return threshold * beatInsensitivity;
    }
}

/// <summary>
/// Simple container for spectral flux data
/// </summary>
public class SpectralFluxInfo
{
    public float time;                  // time in audio track info was gathered
    public float spectralFlux;          // aggregate of positive change in spectrum data between frames
    public float threshold;             // change threshold other fluxes must exceed to be considered an onset
    public float prunedSpectralFlux;    // aggregate of positive changes that exceed beat threshold
    public bool isBeat;                 // whether flux at this point in time is a beat in song
}
