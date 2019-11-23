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
    /// <summary>
    /// Analyzes spectral flux of FFT spectrum data
    /// of an audio clip.
    /// </summary>
    /// <param name="spectrum">FFT spectrum data</param>
    /// <param name="time">temporal position in song (seconds)</param>
    public void AnalyzeSpectrum (float[] spectrum, float time)
    {

    }
}
