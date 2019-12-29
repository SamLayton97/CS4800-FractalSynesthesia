using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages automatic starting of next song in 
/// track list when current one ends
/// </summary>
public class AutoPlayManager : MonoBehaviour
{
    // auto-play support variables
    bool autoPlayIsOn = false;
    List<string> trackNames = new List<string>();   // list of default & custom tracks selectable by user -- used for playing next song in queue
    static AutoPlayManager instance;                // singleton instance

    /// <summary>
    /// Read-access property returning static instance
    /// of AutoPlayManager singleton
    /// </summary>
    public static AutoPlayManager Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // destroy this instance if duplicate of singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // not duplicate, set this instance as auto-play manager singleton
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Determines whether to play next song in track list
    /// </summary>
    public void HandleAutoPlay()
    {
        // if option is on, play next song
        if (autoPlayIsOn)
        {
            Debug.Log("play next");
        }
    }

    /// <summary>
    /// Adds new string to list of track names. 
    /// Used to easily find next track to play
    /// </summary>
    /// <param name="newName">name of track to add</param>
    public void AddTrackName(string newName)
    {
        trackNames.Add(newName);

        Debug.Log(newName);
    }

    /// <summary>
    /// Handles user toggling 'auto-play' option on and off
    /// </summary>
    /// <param name="optionOn">whether auto-play option should turn on</param>
    public void ToggleAutoPlay(bool optionOn)
    {
        autoPlayIsOn = optionOn;
    }
}
