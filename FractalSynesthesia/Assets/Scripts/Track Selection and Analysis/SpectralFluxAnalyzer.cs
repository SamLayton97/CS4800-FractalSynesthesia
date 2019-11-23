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
    List<SpectralFluxInfo> fluxQueue =                      // collection of spectal flux samples used to compare beats with non-beats
        new List<SpectralFluxInfo>();                       // NOTE: treated as queue
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
    public bool AnalyzeSpectrum(float[] spectrum, float time)
    {
        // update previous spectrum data to find flux next frame
        currSpectrum.CopyTo(prevSpectrum, 0);
        spectrum.CopyTo(currSpectrum, 0);

        // push current spectral flux from spectrum onto queue
        SpectralFluxInfo newInfo = new SpectralFluxInfo();
        newInfo.spectralFlux = CalculateRectifiedSpectralFlux();
        fluxQueue.Add(newInfo);

        // if sample collection is large enough to analyze beats from
        if (fluxQueue.Count > thresholdWindowSize)
        {
            // pop first item in queue (old info)
            fluxQueue.RemoveAt(0);

            // cull fluxes that do not exceed threshold set by neighbors
            fluxQueue[indexToProcess].threshold = GetFluxThreshold(indexToProcess);
            CullSpectralFlux(indexToProcess);

            // return true if there is a beat now
            if (IsBeat(indexToProcess - 1))
                return true;
        }

        // return false if either not enough data or no beat
        return false;
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
            threshold += fluxQueue[i].spectralFlux;
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
        fluxQueue[spectralFluxIndex].culledSpectralFlux =
            Mathf.Max(0, fluxQueue[spectralFluxIndex].spectralFlux - fluxQueue[spectralFluxIndex].threshold);
    }

    /// <summary>
    /// Determines whether index of spectral flux should
    /// be considered a beat.
    /// </summary>
    /// <param name="spectralFluxIndex">index to analyze</param>
    /// <returns>true if index is a beat</returns>
    bool IsBeat(int spectralFluxIndex)
    {
        // signify beat if culled flux at index is greater than neighbors
        if (fluxQueue[spectralFluxIndex].culledSpectralFlux > fluxQueue[spectralFluxIndex - 1].culledSpectralFlux &&
            fluxQueue[spectralFluxIndex].culledSpectralFlux > fluxQueue[spectralFluxIndex + 1].culledSpectralFlux)
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
    public float spectralFlux = 0f;         // aggregate of positive change in spectrum data between frames
    public float threshold = 0f;            // change threshold other fluxes must exceed to be considered an onset
    public float culledSpectralFlux = 0f;   // aggregate of positive changes that exceed beat threshold
}
