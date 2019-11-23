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

}
