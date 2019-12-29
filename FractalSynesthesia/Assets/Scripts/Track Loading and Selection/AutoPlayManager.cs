using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages automatic starting of next song in 
/// track list when current one ends
/// </summary>
public class AutoPlayManager : MonoBehaviour
{
    public void ToggleAutoPlay(bool optionOn)
    {
        Debug.Log(optionOn);
    }
}
