using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Loads user's custom tracks from StreamingAssets
/// </summary>
public class CustomTrackLoader
{
    // custom audio support variables
    List<AudioClip> customTracks =              // list of tracks loaded from StreamingAssets
        new List<AudioClip>();

    /// <summary>
    /// Loads all PCM WAV files from StreamingAssets,
    /// returning them as an array of playable audioclips
    /// </summary>
    /// <returns></returns>
    public AudioClip[] LoadTracks()
    {
        // get path to streaming assets directory
        DirectoryInfo streamingAssets = new DirectoryInfo(Application.streamingAssetsPath);
        Debug.Log(streamingAssets);

        return new AudioClip[0];
    }
}
