using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds and controls track selection UI, allowing
/// user to pick from any track within the Resources folder
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class TrackSelection : MonoBehaviour
{
    // display support variables
    [SerializeField] GameObject selectorButton;         // button prefab used to select particular track
    CanvasGroup myCanvasGroup;                          // selector's canvas group component -- used to control visibility

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // retrieve and initialize canvas group to invisible
        myCanvasGroup = GetComponent<CanvasGroup>();
        myCanvasGroup.alpha = 0;
        myCanvasGroup.blocksRaycasts = false;
        myCanvasGroup.interactable = false;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // on user input, toggle visibility of selector canvas
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            myCanvasGroup.alpha = myCanvasGroup.alpha < 1 ? 1 : 0;
            myCanvasGroup.blocksRaycasts = myCanvasGroup.blocksRaycasts ? false : true;
            myCanvasGroup.interactable = myCanvasGroup.interactable ? false : true;
        }
    }
}
