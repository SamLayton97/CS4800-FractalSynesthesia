using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Loads user's custom tracks from StreamingAssets
/// </summary>
public class CustomTrackLoader : MonoBehaviour
{
    /// <summary>
    /// Loads all .wav files from StreamingAssets,
    /// returning them as an list of playable audioclips
    /// </summary>
    /// <returns>array of specific audio types</returns>
    public IEnumerator LoadTracks(List<AudioClip> customTracks)
    {
        // clear custom tracks cache
        customTracks.Clear();

        // load all .wav files from streaming assets
        DirectoryInfo streamingAssets = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] wavFiles = streamingAssets.GetFiles("*.wav");

        // iterate and convert each file
        for (int i = 0; i < wavFiles.Length; i ++)
        {
            // construct uri
            string filePath = wavFiles[i].FullName.ToString();
            string uri = string.Format("file://{0}", filePath);

            // find and convert audio clip using uri
            yield return StartCoroutine(GetAudioClip(uri, AudioType.WAV));
        }
    }

    /// <summary>
    /// Retrieves and converts file into an audio
    /// clip from StreamingAssets.
    /// </summary>
    /// <param name="uri">uniform resource indentifier of audio file</param>
    /// <param name="type">Unity-supported audio file type to convert as</param>
    /// <returns></returns>
    IEnumerator GetAudioClip(string uri, AudioType type)
    {
        // create web request to particular audio file
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, type))
        {
            yield return www.SendWebRequest();

            // catch and log errors
            if (www.isNetworkError)
                Debug.LogError(www.error);
            // otherwise (no problems getting track)
            else
            {
                // TODO: append track to list of custom songs
                AudioClip newTrack = DownloadHandlerAudioClip.GetContent(www);

            }
        }
    }
}
