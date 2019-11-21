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

    // sampling lists
    List<float> donimantRangeSamples =              // list storing samples of dominant frequency band
        new List<float>();
    List<float> deviationScaleSamples =             // list storing samples of scale of std deviation among frequency bands
        new List<float>();
    List<float> approximateVolumeSampels =          // list storing samples of song's approximate volume
        new List<float>();
    List<float> melodyVolumeSamples =               // list storing samples of approximate volume of melody
        new List<float>();
    List<float> melodicRangeSamples =               // list storing samples of song's melodic range
        new List<float>();
}
