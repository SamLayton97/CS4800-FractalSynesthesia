using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class simulating a coroutine that returns
/// data once finished
/// Credit to Ted Bigham
/// https://answers.unity.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
/// </summary>
public class CoroutineWithData
{
    // return value support variables
    object data;
    IEnumerator target;

    /// <summary>
    /// Property with public read-access and private
    /// write-access returning coroutine to perform
    /// </summary>
    public Coroutine Action
    {
        get;
        private set;
    }

    /// <summary>
    /// Constructor for a coroutine that returns data
    /// </summary>
    /// <param name="owner">MonoBehavior object that initates coroutine</param>
    /// <param name="target">coroutine action to perform</param>
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        // set target and run coroutine
        this.target = target;
        this.Action = owner.StartCoroutine(target);
    }

    /// <summary>
    /// Runs coroutine, returning result once finished
    /// </summary>
    /// <returns></returns>
    IEnumerator Run()
    {
        // once coroutine has ended, return data
        while (target.MoveNext())
        {
            data = target.Current;
            yield return data;
        }
    }
}
