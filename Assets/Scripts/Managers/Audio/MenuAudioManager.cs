using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public FMODUnity.EventReference cardShuffleSound;
    public FMODUnity.EventReference cardClickSound;

    private MenuAudioManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void OnCardShuffle()
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(cardShuffleSound);
        instance.start();
    }

    public void OnCardClick()
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(cardClickSound);
        instance.start();
    }
}