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
    [Range(0, 20)]
    [SerializeField] float adjustRate = 1f;             // rate at which branch's hue and saturation shift to its target
    [Range(0f, 1f)]
    [SerializeField] float minValue = 0f;               // minimum value branch's color can be under no beat
    [Range(0f, 1f)]
    [SerializeField] float valueUpswing = 1f;           // rate at which color value increases when beat is heard
    [Range(-1f, 0f)]
    [SerializeField] float valueFalloff = -1f;          // rate at which color value decreases when no beat is heard

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
            currValue = Mathf.Clamp01(currValue + (TrackAnalyzer.Instance.Beat ? valueUpswing : valueFalloff));
            branchRenderer.material.color = Color.HSVToRGB(currHue, currSaturation, currValue);
        }
    }
}
