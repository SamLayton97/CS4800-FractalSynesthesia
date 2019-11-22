using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls growth of a particular branch
/// according to track analysis data
/// </summary>
[RequireComponent(typeof(ColorChanger))]
public class BranchGrower : MonoBehaviour
{
    // structure configuration
    [SerializeField]
    Vector2 branchAngleRange = new Vector2();               // range within which branches can grow at angle from (pre-randomization)
    [Range(10f, 50f)]
    [SerializeField] float angleNoiseScaler = 30f;          // max additional degrees branches can grow, caused by randomization in generation
    [Range(2, 10)]
    [SerializeField] int maxBranchCount = 5;                // max number of branches tree can grow per generation
    [Range(1, 5)]
    [SerializeField] int branchDeviationRange = 3;          // total number of branches pruned during generation -- affected by average deviation scale

    // growth support fields
    Vector3 startingScale = new Vector3(1f, 0f, 1f);        // scale branch starts at when instantiated
    Vector3 targetScale = Vector3.one;                      // scale branch grows to
                                                            // NOTE: reaching this scale starts next generation sprouting from this branch

    // generation support variables
    ColorChanger myColorChange;                 // controls momentary color changes of branch

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // retrieve relevant components
        myColorChange = GetComponent<ColorChanger>();
    }

    /// <summary>
    /// Initializes growth control variables.
    /// Called by parent branch on new generation
    /// or generator on start of scene.
    /// </summary>
    /// <param name="targetScale">length/girth branch grows to</param>
    /// <param name="growthRate">rate which branch grows to full size at</param>
    /// <param name="currGeneration">current generation of fractal</param>
    /// <param name="maxGenerations">max number of generations fractal will undergo before stopping</param>
    /// <param name="branchPrefab">generic branch prefab to manipulate</param>
    public void Initialize(Vector3 targetScale, float growthRate, int currGeneration, int maxGenerations, GameObject branchPrefab)
    {
        // initialize starting and target scales
        this.targetScale = targetScale;
        startingScale = new Vector3(targetScale.x, 0.05f, targetScale.z);

        // start growing branch
        StartCoroutine(Grow(growthRate, currGeneration, maxGenerations, branchPrefab));
    }

    /// <summary>
    /// Scales branch up to its full length and girth
    /// </summary>
    /// <param name="growthRate">Rate at which branch grows.
    /// Note: Scaled such that tree finishes growing when
    /// track ends.</param>
    /// <param name="currGeneration">current generation of fractal</param>
    /// <param name="maxGenerations">max number of generations fractal will undergo before stopping</param>
    /// <param name="branchPrefab">generic prefab to create and manipulate on branching</param>
    IEnumerator Grow(float growthRate, int currGeneration, int maxGenerations, GameObject branchPrefab)
    {
        // while branch hasn't finished growing
        float growProgress = 0f;
        do
        {
            // scale up to target over time
            growProgress += Time.deltaTime * growthRate;
            transform.localScale = Vector3.Lerp(startingScale, targetScale, growProgress);
            yield return new WaitForEndOfFrame();

        } while (transform.localScale.y < targetScale.y);

        // deactivate color changing
        myColorChange.enabled = false;

        // continue fractal if current generation doesn't exceed max
        if (currGeneration < maxGenerations)
        {
            // calculate structure-defining heuristics
            float branchAngle = branchAngleRange.x + (1 - MovementSampler.Instance.AverageDominantRange) * branchAngleRange.y;
            int branchCount = maxBranchCount - Mathf.FloorToInt(Random.Range(0, branchDeviationRange * MovementSampler.Instance.AverageDeviationScale));
            float branchGirth = Mathf.Sqrt(Mathf.Sqrt(MovementSampler.Instance.AverageVolume));
            float branchLength = Mathf.Sqrt(Mathf.Sqrt(MovementSampler.Instance.AverageMelodyVolume));
            float branchNoise = Mathf.Sqrt(MovementSampler.Instance.AverageMelodicRange);

            // for as many branches this branch should grow
            for (int i = 0; i < branchCount; i++)
            {
                // create, rotate, position, and scale new branch
                Transform newBranch = Instantiate(branchPrefab, transform).transform;
                newBranch.Rotate(new Vector3(branchAngle + Random.Range(-1f * branchNoise, branchNoise) * angleNoiseScaler,
                    i * 360f / branchCount), Space.Self);               // angle randomized by melodic range
                newBranch.localPosition += Vector3.up * transform.GetChild(0).localScale.y *
                    (2f - Random.Range(0, branchNoise));                // height randomized by melodic range

                // start growth of new branch, with calculated target scale
                newBranch.GetComponent<BranchGrower>().Initialize(new Vector3(branchGirth, branchLength, branchGirth), 
                    growthRate, currGeneration + 1, maxGenerations, branchPrefab);
            }
        }
    }
}
