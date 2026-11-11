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

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip pizzaPickupSound;
    [SerializeField] private AudioClip deliverySuccessSound;

    public int CurrentScore { get; private set; }
    public bool HasPizza { get; private set; }

    private float timer;
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
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    public void PickUpPizza()
    {
        if (HasPizza) return;

        HasPizza = true;
        UpdatePizzaUI();

        if (pizzaPickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pizzaPickupSound);
        }
    }

    public bool DeliverPizza()
    {
        if (!HasPizza) return false;

        HasPizza = false;
        AddScore(10);
        UpdatePizzaUI();

        if (deliverySuccessSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deliverySuccessSound);
        }

        return true;
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