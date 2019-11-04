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
    float shiftCounter = 0;

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
        // adjust material color over time
        shiftCounter += Time.deltaTime * adjustRate;
        branchRenderer.material.color = Color.Lerp(branchRenderer.material.color, targetColor, shiftCounter);
    }
}
