using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuAudioManager : MonoBehaviour
{
    public FMODUnity.EventReference cardShuffleSound;
    public FMODUnity.EventReference cardClickSound;

    private MenuAudioManager Instance;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindObjects();
    }

    private void FindObjects()
    {
        var onPointerEnter = new EventTrigger.Entry();
        onPointerEnter.eventID = EventTriggerType.PointerEnter;
        onPointerEnter.callback.AddListener( (eventData) => { OnCardShuffle(); } );

        var onPointerClick = new EventTrigger.Entry();
        onPointerClick.eventID = EventTriggerType.PointerClick;
        onPointerEnter.callback.AddListener( (eventData) => { OnCardClick(); } );

        var canvas = GameObject.Find("Canvas");
        var menuCards = canvas.transform.Find("Menu Cards")?.gameObject;
        var cardTriggers = menuCards?.GetComponentsInChildren<EventTrigger>();

        if (cardTriggers != null) {
            foreach (EventTrigger trigger in cardTriggers) {
                trigger.triggers.Add(onPointerEnter);
                trigger.triggers.Add(onPointerClick);
            }
        }

        var audioOptions = canvas.transform.Find("AudioOptions")?.gameObject;
        var audioSliders = audioOptions?.GetComponentsInChildren<Slider>();

        if (audioSliders != null) {
            foreach (Slider slider in audioSliders) {

                switch (slider.name) {
                    case "Master Volume Slider":
                        slider.value = SoundManager.Instance.MasterVolume;
                        slider.onValueChanged.AddListener(SoundManager.Instance.SetMasterVolumeLevel);
                        break;
                    case "Music Volume Slider":
                        slider.value = SoundManager.Instance.MusicVolume;
                        slider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolumeLevel);
                        break;
                    case "SFX Volume Slider":
                        slider.value = SoundManager.Instance.SfxVolume;
                        slider.onValueChanged.AddListener(SoundManager.Instance.SetSfxVolumeLevel);
                        break;
                }
            }
        }
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
