using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Samples structurally relevant data from
/// track analysis at fixed rate.
/// </summary>
[RequireComponent(typeof(TreeGenerator))]
public class MovementSampler : MonoBehaviour
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

    // sample average variables
    float avgDominantRange = 0f;
    float avgDeviationScale = 0f;
    float avgApproximateVolume = 0f;
    float avgMelodyVolume = 0f;
    float avgMelodicRange = 0f;

    // support variables
    static MovementSampler instance;    // pseudo-singleton
    TreeGenerator myGenerator;
    float sampleCounter = 0f;
    float sampleTime = 0f;

    #region Properties

    /// <summary>
    /// Read-access property returning instance
    /// of sampler from this scene.
    /// NOTE: Not a true singleton, avoid
    /// placing multiple in scene.
    /// </summary>
    public static MovementSampler Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// Read-access property returning average dominant
    /// range sampled during branch's growth
    /// </summary>
    public float AverageDominantRange
    {
        get { return avgDominantRange; }
    }

    /// <summary>
    /// Read-access property returning average standard
    /// deviation scale among frequency bands sampled
    /// during branch's growth
    /// </summary>
    public float AverageDeviationScale
    {
        get { return avgDeviationScale; }
    }

    /// <summary>
    /// Read-access property returning average approximate
    /// volume sampled during branch's growth
    /// </summary>
    public float AverageVolume
    {
        get { return avgApproximateVolume; }
    }

    /// <summary>
    /// Read-access property returning average volume
    /// of dominant frequency band (melody) sampled during
    /// branch's growth
    /// </summary>
    public float AverageMelodyVolume
    {
        get { return avgMelodyVolume; }
    }

    /// <summary>
    /// Read-access property returning average change in
    /// dominant frequency band (melodic range) sampled
    /// during branch's growth.
    /// </summary>
    public float AverageMelodicRange
    {
        get { return avgMelodicRange; }
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
        StartCoroutine(RefreshAverages(TrackSelectionManager.Instance.CurrentTrack.length / (myGenerator.TotalGenerations + 1)));
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
    /// Updates averages of structurally relevant data
    /// over current branch generation
    /// </summary>
    /// <param name="refreshTime">seconds coroutine waits before
    /// refreshing sample averages</param>
    /// <returns>coroutine controlling this operation</returns>
    IEnumerator RefreshAverages(float refreshTime)
    {
        // while the track is still playing
        do
        {
            // wait duration of branch generation
            yield return new WaitForSeconds(refreshTime);

            // update averages
            avgDominantRange = dominantRangeSamples.Average();
            avgDeviationScale = deviationScaleSamples.Average();
            avgApproximateVolume = approximateVolumeSamples.Average();
            avgMelodyVolume = melodyVolumeSamples.Average();
            avgMelodicRange = melodicRangeSamples.Average();

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
