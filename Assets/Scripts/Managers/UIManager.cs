using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator attackCardAnimator;
    public Animator moveCardAnimator;
    public GameObject player;

    public Sprite[] healthSprites;

    public Image healthImage;

    public TMP_Text wavesText;

    public GameObject pausePanel;
    public GameObject pauseTitle;
    private Image pauseTitleImage;
    public Sprite[] pauseTitleSprites;
    public GameEvent gameOverEvent;
    public GameEvent pauseEvent;
    public GameEvent waveChangeEvent;
    public GameEvent wavesDoneEvent;
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

        // Events we listen
        waveChangeEvent ??= GameEventLoader.Load<GameEvent>("WaveChangeEvent");
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");
        gameOverEvent ??= GameEventLoader.Load<GameEvent>("GameOverEvent");
        pauseEvent ??= GameEventLoader.Load<GameEvent>("PauseEvent");
        entityHitEvent ??= GameEventLoader.Load<EntityEvent>("EntityHitEvent");

        waveChangeEvent?.AddListener(UpdateWaveCount);
        wavesDoneEvent?.AddListener(WinPanel);
        gameOverEvent?.AddListener(GameOverPanel);
        pauseEvent?.AddListener(PauseGame);
        entityHitEvent?.AddListener(UpdatePlayerHealth);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FindObjects();
    }

    private void FindObjects() {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerAbilityController>().OnAbilityChange += PlayCardAnimations;

            var canvas = GameObject.Find("Canvas");
            var abilityPanel = canvas.transform.Find("Ability Panel")?.gameObject;
            attackCardAnimator = abilityPanel.transform.Find("Attack Card Holder").GetComponent<Animator>();
            moveCardAnimator = abilityPanel.transform.Find("Movement Card Holder").GetComponent<Animator>();

            var healthBar = canvas.transform.Find("HealthBar")?.gameObject;
            healthImage = healthBar.GetComponent<Image>();

            var wavesPanel = canvas.transform.Find("Waves Panel")?.gameObject;
            wavesText = wavesPanel.transform.Find("Waves Count Text").GetComponent<TextMeshProUGUI>();

            pausePanel = canvas.transform.Find("Pause Panel")?.gameObject;
            pauseTitle = pausePanel.transform.Find("Pause Title")?.gameObject;
        }
    }

    private void OnDisable()
    {
        if (player != null)
            player.GetComponent<PlayerAbilityController>().OnAbilityChange -= PlayCardAnimations;
    }

    private void OnDestroy()
    {
        waveChangeEvent?.RemoveListener(UpdateWaveCount);
        wavesDoneEvent?.RemoveListener(WinPanel);
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

    private void WinPanel()
    {
        pauseTitle.GetComponent<Image>().sprite = pauseTitleSprites[2];
        pausePanel.SetActive(true);
    }


    public void Exit()
    {

    }

}
