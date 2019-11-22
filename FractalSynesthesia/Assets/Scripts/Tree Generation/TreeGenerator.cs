using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Generates a fractal tree using L-system and adjusted
/// by values of music analyzer.
/// </summary>
public class TreeGenerator : MonoBehaviour
{
    // branching configuration
    [Range(1, 10)]
    [SerializeField] int maxGenerations = 10;       // max number of generations fractal will undergo before stopping
    [SerializeField] GameObject branchPrefab;       // generic branch prefab to spawn and manipulate
    [SerializeField] Vector2 branchAngleRange =     // range within which branches can grow at angle from (pre-randomization)
        new Vector2();
    [Range(10f, 50f)]
    [SerializeField] float angleNoiseScaler = 30f;  // max additional degrees branches can grow, caused by randomization in generation
    [Range(2, 10)]
    [SerializeField] int maxBranchCount = 5;        // max number of branches tree can grow per generation
    [Range(1, 5)]
    [SerializeField] int branchDeviationRange = 3;  // total number of branches pruned during generation -- affected by average deviation scale

    // generation support variables
    IEnumerator growTree;                           // coroutine controlling growth of fractal tree over course of track
    Transform startingTrunk;                        // transform of initial branch object to build tree from
    float growthRate = 1f;                          // rate at which branches grow before splitting -- entire tree finishes growing when song is over

    // music data sampling variables
    [SerializeField] float sampleRate = 1f;         // rate (per second) which generator samples values from track analyzer
    float sampleTime = 0;
    float sampleTimeCounter = 0;
    List<float> dominantRangeSamples =              // list storing samples of dominant frequency band
        new List<float>();
    List<float> deviationScaleSamples =            // list storing samples of deviation scales among frequency bands
        new List<float>();
    List<float> approximateVolumeSamples =          // list storing samples of song's approximate volume
        new List<float>();
    List<float> melodyVolumeSamples =               // list storing samples of approximate volume of melody
        new List<float>();
    List<float> melodicRangeSamples =               // list storing samples of song's melodic range
        new List<float>();

    // structure support variables
    float branchAngle = 45f;                        // angle at which to grow branches -- adjusted by average dominant range
    int branchCount = 5;                            // number of branches created on generation -- adjusted by average deviation scale
    float branchGirth = 0.7f;                       // target girth of branches created on generation -- adjusted by approximate volume
    float branchLength = 0.7f;                      // target length of branches created on generation -- adjusted by approximate lead melody volume
    float branchNoise = 0f;                         // variation in height and angle of each branch per trunk -- adjusted by melodic range

    #region Properties

    /// <summary>
    /// Read-access property returning max number
    /// of generations tree will undergo.
    /// With length of track, controls speed of
    /// branch growth.
    /// </summary>
    public int MaxGenerations
    {
        get { return maxGenerations; }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // retrieve references to trunk to grow tree from
        startingTrunk = transform.GetChild(0);

        // calculate time between samples
        sampleTime = 1f / sampleRate;
    }

    /// <summary>
    /// Called once before first frame of Update()
    /// </summary>
    void Start()
    {
        // set trees growth rate
        // NOTE: tree should finish growing approximately when song ends
        growthRate = (maxGenerations + 1) / TrackSelectionManager.Instance.CurrentTrack.length;

        // generate tree from starting branch
        //growTree = GenerateTree(startingTrunk, maxGenerations);
        //StartCoroutine(growTree);
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Recursive coroutine which generates a fractal tree
    /// over several frames until break case is reached
    /// </summary>
    /// <param name="trunk">'trunk' of tree to branch from</param>
    /// <param name="generation">number of generations before method stops</param>
    /// <returns></returns>
    IEnumerator GenerateTree(Transform trunk, int maxGenerations)
    {
        yield return new WaitForEndOfFrame();

        //// initialize a generation counter
        //int generation = 0;

        //// create list of trunks to grow and append starting trunk
        //List<Transform> toGrow = new List<Transform>();
        //toGrow.Add(trunk);

        //// growth of starting trunk
        //float branchGrowth = 0;
        //Vector3 targetScale = trunk.localScale;
        //Vector3 startingScale = new Vector3(targetScale.x, 0.05f, targetScale.z);
        //trunk.localScale = startingScale;

        //// continue to generate while fractal tree hasn't reached max generations
        //do
        //{
        //    // scale each growing branch over time
        //    foreach (Transform currTrunk in toGrow)
        //    {
        //        // scale branch up over time
        //        branchGrowth += Time.deltaTime * growthRate;
        //        currTrunk.localScale = Vector3.Lerp(startingScale, targetScale, branchGrowth);
        //        yield return new WaitForEndOfFrame();

        //        // if song sends before tree finishes generating, stop generating
        //        // NOTE: Solution is quick fix for times where fractal cannot keep
        //        // up with song. Next major update will push scaling control to
        //        // branches themselves rather than in this controller.
        //        if (!TrackAnalyzer.Instance.TrackIsPlaying)
        //            StopCoroutine(growTree);
        //    }

        //    // if last branch in generation has finished growing, branch
        //    if (toGrow[toGrow.Count - 1].localScale.y >= targetScale.y)
        //    {
        //        // calculate structure defining variables using music data
        //        branchAngle = branchAngleRange.x + (1 - dominantRangeSamples.Average()) * branchAngleRange.y;
        //        float averageDeviation = deviationScaleSamples.Average();
        //        branchCount = maxBranchCount - Mathf.FloorToInt(Random.Range(0, branchDeviationRange * averageDeviation));
        //        branchGirth = Mathf.Sqrt(Mathf.Sqrt(approximateVolumeSamples.Average()));
        //        branchLength = Mathf.Sqrt(Mathf.Sqrt(melodyVolumeSamples.Average()));
        //        branchNoise = Mathf.Sqrt(melodicRangeSamples.Average());

        //        // initialize list storing branches to be created by this generation
        //        List<Transform> newBranches = new List<Transform>();

        //        // branch from each grown trunk
        //        foreach (Transform currTrunk in toGrow)
        //        {
        //            // max trunk growth and lock color
        //            currTrunk.localScale = targetScale;
        //            currTrunk.GetComponent<ColorChanger>().enabled = false;

        //            // skip branching if this is last generation
        //            if (generation == maxGenerations)
        //                continue;

        //            // create n branches from roughly top of trunk
        //            for (int i = 0; i < branchCount; i++)
        //            {
        //                // create, rotate, position, and scale new branch
        //                Transform currBranch = Instantiate(branchPrefab, currTrunk).transform;
        //                currBranch.Rotate(new Vector3(branchAngle + Random.Range(-1f * branchNoise, branchNoise) * angleNoiseScaler, 
        //                    i * 360f / branchCount), Space.Self);               // angle randomized by melodic range
        //                currBranch.localPosition += Vector3.up * currTrunk.GetChild(0).localScale.y * 
        //                    (2f - Random.Range(0, branchNoise));                // height randomized by melodic range
        //                currBranch.localScale = startingScale;

        //                // add to list of future trunks
        //                newBranches.Add(currBranch);
        //            }
        //        }

        //        // set target scale of next branch generation
        //        targetScale = new Vector3(branchGirth, branchLength, branchGirth);

        //        // reset music sample lists for next generation
        //        dominantRangeSamples.Clear();
        //        deviationScaleSamples.Clear();
        //        approximateVolumeSamples.Clear();
        //        melodyVolumeSamples.Clear();
        //        melodicRangeSamples.Clear();

        //        // treat new branches as trunks and generating again
        //        branchGrowth = 0;
        //        generation++;
        //        toGrow = new List<Transform>(newBranches);
        //    }
        //} while (generation <= maxGenerations);
    }

    #endregion

}
