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
    // wind change support variables
    TerrainData terrainData;

    /// <summary>
    /// Used for intialization
    /// </summary>
    void Awake()
    {
        // retrieve terrain data
        terrainData = GetComponent<Terrain>().terrainData;
    }
}
