using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;


public class MusicController : MonoBehaviour
{
    public static MusicController Instance;
    public string MusicStateParameterName = "MusicState";
    public string DefaultMusicState;
    [SerializeField] private List<string> musicStates;
    [SerializeField] private string currentMusicState;
    [SerializeField] private StudioEventEmitter musicEmitter;
    [SerializeField] private FMODUnity.EventReference fmodEvent;
    [SerializeField] private FMOD.Studio.EventInstance musicEventInstance;
    [SerializeField] private FMOD.Studio.PARAMETER_DESCRIPTION paramDesc;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;
    }

    private void Start() {
        musicEmitter = Instance.gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
        musicEventInstance = musicEmitter.EventInstance;
        fmodEvent = musicEmitter.EventReference;

        GetMusicStates(musicEmitter);
        SetMusicState(SceneManager.GetActiveScene().name);
    }

    void GetMusicStates(StudioEventEmitter musicEmitter)
    {
        var desc = musicEmitter.EventDescription;

        var result = desc.getParameterDescriptionByName(MusicStateParameterName, out paramDesc);
        if (result != FMOD.RESULT.OK) return;

        string foundState;
        for (int i = 0; i <= paramDesc.maximum; i++)
        {
            result = musicEmitter.EventDescription.getParameterLabelByID(paramDesc.id, i, out foundState);

            if (result == FMOD.RESULT.OK)
            {
                musicStates.Add(foundState);
                if (DefaultMusicState == "" && i == paramDesc.defaultvalue) DefaultMusicState = foundState;
            }
        }
    }

    public void SetMusicState(string sceneName)
    {
        if (musicEventInstance.Equals(null) || musicStates.Count == 0) return;

        int idx = musicStates.FindIndex(a => a.Contains(sceneName));
        if (idx < 0) idx = musicStates.FindIndex(a => a.Contains(DefaultMusicState));
        currentMusicState = musicStates[idx];

        musicEventInstance.setParameterByIDWithLabel(paramDesc.id, currentMusicState);
    }
}
