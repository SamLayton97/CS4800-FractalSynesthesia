using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages automatic starting of another song in 
/// track list when current one ends
/// </summary>
public class AutoPlayManager : MonoBehaviour
{
    // auto-play support variables
    bool autoPlayIsOn = false;
    bool shuffleIsOn = false;
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

    #region Unity Methods

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

    #endregion

    #region Public Methods

    /// <summary>
    /// Determines whether to play next song in track list
    /// </summary>
    public void HandleAutoPlay()
    {
        // if option is on, play another song
        if (autoPlayIsOn)
        {
            // find and play next song in queue
            int songIndex = trackNames.IndexOf(TrackSelectionManager.Instance.CurrentTrack.name);
            songIndex = (songIndex + 1 < trackNames.Count) ? songIndex + 1 : 0;
            TrackSelectionManager.Instance.SelectTrack(trackNames[songIndex]);
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
    }

    #endregion

    #region Option Toggling Methods

    /// <summary>
    /// Handles user toggling 'auto-play' option on and off
    /// </summary>
    /// <param name="optionOn">whether auto-play option should turn on</param>
    public void ToggleAutoPlay(bool optionOn)
    {
        autoPlayIsOn = optionOn;
    }

    #endregion

}
