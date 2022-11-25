using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicScr : MonoBehaviour
{
    public AK.Wwise.Event Music;
    public AK.Wwise.RTPC RoomRTPC;
    string sceneName;
    private static MusicScr Instance;
    Scene m_Scene;

    private void Awake()
    {
        if (Instance)
		{
			DestroyImmediate(this);
			return;
		}
		Instance = this;
    }
    void Start()
    {
        SetMusicState();
        Music.Post(Instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SetMusicState();
    }

     void SetMusicState()
    {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        if (m_Scene.name == "Menu") AkSoundEngine.SetState("STATE_Music", "STATE_Menu");
        else AkSoundEngine.SetState("STATE_Music", "STATE_Rooms");

        if (m_Scene.name == "Map_1") RoomRTPC.SetValue(gameObject, 1);
        else if (m_Scene.name == "Map_2") RoomRTPC.SetValue(gameObject, 2);
        else if (m_Scene.name == "Map_3") RoomRTPC.SetValue(gameObject, 3);
        else RoomRTPC.SetValue(gameObject, 3);
    }
}
