using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Samples structurally relevant data from
/// track analysis at fixed rate.
/// </summary>
public class DataSampler : MonoBehaviour
{
    // sample configuration variables
    [Range(1f, 10f)]
    [SerializeField] float sampleRate = 1f;         // rate (per second) which object samples data from track analysis

    void Update()
    {
        Debug.Log(Time.time);
    }

}
