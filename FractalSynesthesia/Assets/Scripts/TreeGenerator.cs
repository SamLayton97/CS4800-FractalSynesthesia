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
        GenerateTree(startingTrunk, 0);
    }

    /// <summary>
    /// Recusively generates a fractal tree from 
    /// starting 'trunk'
    /// </summary>
    /// <param name="trunk">current 'trunk' of tree to 
    /// grow branches from</param>
    /// <param name="generation">current generation of fractal</param>
    void GenerateTree(Transform trunk, int generation)
    {
        // TODO: manipulate tree's color using music data

        // if fractal hasn't exceeded generation limit
        if (generation < maxGenerations)
        {
            // create n branches from roughly top of trunk
            // NOTE: count hard set to 4 for testing
            int branchCount = 4;
            for (int i = 0; i < branchCount; i++)
            {
                // create and scale, rotate, and position new branch
                Transform currBranch = Instantiate(branchPrefab, trunk).transform;
                currBranch.localScale = new Vector3(1f / trunk.localScale.x, 1f / trunk.localScale.y, 1f / trunk.localScale.z) * 0.8f;
                currBranch.Rotate(new Vector3(45f, i * 360f / branchCount), Space.Self);
                currBranch.localPosition += Vector3.up * trunk.GetChild(0).localScale.y * 2f;

                // treat new branch as a trunk and continue generation
                GenerateTree(currBranch, generation + 1);
            }
            
        }
    }
}
