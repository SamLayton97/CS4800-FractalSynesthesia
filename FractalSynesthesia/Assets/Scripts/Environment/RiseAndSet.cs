using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls position of sun (i.e., rotation of
/// directional light) to rise and fall over duration
/// of song
/// </summary>
public class RiseAndSet : MonoBehaviour
{
    [SerializeField]
    Vector3 sunrise = new Vector2();        // euler angles of directional light at sunrise
    [SerializeField]
    Vector3 sunset = new Vector3();         // euler angles of directional light at sunset

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // adjust rotation of light to match progress through track
        transform.eulerAngles = Vector3.Lerp(sunrise, sunset, 1);
    }
}
