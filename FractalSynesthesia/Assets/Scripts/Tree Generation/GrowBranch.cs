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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Grow(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
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
