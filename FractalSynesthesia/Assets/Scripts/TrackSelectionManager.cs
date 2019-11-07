using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds and controls track selection UI, allowing
/// user to pick from any track within the Resources folder
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class TrackSelectionManager : MonoBehaviour
{
    // display support variables
    [SerializeField] GameObject selectorButton;         // button prefab used to select particular track
    Transform buttonHolder;                             // transform of panel object holding all track button
    CanvasGroup myCanvasGroup;                          // selector's canvas group component -- used to control visibility

    // track setting support
    [SerializeField] AudioClip currTrack;               // track to play for this instance of fractal -- defaults to track set in Editor

    // singleton support
    static TrackSelectionManager instance;              // instance of track selection singleton

    /// <summary>
    /// Read-access property returning instance of
    /// track selection instance
    /// </summary>
    public static TrackSelectionManager Instance
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

        // retrieve relevant components
        myCanvasGroup = GetComponent<CanvasGroup>();
        buttonHolder = transform.GetChild(0);

        // initialize selector to be invisible
        myCanvasGroup.alpha = 0;
        myCanvasGroup.blocksRaycasts = false;
        myCanvasGroup.interactable = false;

        // for each track in Resources
        foreach (AudioClip unloadedTrack in Resources.LoadAll("", typeof(AudioClip)))
        {
            // create and initialize new button in holder
            GameObject newButton = Instantiate(selectorButton, buttonHolder);
            newButton.GetComponent<TrackSelector>().Initialize(unloadedTrack.name);

            // disable button corresponding to current track
            if (currTrack.name == unloadedTrack.name)
                newButton.GetComponent<Button>().interactable = false;
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

    /// <summary>
    /// Handles users selecting track from list of options
    /// </summary>
    /// <param name="trackName">name of selected track</param>
    public void SelectTrack(string trackName)
    {
        Debug.Log(trackName);
    }
}
