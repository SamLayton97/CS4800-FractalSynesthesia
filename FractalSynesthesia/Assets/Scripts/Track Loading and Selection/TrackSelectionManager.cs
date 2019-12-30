using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
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
    [SerializeField] Transform buttonHolder;            // transform of panel object holding all track button
    CanvasGroup myCanvasGroup;                          // selector's canvas group component -- used to control visibility

    // track setting support
    [SerializeField] AudioClip currentTrack;            // track to play for this instance of fractal -- defaults to track set in Editor
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
        get { return currentTrack; }
    }

    #region Unity Methods

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
        if (!buttonHolder)
            buttonHolder = transform.GetChild(0);   // assume content holder is immediate child if not set in editor

        // initialize selector to be invisible
        myCanvasGroup.alpha = 1;
        myCanvasGroup.blocksRaycasts = true;
        myCanvasGroup.interactable = true;

        // initialize default track from Resources
        if (!currentTrack)
            currentTrack = Resources.Load<AudioClip>("Mozart - Menuettos");    // if not set prior to launch, default clip
    }

    /// <summary>
    /// Used for late initialization
    /// </summary>
    void Start()
    {
        // NOTE: by now, all UI singletons should have initialized
        // initialize UI for default track
        InitializeTrack(currentTrack);

        // if proper filepath exists, load custom tracks from StreamingAssets
        if (System.IO.Directory.Exists(Application.streamingAssetsPath))
            StartCoroutine(LoadCustomTracks());
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

    #endregion

    #region Private Methods

    /// <summary>
    /// Loads track into dictionary and initializes
    /// UI that allows users to select it
    /// </summary>
    /// <param name="unloadedTrack">unloaded audio clip to initialize</param>
    void InitializeTrack(AudioClip unloadedTrack)
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

        // initialize auto-play capabilities for track
        AutoPlayManager.Instance.AddTrackName(unloadedTrack.name);

        // disable button of current track
        if (currentTrack.name == unloadedTrack.name)
            interact.interactable = false;
    }

    /// <summary>
    /// Loads all supported audio files from StreamingAssets,
    /// returning them as an list of playable audioclips
    /// </summary>
    /// <returns>array of specific audio types</returns>
    public IEnumerator LoadCustomTracks()
    {
        // load all supported audio files from streaming assets
        DirectoryInfo streamingAssets = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] audioFiles = streamingAssets.GetFiles("*.*");

        // iterate over and convert each file
        for (int i = 0; i < audioFiles.Length; i++)
        {
            // construct uri
            string filePath = audioFiles[i].FullName.ToString();
            string uri = string.Format("file://{0}", filePath);

            // determine audio type from file extension
            AudioType type = AudioType.WAV;
            switch (audioFiles[i].Extension)
            {
                // convert to one of supported audio types
                case ".wav":
                    break;
                case ".ogg":
                    type = AudioType.OGGVORBIS;
                    break;
                // if file isn't supported, skip it
                default:
                    continue;
            }

            // create web request to particular audio file
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, type))
            {
                yield return www.SendWebRequest();

                // catch and log errors
                if (www.isNetworkError)
                    Debug.LogError(www.error);
                // otherwise (no problems getting track)
                else
                {
                    // initialize custom track
                    AudioClip newTrack = DownloadHandlerAudioClip.GetContent(www);
                    newTrack.name = Path.GetFileNameWithoutExtension(audioFiles[i].Name);
                    InitializeTrack(newTrack);
                }
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles users selecting track from list of options
    /// </summary>
    /// <param name="newTrack">name of selected track</param>
    public void SelectTrack(string newTrack)
    {
        // re-enable button of old track
        trackSelectors[currentTrack.name].interactable = true;

        // select track to generate from and reload scene
        currentTrack = tracks[newTrack];
        trackSelectors[newTrack].interactable = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

}
