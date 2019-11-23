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
    // analysis support variables
    float[] prevSpectrum = new float[1024];                 // FFT spectrum of audio clip on previous frame
    float[] currSpectrum = new float[1024];                 // FFT spectrum of audio clip on current frame
    List<SpectralFluxInfo> fluxSamples =                    // collection of spectal flux samples used to compare beats with non-beats
        new List<SpectralFluxInfo>();
    int thresholdWindowSize = 30;                           // size of window within which analyzer compares beats with non-beats
    float beatInsensitivity = 1f;                           // multiplier of how insensitive beat mapping is -- higher value requires stronger beat
    int indexToProcess = 0;                                 // index representing "now" -- slightly back in time to analyze peaks

    /// <summary>
    /// Constructor for analyzer. Allows users
    /// to set custom parameters for beat analysis.
    /// </summary>
    public SpectralFluxAnalyzer(int thresholdWindowSize = 30, float beatInsensitivity = 1f)
    {
        // intialize configrable data
        this.thresholdWindowSize = thresholdWindowSize;
        this.beatInsensitivity = beatInsensitivity;

        // set starting index to process
        indexToProcess = thresholdWindowSize / 2;
    }

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
            // cull fluxes that do not exceed threshold set by neighbors
            fluxSamples[indexToProcess].threshold = GetFluxThreshold(indexToProcess);
            CullSpectralFlux(indexToProcess);

            // determine if flux signifies beat (i.e., peak among culled fluxes)

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
    /// <returns>threshold used to cull non-beats</returns>
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

    /// <summary>
    /// Culls fluxes that do not exceed threshold
    /// to be considered a beat
    /// </summary>
    /// <param name="spectralFluxIndex">index to analyze</param>
    /// <returns>culled flux, 0 if too insignificant</returns>
    void CullSpectralFlux(int spectralFluxIndex)
    {
        fluxSamples[spectralFluxIndex].culledSpectralFlux =
            Mathf.Max(0, fluxSamples[spectralFluxIndex].spectralFlux - fluxSamples[spectralFluxIndex].threshold);
    }

    /// <summary>
    /// Determines whether index of spectral flux should
    /// be considered a beat.
    /// </summary>
    /// <param name="spectralFluxIndex">index to analyze</param>
    /// <returns>true if index is a beat</returns>
    bool isBeat(int spectralFluxIndex)
    {
        // signify beat if culled flux at index is greater than neighbors
        if (fluxSamples[spectralFluxIndex].culledSpectralFlux > fluxSamples[spectralFluxIndex - 1].culledSpectralFlux &&
            fluxSamples[spectralFluxIndex].culledSpectralFlux > fluxSamples[spectralFluxIndex + 1].culledSpectralFlux)
            return true;

        // otherwise, no beat
        return false;
    }
}

/// <summary>
/// Simple container for spectral flux data
/// </summary>
public class SpectralFluxInfo
{
    public float time = 0f;                 // time in audio track info was gathered
    public float spectralFlux = 0f;         // aggregate of positive change in spectrum data between frames
    public float threshold = 0f;            // change threshold other fluxes must exceed to be considered an onset
    public float culledSpectralFlux = 0f;   // aggregate of positive changes that exceed beat threshold
    public bool isBeat = false;             // whether flux at this point in time is a beat in song
}
