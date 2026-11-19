using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI Reference")]
    [Tooltip("Drag your TextMeshPro UGUI text element here.")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [Tooltip("Optional: Text showing whether you are carrying a pizza.")]
    [SerializeField] private TextMeshProUGUI pizzaStatusText;

    [Header("2D Sound Effects & Mix Balance")]
    [SerializeField] private AudioClip coinSound;
    [Range(0f, 1f)] [SerializeField] private float coinVolume = 0.5f;

    [SerializeField] private AudioClip pizzaPickupSound;
    [Range(0f, 1f)] [SerializeField] private float pizzaPickupVolume = 1f;

    [SerializeField] private AudioClip deliverySuccessSound;
    [Range(0f, 1f)] [SerializeField] private float deliverySuccessVolume = 1f;

    public int CurrentScore { get; private set; }
    public bool HasPizza { get; private set; }

    private float timer;
    private float lastCoinSoundTime = -1f;
    private AudioSource audioSource;
    private bool isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Force pure 2D centered stereo audio so sounds never pan left/right or fade with distance
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    private void Start()
    {
        ResetScore();
    }

    private void Update()
    {
        if (isGameOver) return;

        // Adds +1 score every 1 second of survival
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            AddScore(1);
            timer -= 1f;
        }
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        timer = 0f;
        HasPizza = false;
        UpdateScoreUI();
        UpdatePizzaUI();
    }

    public void CollectCoin()
    {
        AddScore(1);

        // Tiny 0.04s guard prevents rapid back-to-back coins from phase-glitching
        if (coinSound != null && Time.unscaledTime - lastCoinSoundTime >= 0.04f)
        {
            lastCoinSoundTime = Time.unscaledTime;
            PlaySFX(coinSound, coinVolume);
        }
    }

    public void PickUpPizza()
    {
        if (HasPizza) return;

        HasPizza = true;
        UpdatePizzaUI();
        PlaySFX(pizzaPickupSound, pizzaPickupVolume);
    }

    public bool DeliverPizza()
    {
        if (!HasPizza) return false;

        HasPizza = false;
        AddScore(10);
        UpdatePizzaUI();
        PlaySFX(deliverySuccessSound, deliverySuccessVolume);

        return true;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void StopScoring()
    {
        isGameOver = true;
    }

    public void ResumeScoring()
    {
        isGameOver = false;
        timer = 0f;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {CurrentScore}";
        }
    }

    private void UpdatePizzaUI()
    {
        if (pizzaStatusText != null)
        {
            pizzaStatusText.text = HasPizza ? "PIZZA: READY TO DELIVER!" : "PIZZA: NONE";
        }
    }
}