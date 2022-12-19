using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;


public class MusicController : MonoBehaviour
{
    public string MusicStateParameterName = "MusicState";
    public FMODUnity.EventReference fmodEvent;
    string sceneName;
    private FMOD.Studio.EventInstance musicEventInstance;
    private FMOD.Studio.PARAMETER_DESCRIPTION paramDesc;
    private static MusicController Instance;
    public string DefaultMusicState;
    [SerializeField]
    private List<string> musicStates;
    [SerializeField]
    private string currentMusicState;
    // Music States

    Scene m_Scene;

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

    void Start()
    {
        var musicEmitter = Instance.GetComponent<FMODUnity.StudioEventEmitter>();
        musicEventInstance = musicEmitter.EventInstance;
        fmodEvent = musicEmitter.EventReference;

        GetMusicStates(musicEmitter);
        SetMusicState();
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

    // Update is called once per frame
    void Update()
    {
        SetMusicState();
    }

    public void SetMusicState()
    {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        if (musicEventInstance.Equals(null) || musicStates.Count == 0) return;

        int idx = musicStates.FindIndex(a => a.Contains(sceneName));
        if (idx < 0) idx = musicStates.FindIndex(a => a.Contains(DefaultMusicState));
        currentMusicState = musicStates[idx];

        musicEventInstance.setParameterByIDWithLabel(paramDesc.id, currentMusicState);
    }
}
