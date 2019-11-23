using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contols color adjustments to branch 
/// according to track analysis data
/// </summary>
public class ColorChanger : MonoBehaviour
{
    // configuration variables
    [Range(0, 120)]
    [SerializeField] float adjustRate = 1f;             // rate at which branch color shifts to its target

    // color setting support variables
    Renderer branchRenderer;                    // reference to renderer of child branch primative -- used to control branch color
                                                // NOTE: assumes child objects has Material component attached to it
    float currHue = 0f;                         // current hue of branch color
    float currSaturation = 0f;                  // curren saturation of branch color
    float currValue = 0f;                       // current value of branch color

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // retrieve reference to child's material component
        branchRenderer = GetComponentInChildren<Renderer>();
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // if track is playing
        if (TrackAnalyzer.Instance.TrackIsPlaying)
        {
            // match HSV-color of branch with track analysis values
            currHue = Mathf.Lerp(currHue, TrackAnalyzer.Instance.DominantRange, Mathf.Min(Time.deltaTime * adjustRate, 1));
            currSaturation = Mathf.Lerp(currSaturation, 1 - TrackAnalyzer.Instance.BandDeviationScale, Mathf.Min(Time.deltaTime * adjustRate, 1));
            currValue = Mathf.Clamp01(currValue + (TrackAnalyzer.Instance.Beat ? 0.5f : -0.02f));
            branchRenderer.material.color = Color.HSVToRGB(currHue, currSaturation, currValue);
        }
    }
}
