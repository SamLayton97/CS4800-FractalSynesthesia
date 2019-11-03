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
    [SerializeField] GameObject baseBranch;         // branch object spawned -- initialized to trunk placed in scene

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        // if not already set, retrieve starting branch of tree
        if (!baseBranch)
            baseBranch = transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Called once before first frame of Update()
    /// </summary>
    void Start()
    {
        // TODO: retrieve data from music analysis

        // generate fractal tree from trunk
        GenerateTree(baseBranch.transform, 1);
    }

    /// <summary>
    /// Recusively generates a fractal tree from 
    /// starting 'trunk'
    /// </summary>
    /// <param name="trunk">current 'trunk' of tree to 
    /// grow branches from</param>
    void GenerateTree(Transform trunk, int generation)
    {
        // create list of future trunks to branch from
        List<Transform> branchFrom = new List<Transform>();

        // create n branches from around top of trunk
        // NOTE: currently hard-set to 4 for easy testing
        int branchCount = 4;
        for (int i = 0; i < branchCount; i++)
        {
            // create, rotate, scale, and position branches of current trunk
            Transform currBranch = Instantiate(trunk, transform).transform;
            currBranch.Rotate(new Vector3(45, i * (360f / branchCount), 0));
            currBranch.localScale *= 0.8f;
            currBranch.localPosition = Quaternion.Euler(trunk.localEulerAngles.x / 2f, trunk.localEulerAngles.y / 2f, trunk.localEulerAngles.z / 2f) * 
                (trunk.localPosition + (Vector3.up * trunk.localScale.y * 0.95f)) + 
                Quaternion.Euler(currBranch.localEulerAngles.x, currBranch.localEulerAngles.y, currBranch.localEulerAngles.z) * (Vector3.up * currBranch.localScale.y);

            // add branch to future trunks to branch from
            branchFrom.Add(currBranch.transform);
        }

        // if tree hasn't reached max number of generations, continue with each new branch
        if (generation < maxGenerations)
        {
            foreach (Transform branch in branchFrom)
                GenerateTree(branch, generation + 1);
        }
    }
}
