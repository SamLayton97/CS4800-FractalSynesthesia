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
    [Range(0, 120)]
    [SerializeField] float adjustRate = 1f;     // rate at which branch color shifts to its target

    // color setting support variables
    Color targetColor = Color.white;            // color branch shifts to over time
    Renderer branchRenderer;                    // reference to renderer of child branch primative -- used to control branch color
                                                // NOTE: assumes child objects has Material component attached to it
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
        // match color of branch with track analysis values
        branchRenderer.material.color = Color.Lerp(branchRenderer.material.color,
            Color.HSVToRGB(TrackAnalyzer.Instance.DominantRange,
            1 - TrackAnalyzer.Instance.BandDeviationScale,
            TrackAnalyzer.Instance.LeadVoiceDominance),
            Mathf.Min(Time.deltaTime * adjustRate, 1f));
    }
}
