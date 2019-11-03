﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a fractal tree using L-system and adjusted
/// by values of music analyzer.
/// </summary>
[RequireComponent(typeof(TrackAnalyzer))]
public class TreeGenerator : MonoBehaviour
{
    // branching configuration variables
    [Range(1, 10)]
    [SerializeField] int maxGenerations = 10;       // max number of generations fractal will undergo before stopping
    [SerializeField] GameObject branchPrefab;       // generic branch prefab to spawn and manipulate

    // generation support variables
    Transform startingTrunk;                        // transform of initial branch object to build tree from
    float growthRate = 1f;                          // rate at which branches grow before splitting -- entire tree should finish growing when song is over

    /// <summary>
    /// Read-access property returning number of generations
    /// fractal tree undergoes before breaking
    /// </summary>
    public int MaxGenerations
    {
        get { return maxGenerations; }
    }


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

        // create list of trunks to grow and append starting trunk
        List<Transform> toGrow = new List<Transform>();
        toGrow.Add(trunk);

        // initialize starting and target scale of trunk
        float branchGrowth = 0;
        Vector3 startingScale = new Vector3(1, 0, 1);
        Debug.Log("here!");
        Vector3 targetScale = Vector3.one;
        trunk.localScale = startingScale;

        // while generation has not exceeded limit
        do
        {
            // scale each growing branch over time
            foreach (Transform currTrunk in toGrow)
            {
                // scale branch up over time
                branchGrowth += Time.deltaTime * growthRate;
                currTrunk.localScale = Vector3.Lerp(startingScale, targetScale, Mathf.Max(0.05f, branchGrowth));
                yield return new WaitForEndOfFrame();
            }

            // if last branch in generation has finished growing and this isn't the last generation
            if (toGrow[toGrow.Count - 1].localScale.y >= targetScale.y && generation != maxGenerations)
            {
                // initialize list storing branches created by this generation
                List<Transform> newBranches = new List<Transform>();
                generation++;

                // branch from each grown trunk
                foreach (Transform currTrunk in toGrow)
                {
                    // create n branches from roughly top of trunk
                    // NOTE: count hard set to 4 for testing
                    int branchCount = 4;
                    for (int i = 0; i < branchCount; i++)
                    {
                        // create, rotate, and position new branch
                        Transform currBranch = Instantiate(branchPrefab, currTrunk).transform;
                        currBranch.Rotate(new Vector3(35f, i * 360f / branchCount), Space.Self);
                        currBranch.localPosition += Vector3.up * currTrunk.GetChild(0).localScale.y * 2f;

                        // store starting and target scales of next branch generation
                        targetScale = new Vector3(1f / currTrunk.localScale.x, 1f / currTrunk.localScale.y, 1f / currTrunk.localScale.z) * 0.7f;
                        startingScale = new Vector3(targetScale.x, 0, targetScale.z);
                        currBranch.localScale = startingScale;

                        // add to list of future trunks
                        newBranches.Add(currBranch);
                    }
                }

                // treat new branches as trunks and continue generating
                branchGrowth = 0;
                toGrow = new List<Transform>(newBranches);

                //Debug.Log(startingScale + " " + targetScale);
            }
        } while (generation <= maxGenerations);

        Debug.Log("Done!");
    }
}
