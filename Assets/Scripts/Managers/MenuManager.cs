using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public LevelEvent levelChangeEvent;
    public string firstLevel = "Room00";
    public static MenuManager Instance;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;

        levelChangeEvent ??= GameEventLoader.Load<LevelEvent>("LevelChangeEvent");
        levelChangeEvent?.AddListener(NextScene);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

    public void NextScene(LevelPayload payload) {
        var levelSettings = payload.levelSettings;
        var musicState = levelSettings.musicState == "" ? MusicController.Instance.DefaultMusicState : levelSettings.musicState;
        MusicController.Instance.SetMusicState(musicState);
        SceneManager.LoadScene(levelSettings.sceneName);
    }

    public void NextScene(string sceneName)
    {
        MusicController.Instance.SetMusicState(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void PlayGame()
    {
        Singleton.Instance.GameManager.gameState = GameState.StartRoom;
        NextScene(firstLevel);
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

    private void OnDestroy() {
        levelChangeEvent?.RemoveListener(NextScene);
    }
}