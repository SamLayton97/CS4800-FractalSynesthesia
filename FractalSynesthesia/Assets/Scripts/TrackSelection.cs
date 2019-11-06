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

    // singleton support
    static TrackSelection instance;                     // instance of track selection singleton

    /// <summary>
    /// Read-access property returning instance of
    /// track selection instance
    /// </summary>
    public static TrackSelection Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // destroy this instance if duplicate of singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // set this instance as singleton if not duplicate
        instance = this;
        DontDestroyOnLoad(gameObject);

        // retrieve and initialize canvas group to invisible
        myCanvasGroup = GetComponent<CanvasGroup>();
        myCanvasGroup.alpha = 0;
        myCanvasGroup.blocksRaycasts = false;
        myCanvasGroup.interactable = false;

        // for each track in Resources
        foreach (AudioClip unloadedTrack in Resources.LoadAll("", typeof(AudioClip)))
        {
            Debug.Log(unloadedTrack.name);
        }
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
