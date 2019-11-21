using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls growth of a particular branch
/// according to track analysis data
/// </summary>
public class GrowBranch : MonoBehaviour
{
    // growth support fields
    Vector3 startingScale = new Vector3(1f, 0f, 1f);
    Vector3 targetScale = Vector3.one;

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // TESTING: test initialization
        Initialize(Vector3.one * 10, 1f, 1);
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

        Debug.Log("Done!");
    }
}
