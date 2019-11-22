using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls growth of a particular branch
/// according to track analysis data
/// </summary>
[RequireComponent(typeof(ColorChanger))]
public class BranchGrower : MonoBehaviour
{
    // growth support fields
    Vector3 startingScale = new Vector3(1f, 0f, 1f);        // scale branch starts at when instantiated
    Vector3 targetScale = Vector3.one;                      // scale branch grows to
                                                            // NOTE: reaching this scale starts next generation sprouting from this branch

    // generation support variables
    ColorChanger myColorChange;                 // controls momentary color changes of branch

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // retrieve relevant components
        myColorChange = GetComponent<ColorChanger>();

        // TESTING: test initialization
        Initialize(Vector3.one * 2f, 0.1f, 1);
    }

    /// <summary>
    /// Initializes growth control variables.
    /// Called by parent branch on new generation
    /// or generator on start of scene.
    /// </summary>
    /// <param name="targetScale">length/girth branch grows to</param>
    /// <param name="growthRate">rate which branch grows to full size at</param>
    /// <param name="currGeneration">current generation of fractal</param>
    public void Initialize(Vector3 targetScale, float growthRate, int currGeneration)
    {
        // initialize starting and target scales
        this.targetScale = targetScale;
        startingScale = new Vector3(targetScale.x, 0.05f, targetScale.z);

        // start growing branch
        StartCoroutine(Grow(growthRate));
    }

    /// <summary>
    /// Scales branch up to its full length and girth
    /// </summary>
    /// <param name="growthRate">Rate at which branch grows.
    /// Note: Scaled such that tree finishes growing when
    /// track ends.</param>
    /// <returns></returns>
    IEnumerator Grow(float growthRate)
    {
        // while branch hasn't finished growing
        float growProgress = 0f;
        do
        {
            // scale up to target over time
            growProgress += Time.deltaTime * growthRate;
            transform.localScale = Vector3.Lerp(startingScale, targetScale, growProgress);
            yield return new WaitForEndOfFrame();

        } while (transform.localScale.y < targetScale.y);

        // TODO: branch once grown

        // deactivate color changing and sampling
        myColorChange.enabled = false;

        Debug.Log("Done!");
    }
}
