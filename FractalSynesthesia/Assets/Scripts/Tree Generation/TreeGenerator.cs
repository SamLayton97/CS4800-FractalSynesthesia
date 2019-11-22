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

    // generation support variables
    [SerializeField] BranchGrower startingTrunk;    // growth controller of initial branch object to build tree from
    [SerializeField] GameObject branchPrefab;       // generic branch prefab to spawn and manipulate

    #region Properties

    /// <summary>
    /// Read-access property returning max number of
    /// generations fractal undergoes before stopping.
    /// </summary>
    public int TotalGenerations
    {
        get { return maxGenerations; }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Used for late initialization
    /// </summary>
    void Start()
    {
        // if not set in editor, retrieve initial trunk to grow from
        if (!startingTrunk)
            startingTrunk = transform.GetChild(0).GetComponent<BranchGrower>();

        // calculate rate of branch growth
        // NOTE: tree should finish growing approximately when song ends
        float growthRate = (TotalGenerations + 1) / TrackSelectionManager.Instance.CurrentTrack.length;

        // begin fractal tree by initializing trunk
        startingTrunk.Initialize(Vector3.one, growthRate, 0, TotalGenerations, branchPrefab);
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
