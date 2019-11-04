using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contols color adjustments to branch 
/// according to track analysis data
/// </summary>
public class ChangeBranchColor : MonoBehaviour
{
    // configuration variables
    [Range(0, 1)]
    [SerializeField] float adjustRate = 1f;     // rate at which branch shifts from one color to the next
                                                // NOTE: 1 denotes near-instantaneous change in branch's color

    // color setting support variables
    Color targetColor = Color.black;            // color branch shifts to over time
    Renderer branchRenderer;                    // reference to renderer of child branch primative -- used to control branch color
                                                // NOTE: assumes child objects has Material component attached to it
    float shiftTargetCounter = 0;
    List<Color> colorSamples =                  // colors sampled from track over time -- used to determine next target color
        new List<Color>();

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
        // adjust material color to target over time
        shiftTargetCounter += Time.deltaTime * adjustRate;
        branchRenderer.material.color = Color.Lerp(branchRenderer.material.color, targetColor, shiftTargetCounter);

        // TODO: retrieve hsv-color sample from track analyzer
        Debug.Log(TrackAnalyzer.Instance.BandStandardDeviation + " " + TrackAnalyzer.Instance.DominantRange +
            " " + TrackAnalyzer.Instance.LeadVoiceDominance);

        // when color has fully shifted to target
        if (shiftTargetCounter >= 1)
        {
            // TODO: find new target color and reset counter
            shiftTargetCounter = 0;
        }
    }
}
