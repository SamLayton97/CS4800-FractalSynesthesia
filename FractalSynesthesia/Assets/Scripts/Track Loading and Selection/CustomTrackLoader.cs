using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Loads user's custom tracks from StreamingAssets
/// </summary>
public class CustomTrackLoader
{
    /// <summary>
    /// Loads all PCM WAV files from StreamingAssets,
    /// returning them as an array of playable audioclips
    /// </summary>
    /// <returns></returns>
    public AudioClip[] LoadTracks()
    {
        // load all .wav files from streaming assets
        DirectoryInfo streamingAssets = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] wavFiles = streamingAssets.GetFiles("*.mp3");
        Debug.Log(wavFiles.Length);

        return new AudioClip[0];
    }
}
