using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a fractal tree using L-system and adjusted
/// by values of music analyzer.
/// </summary>
public class TreeGenerator : MonoBehaviour
{
    // branching support variables
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
        GenerateTree(baseBranch.transform);
    }

    /// <summary>
    /// Recusively generates a fractal tree from 
    /// starting 'trunk'
    /// </summary>
    /// <param name="trunk">current 'trunk' of tree to 
    /// grow branches from</param>
    void GenerateTree(Transform trunk)
    {
        // create n branches from around top of trunk
        // NOTE: currently hard-set to 4 for easy testing
        for (int i = 0; i < 1; i++)
        {
            // create, position, rotate and scale branches of current trunk
            GameObject currBranch = Instantiate(baseBranch, transform);
            currBranch.transform.localPosition = 
                Quaternion.Euler(trunk.localEulerAngles.x, trunk.localEulerAngles.y, trunk.localEulerAngles.z) * new Vector3(0, trunk.localScale.y * 2, 0);
        }
    }
}
