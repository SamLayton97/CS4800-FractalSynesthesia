using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls position of sun (i.e., rotation of
/// directional light) to rise and fall over duration
/// of song
/// </summary>
[RequireComponent(typeof(Light))]
public class RiseAndSet : MonoBehaviour
{
    // positional configuration values
    [SerializeField]
    Vector3 sunrise = new Vector2();        // euler angles of directional light at sunrise
    [SerializeField]
    Vector3 sunset = new Vector3();         // euler angles of directional light at sunset

    // color configuration values
    [SerializeField] Color noonColor;       // color of directional light at noon
    [SerializeField] Color riseSetColor;    // color of directional light at sunrise/sunset

    // lighting support variables
    Light sunlight;

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        sunlight = GetComponent<Light>();
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // so long as track is playing
        if (TrackAnalyzer.Instance.TrackIsPlaying)
        {
            // adjust rotation and color of light to match progress through track
            transform.eulerAngles = Vector3.Slerp(sunrise, sunset,
                TrackAnalyzer.Instance.Progress);

        }
    }
}
