using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a fractal tree using L-system and adjusted
/// by values of music analyzer.
/// </summary>
public class TreeGenerator : MonoBehaviour
{
    // branching configuration variables
    [Range(1, 50)]
    [SerializeField] int maxGenerations = 10;       // max number of generations fractal will undergo before stopping
    [SerializeField] GameObject branchPrefab;       // generic branch prefab to spawn and manipulate

    // generation support variables
    Transform startingTrunk;                        // transform of initial branch object to build tree from

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // retrieve starting branch of tree
        startingTrunk = transform.GetChild(0);
    }

    /// <summary>
    /// Called once before first frame of Update()
    /// </summary>
    void Start()
    {
        // TODO: retrieve data from music analysis

        // generate tree from starting branch
        StartCoroutine(GenerateTree(startingTrunk, maxGenerations));
    }

    /// <summary>
    /// Recursive coroutine which generates a fractal tree
    /// over several frames until break case is reached
    /// </summary>
    /// <param name="trunk">'trunk' of tree to branch from</param>
    /// <param name="generation">number of generations before method stops</param>
    /// <returns></returns>
    IEnumerator GenerateTree(Transform trunk, int maxGenerations)
    {
        // initialize a generation counter
        int generation = 1;

        // create list of trunks to branch from and append starting trunk
        List<Transform> branchFrom = new List<Transform>();
        branchFrom.Add(trunk);

        // while generation has not exceeded limit
        while (generation <= maxGenerations)
        {
            Debug.Log(generation);

            // initialize list storing branches created from this generation
            List<Transform> newBranches = new List<Transform>();

            // for each unbranched trunk
            Debug.Log(branchFrom.Count);
            foreach (Transform currTrunk in branchFrom)
            {
                // create n branches from roughly top of trunk
                // NOTE: count hard set to 4 for testing
                int branchCount = 4;
                for (int i = 0; i < branchCount; i++)
                {
                    // create, scale, rotate, and position new branch
                    Transform currBranch = Instantiate(branchPrefab, trunk).transform;
                    currBranch.localScale = new Vector3(1f / trunk.localScale.x, 1f / trunk.localScale.y, 1f / trunk.localScale.z) * 0.8f;
                    currBranch.Rotate(new Vector3(45f, i * 360f / branchCount), Space.Self);
                    currBranch.localPosition += Vector3.up * trunk.GetChild(0).localScale.y * 2f;

                    // add to list of future trunks
                    newBranches.Add(currBranch);
                }
            }

            // treat each new branch as a trunk for next generation
            branchFrom = new List<Transform>(newBranches);
            generation++;
            yield return new WaitForEndOfFrame();
        }
    }
}
