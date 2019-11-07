using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles closing application on user input
/// </summary>
public class ApplicationCloser : MonoBehaviour
{
    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // on user input, close application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Program ran for " + Time.time + " seconds.");
        }
    }
}
