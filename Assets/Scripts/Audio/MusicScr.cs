using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicScr : MonoBehaviour
{
    public AK.Wwise.Event Music;
    public AK.Wwise.RTPC RoomRTPC;
    string sceneName;
    Scene m_Scene;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        if (m_Scene.name == "Menu_m") AkSoundEngine.SetState("STATE_Music", "STATE_Menu");
        else AkSoundEngine.SetState("STATE_Music", "STATE_Rooms");

        if (m_Scene.name == "Map_1") RoomRTPC.SetValue(gameObject, 1);
        if (m_Scene.name == "Map_2") RoomRTPC.SetValue(gameObject, 2);
        if (m_Scene.name == "Map_3") RoomRTPC.SetValue(gameObject, 3);
    }
}
