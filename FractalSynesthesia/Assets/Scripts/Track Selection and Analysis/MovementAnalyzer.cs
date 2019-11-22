using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Samples structurally relevant data from
/// track analysis at fixed rate. Also calculates
/// heuristics representative of song over course
/// of current branch generation.
/// </summary>
[RequireComponent(typeof(TreeGenerator))]
public class MovementAnalyzer : MonoBehaviour
{
    // sample configuration variables
    [Range(1f, 10f)]
    [SerializeField] float sampleRate = 1f;         // rate (per second) which object samples data from track analysis

    // sampling lists
    List<float> dominantRangeSamples =              // list storing samples of dominant frequency band
        new List<float>();
    List<float> deviationScaleSamples =             // list storing samples of scale of std deviation among frequency bands
        new List<float>();
    List<float> approximateVolumeSamples =          // list storing samples of song's approximate volume
        new List<float>();
    List<float> melodyVolumeSamples =               // list storing samples of approximate volume of melody
        new List<float>();
    List<float> melodicRangeSamples =               // list storing samples of song's melodic range
        new List<float>();

    // structural heuristic variables
    float branchAngle = 0f;
    int branchCount = 0;
    float branchGirth = 0f;
    float branchLength = 0f;
    float branchNoise = 0f;

    // support variables
    static MovementAnalyzer instance;    // pseudo-singleton
    TreeGenerator myGenerator;
    float sampleCounter = 0f;
    float sampleTime = 0f;
    float updateTime = 0;

    #region Properties

    /// <summary>
    /// Read-access property returning instance
    /// of sampler from this scene.
    /// NOTE: Not a true singleton, avoid
    /// placing multiple in scene.
    /// </summary>
    public static MovementAnalyzer Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// Read-access property returning angle at
    /// which to grow branches on next generation
    /// </summary>
    public float BranchAngle
    {
        get { return branchAngle; }
    }

    /// <summary>
    /// Read-access property returning number of 
    /// branches to grow on next generation
    /// </summary>
    public int BranchCount
    {
        get { return branchCount; }
    }

    /// <summary>
    /// Read-access property returning xz-scale branches
    /// will grow to on next generation
    /// </summary>
    public float BranchGirth
    {
        get { return branchGirth; }
    }

    /// <summary>
    /// Read-access property returning y-scale branches
    /// grow to on next generation
    /// </summary>
    public float BranchLength
    {
        get { return branchLength; }
    }

    /// <summary>
    /// Read-access property returning randomness in
    /// angle and height of next-generation branches
    /// </summary>
    public float BranchNoise
    {
        get { return branchNoise; }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Used for early initialization
    /// </summary>
    void Awake()
    {
        // set pseudo-singleton to this object
        instance = this;
    }

    /// <summary>
    /// Used for late initialization
    /// </summary>
    void Start()
    {
        // calculate time between samples
        sampleTime = 1f / sampleRate;

        // start averages refresh coroutine
        myGenerator = GetComponent<TreeGenerator>();
        StartCoroutine(RefreshHeuristics(TrackSelectionManager.Instance.CurrentTrack.length / (myGenerator.TotalGenerations + 1)));
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // increment sample counter
        sampleCounter += Time.deltaTime;

        // if counter exceeds time
        if (sampleCounter >= sampleTime)
        {
            // sample various music data from track analyzer
            dominantRangeSamples.Add(TrackAnalyzer.Instance.DominantRange);
            deviationScaleSamples.Add(TrackAnalyzer.Instance.BandDeviationScale);
            approximateVolumeSamples.Add(TrackAnalyzer.Instance.ApproximateVolume);
            melodyVolumeSamples.Add(TrackAnalyzer.Instance.MelodicVolume);

            // sample melodic range values
            // NOTE: not performed in track analyzer as difference in dominant voice between each frame is negligible
            melodicRangeSamples.Add(Mathf.Abs(dominantRangeSamples[dominantRangeSamples.Count - 1] -
                dominantRangeSamples[Mathf.Max(0, dominantRangeSamples.Count - 2)]));

            // reset counter
            sampleCounter = 0;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates structural heuristic values, clearing
    /// lists for next generation
    /// </summary>
    /// <param name="refreshTime">seconds coroutine waits before
    /// refreshing sample averages</param>
    /// <returns>coroutine controlling this operation</returns>
    IEnumerator RefreshHeuristics(float refreshTime)
    {
        // while the track is still playing
        do
        {
            // wait duration of branch generation
            yield return new WaitForSeconds(refreshTime);

            // TODO: update heuristics
            

            // clear lists
            dominantRangeSamples.Clear();
            deviationScaleSamples.Clear();
            approximateVolumeSamples.Clear();
            melodyVolumeSamples.Clear();
            melodicRangeSamples.Clear();
            
        } while (TrackAnalyzer.Instance.TrackIsPlaying);
    }

    #endregion

}
