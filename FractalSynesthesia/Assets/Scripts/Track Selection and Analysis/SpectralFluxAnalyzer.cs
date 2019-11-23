using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Analyzes spectral flux of the current moment
/// of a audio clip. Used for algorithmic beat
/// mapping.
/// Based on following source: 
/// https://medium.com/giant-scam/algorithmic-beat-mapping-in-unity-real-time-audio-analysis-using-the-unity-api-6e9595823ce4
/// </summary>
public class SpectralFluxAnalyzer
{
    // analysis support variables
    float[] prevSpectrum = new float[1024];     // FFT spectrum of audio clip on previous frame
    float[] currSpectrum = new float[1024];     // FFT spectrum of audio clip on current frame

    /// <summary>
    /// Analyzes spectral flux of FFT spectrum data
    /// of an audio clip.
    /// </summary>
    /// <param name="spectrum">FFT spectrum data</param>
    /// <param name="time">temporal position in song (seconds)</param>
    public void AnalyzeSpectrum (float[] spectrum, float time)
    {
        // update previous spectrum data to find flux next frame
        currSpectrum.CopyTo(prevSpectrum, 0);
        spectrum.CopyTo(currSpectrum, 0);

        // get current spectral flux from spectrum
        Debug.Log(CalculateSpectralFlux());
    }

    /// <summary>
    /// Finds aggregate positive difference between
    /// current and previous spectrum data.
    /// </summary>
    /// <returns>aggregate positive difference</returns>
    float CalculateSpectralFlux()
    {
        // calculate aggregate positive difference in data
        float aggregate = 0;
        for (int i = 0; i < 1024; i++)
            aggregate += Mathf.Max(0, currSpectrum[i] - prevSpectrum[i]);

        // return sum of positive changes
        return aggregate;
    }
}
