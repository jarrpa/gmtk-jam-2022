using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator attackCardAnimator;
    public Animator moveCardAnimator;
    private GameObject healthBar;
    public GameObject player;

    public Sprite[] healthSprites;

    public Image healthImage;

    private GameObject wavesPanel;
    public TMP_Text wavesText;

    public GameObject canvas;
    public GameObject abilityPanel;
    public GameObject pausePanel;
    public GameObject pauseTitle;
    public Button winButton;
    private Image pauseTitleImage;
    public Sprite[] pauseTitleSprites;
    public GameEvent gameOverEvent;
    public GameEvent pauseEvent;
    public GameEvent waveChangeEvent;
    public GameEvent wavesDoneEvent;
    public LevelEvent playerVictoryEvent;
    public GameEvent bazookaPickupEvent;
    public EntityEvent entityHitEvent;

    private static UIManager Instance;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;

        // Events we trigger
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");

        // Events we listen
        waveChangeEvent ??= GameEventLoader.Load<GameEvent>("WaveChangeEvent");
        playerVictoryEvent ??= GameEventLoader.Load<LevelEvent>("PlayerVictoryLevelEvent");
        gameOverEvent ??= GameEventLoader.Load<GameEvent>("GameOverEvent");
        pauseEvent ??= GameEventLoader.Load<GameEvent>("PauseEvent");
        bazookaPickupEvent ??= GameEventLoader.Load<GameEvent>("BazookaPickupEvent");
        entityHitEvent ??= GameEventLoader.Load<EntityEvent>("EntityHitEvent");

        waveChangeEvent?.AddListener(UpdateWaveCount);
        playerVictoryEvent?.AddListener(WinPanel);
        gameOverEvent?.AddListener(GameOverPanel);
        pauseEvent?.AddListener(PauseGame);
        bazookaPickupEvent?.AddListener(OnBazookaPickup);
        entityHitEvent?.AddListener(UpdatePlayerHealth);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnBazookaPickup()
    {
        abilityPanel?.SetActive(true);
        healthBar?.SetActive(true);
        pausePanel.transform.Find("Restart Button")?.gameObject.SetActive(true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FindObjects();
    }

    private void FindObjects() {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerAbilityController>().OnAbilityChange += PlayCardAnimations;

            canvas = GameObject.Find("Canvas");
            abilityPanel = canvas.transform.Find("Ability Panel")?.gameObject;
            attackCardAnimator = abilityPanel.transform.Find("Attack Card Holder").GetComponent<Animator>();
            moveCardAnimator = abilityPanel.transform.Find("Movement Card Holder").GetComponent<Animator>();

            healthBar = canvas.transform.Find("HealthBar")?.gameObject;
            healthImage = healthBar.GetComponent<Image>();

            wavesPanel = canvas.transform.Find("Waves Panel")?.gameObject;
            wavesText = wavesPanel.transform.Find("Waves Count Text").GetComponent<TextMeshProUGUI>();

            pausePanel = canvas.transform.Find("Pause Panel")?.gameObject;
            pauseTitle = pausePanel.transform.Find("Pause Title")?.gameObject;

            winButton = canvas.transform.Find("WinButton")?.gameObject.GetComponent<Button>();
            winButton.onClick.AddListener(ClearLevel);
        }
    }

    public void ClearLevel() {
        var gameManager = Singleton.Instance.GameManager;
        var waveManager = Singleton.Instance.WaveManager;

        gameManager.StopAllCoroutines();
        waveManager.StopAllCoroutines();
        waveManager.currentWave = waveManager.waves.Length;
        waveManager.WaveCompleted();
        UpdateWaveCount();
    }

    private void OnDisable()
    {
        if (player != null)
            player.GetComponent<PlayerAbilityController>().OnAbilityChange -= PlayCardAnimations;
    }

    private void OnDestroy()
    {
        waveChangeEvent?.RemoveListener(UpdateWaveCount);
        playerVictoryEvent?.RemoveListener(WinPanel);
        gameOverEvent?.RemoveListener(GameOverPanel);
        pauseEvent?.RemoveListener(PauseGame);
        entityHitEvent?.RemoveListener(UpdatePlayerHealth);

        Time.timeScale = 1f;
    }

    private void UpdatePlayerHealth(EntityPayload hitData)
    {
        if (hitData.entity.CompareTag("Player"))
        {
            var health = hitData.entity.currentHealth;
            for (int i = 100; i >= 0; i -= 20)
            {
                if (health > i - 20 && health < i)
                {
                    healthImage.sprite = healthSprites[((100 - i) / 20) % healthSprites.Length];
                }
            }
        }
    }

    private void UpdateWaveCount()
    {
        if (!wavesPanel.activeInHierarchy) wavesPanel.SetActive(true);
        wavesText.text = Singleton.Instance.WaveManager.currentWave + " / " + Singleton.Instance.WaveManager.waves.Length;
    }

    private void PlayCardAnimations(int ability, int weapon)
    {
        attackCardAnimator.Play("card_" + weapon);
        moveCardAnimator.Play("card_" + ability);
    }

    private void PauseGame()
    {
        pauseTitle.GetComponent<Image>().sprite = pauseTitleSprites[0];
        pausePanel.SetActive(Singleton.Instance.GameManager.gameIsPaused);
    }

    private void GameOverPanel()
    {
        pauseTitle.GetComponent<Image>().sprite = pauseTitleSprites[1];
        pausePanel.SetActive(true);
    }

    private void WinPanel(LevelPayload payload)
    {
        pauseTitle.GetComponent<Image>().sprite = pauseTitleSprites[2];
        pausePanel.SetActive(true);
    }


    public void Exit()
    {

    }

}
