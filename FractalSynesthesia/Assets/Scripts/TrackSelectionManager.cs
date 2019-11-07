using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    Dictionary<string, AudioClip> tracks =              // dictionary pairing names of tracks with their corresponding audio clips
        new Dictionary<string, AudioClip>();
    Dictionary<string, Button> trackSelectors =         // dictionary pairing buttons with track they enable
        new Dictionary<string, Button>();

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
    /// Read-access property returning track
    /// currently selected by user
    /// </summary>
    public AudioClip CurrentTrack
    {
        get { return currTrack; }
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
            // load into dictionary
            tracks.Add(unloadedTrack.name, unloadedTrack);

            // create and initialize new button in holder
            GameObject newButton = Instantiate(selectorButton, buttonHolder);
            newButton.GetComponent<TrackSelector>().Initialize(unloadedTrack.name);

            // load button's button component into dictionary
            // NOTE: controls interactability on consecutive scene loads
            Button interact = newButton.GetComponent<Button>();
            trackSelectors.Add(unloadedTrack.name, interact);

            // disable button of current track
            if (currTrack.name == unloadedTrack.name)
                interact.interactable = false;
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
    /// <param name="newTrack">name of selected track</param>
    public void SelectTrack(string newTrack)
    {
        // re-enable button of old track
        trackSelectors[currTrack.name].interactable = true;

        // select track to generate from and reload scene
        currTrack = tracks[newTrack];
        trackSelectors[newTrack].interactable = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
