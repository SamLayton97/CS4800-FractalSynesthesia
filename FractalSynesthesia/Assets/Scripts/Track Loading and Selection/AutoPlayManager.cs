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
    static AutoPlayManager instance;    // singleton instance

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
    /// Handles user toggling 'auto-play' option on and off
    /// </summary>
    /// <param name="optionOn">whether auto-play option should turn on</param>
    public void ToggleAutoPlay(bool optionOn)
    {
        autoPlayIsOn = optionOn;
    }
}
