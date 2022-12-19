using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //private Dictionary<string, AsyncOperation> loadOperations = new Dictionary<string, AsyncOperation>();

    private static MenuManager Instance;

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
        // TODO: Figure out async loading...
        //menuScene = SceneManager.GetSceneByName("Menu");
        //creditsScene = SceneManager.GetSceneByName("Credits");
        //optionsScene = SceneManager.GetSceneByName("Options");

        //StartCoroutine(LoadSceneAsyncProcess("Menu"));
        //StartCoroutine(LoadSceneAsyncProcess("Credits"));
        //StartCoroutine(LoadSceneAsyncProcess("Options"));

        FindObjects();
    }

    private void FindObjects()
    {
        var canvas = GameObject.Find("Canvas");
        var menuCards = canvas.transform.Find("Menu Cards")?.gameObject;
        var cardButtons = menuCards?.GetComponentsInChildren<Button>();
        if (cardButtons != null)
        {
            foreach (Button button in cardButtons)
            {
                switch (button.name)
                {
                    case "Exit Card":
                        button.onClick.AddListener(ExitGame);
                        break;
                    case "Back Card":
                        button.onClick.AddListener(MainMenu);
                        break;
                    case "Credits Card":
                        button.onClick.AddListener(ShowCredits);
                        break;
                    case "Options Card":
                        button.onClick.AddListener(ShowOptions);
                        break;
                    case "Play Card":
                        button.onClick.AddListener(PlayGame);
                        break;
                }
            }
        }

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var pausePanel = canvas.transform.Find("Pause Panel")?.gameObject;
            var buttons = pausePanel?.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                switch (button.name)
                {
                    case "Restart Button":
                        button.onClick.AddListener(RestartLevel);
                        break;
                    case "Exit Button":
                        button.onClick.AddListener(ExitGame);
                        break;
                    case "Menu Button":
                        button.onClick.AddListener(MainMenu);
                        break;
                }
            }
        }
    }

    // TODO: Figure out async loading...
    // private IEnumerator LoadSceneAsyncProcess(string sceneName)
    // {
    //     if (SceneManager.GetSceneByName(sceneName).IsValid()) yield break;

    //     var asyncLoad = SceneManager.LoadSceneAsync(sceneName); //, LoadSceneMode.Additive);
    //     asyncLoad.allowSceneActivation = false;
    //     //asyncLoad.completed += ;

    //     loadOperations.Add(sceneName, asyncLoad);

    //     while (!asyncLoad.isDone)
    //     {
    //         //Debug.Log($"[scene]:{sceneName} [load progress]: {asyncLoad.progress}");
    //         yield return null;
    //     }
    //     Debug.Log($"[scene]:{sceneName} [load progress]: {asyncLoad.progress}");
    // }

    public void NextScene(string sceneName)
    {
        MusicController.Instance.SetMusicState(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void PlayGame()
    {
        // TODO: Probably expose this somewhere...
        NextScene("demo-level-jarrpa");
    }

    public void ShowCredits()
    {
        NextScene("Credits");
    }

    public void ShowOptions()
    {
        NextScene("Options");
    }

    public void MainMenu()
    {
        NextScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}