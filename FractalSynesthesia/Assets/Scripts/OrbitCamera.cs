using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables user to rotate camera around an object
/// using their mouse
/// </summary>
public class OrbitCamera : MonoBehaviour
{
    // configuration variables
    [SerializeField] Transform targetTransform;         // transform of object to orbit about
    [SerializeField] Vector3 lookOffset;                // initial offset to rotate camera (in degrees)
    [SerializeField] float orbitSpeed = 5f;             // speed modifier to rotate camera at

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // set camera to start facing object plus offset
        transform.LookAt(targetTransform);
        transform.Rotate(lookOffset);
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // if user is holding mouse button down
        if (Input.GetMouseButton(0))
        {
            // temporarily hide mouse
            Cursor.visible = false;

            // rotate camera around object's y-axis using "Mouse X" input
            transform.RotateAround(targetTransform.position, Vector3.down, Input.GetAxis("Mouse X") * Time.deltaTime * orbitSpeed * -1);
        }
        // otherwise, show cursor again
        else
            Cursor.visible = true;
    }
}
