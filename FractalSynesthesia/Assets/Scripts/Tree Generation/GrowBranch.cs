using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls growth of a particular branch
/// according to track analysis data
/// </summary>
public class GrowBranch : MonoBehaviour
{
    // configuration fields

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Grow()
    {
        yield return new WaitForEndOfFrame();
    }
}
