using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control scale, speed, tint of wind over
/// grass according to track analysis data.
/// </summary>
[RequireComponent(typeof(Terrain))]
public class WindChanger : MonoBehaviour
{
    // wind change configuration variables
    [Range(0f, 1f)]
    [SerializeField] float colorAdjustRate = 1f;    // rate at which waving grass ting shifts to its target

    // wind change support variables
    TerrainData terrainData;                // used to modify terrain
    float currHue = 0f;                     // current hue of waving grass tint
    float currSaturation = 0f;              // current saturation of waving grass tint

    /// <summary>
    /// Used for intialization
    /// </summary>
    void Awake()
    {
        // retrieve terrain data
        terrainData = GetComponent<Terrain>().terrainData;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // so long as track is playing
        if (TrackAnalyzer.Instance.TrackIsPlaying)
        {
            // match waving grass tint hue and saturation with track analysis values
            currHue = Mathf.Lerp(currHue, TrackAnalyzer.Instance.DominantRange, Mathf.Min(Time.deltaTime * colorAdjustRate, 1));
            currSaturation = Mathf.Lerp(currSaturation, 1 - TrackAnalyzer.Instance.BandDeviationScale, Mathf.Min(Time.deltaTime * colorAdjustRate, 1));
            terrainData.wavingGrassTint = Color.HSVToRGB(currHue, currSaturation, 0.85f);

            // TODO: match wind speed with song's beats per minute
            //terrainData.wavingGrassSpeed = Random.Range(0.5f, 0.51f);
        }
    }
}
