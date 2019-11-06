using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Initializes button to set appropriate track on input
/// </summary>
[RequireComponent(typeof(Button))]
public class TrackSelector : MonoBehaviour
{
    // display support variables
    Text buttonText;

    /// <summary>
    /// Used for initalization
    /// </summary>
    void Awake()
    {
        // retrieve text component from child if not already found
        if (!buttonText)
            buttonText = GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Initialize button to display and select correct
    /// track on user input
    /// </summary>
    /// <param name="trackName">name of track</param>
    public void Initialize(string trackName)
    {
        // if not already retrieved, get text component from child
        if (!buttonText)
            buttonText = GetComponentInChildren<Text>();

        // change displayed text
        buttonText.text = trackName;
    }

    /// <summary>
    /// Changes track when user clicks button
    /// </summary>
    public void ChangeTrackOnClick()
    {
        // select track matching button's text as they are identical
        TrackSelectionManager.Instance.SelectTrack(buttonText.text);
    }
}
