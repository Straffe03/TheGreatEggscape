using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int keys = 0;
    public static int pickUps = 0;
    public int maxKeys;
    public int maxEnemies;
    public int maxPowerUps;
    public static int score = 0;
    public int scorePerPowerUpSpeed = 50;
    public int scorePerKey = 100;

    public string difficulty;

    public static bool victory;


    public TextMeshProUGUI keytxt;
    public TextMeshProUGUI scoretxt;
    public TextMeshProUGUI powerupTimerText;
    public Slider healthSlider;

    string maxKeyText;

    private float remainingTime = 0f;
    private bool isActive = false;

    public void ActivateTimer(float duration)
    {
        remainingTime = duration;
        isActive = true;
        UpdatePowerUpTimer();
    }



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // mata duplicados
        }
    }
    public void KeyCollected()
    {
        Debug.Log("Clau recollida! 🗝️");
        score += scorePerKey;
        keys++;
        CheckVictory();
    }
    void Start()
    {
        Debug.Log("Pasa por el start");
        difficulty = PlayerPrefs.GetString("Difficulty");
        Debug.Log(difficulty);
        if (difficulty == null)
        {
            difficulty = "Normal";
        }

        if (difficulty == "Endless")
        {
            maxKeyText = "Infinite";
            maxKeys = 5;
            maxEnemies = 6;
            maxPowerUps = 2;
        }
        else if (difficulty == "Dificil")
        {
            maxKeys = 15;
            maxKeyText = maxKeys.ToString();
            maxEnemies = 5;
            maxPowerUps = 3;
        }
        else
        {
            maxKeys = 10;
            maxKeyText = maxKeys.ToString();
            maxEnemies = 4;
            maxPowerUps = 3;
            Debug.Log(maxKeys);
        }

    }
    void Update()
    {
        if (keytxt != null)
        {
            keytxt.text = keys.ToString() + "/" + maxKeyText;
            scoretxt.text = "Score: " + score.ToString();
        }
        if (!isActive) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            isActive = false;
            powerupTimerText.text = "Power-Up: --"; // o "Power-Up terminado"
        }
        else
        {
            UpdatePowerUpTimer();
        }

    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void SetHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    void CheckVictory()
    {
        if (difficulty != "Endless" && maxKeys == keys)
        {
            Debug.Log("Ganar: iniciando escena final...");
            victory = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuFinal");
        }
    }
    public void PlayerLoss()
    {
        victory = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuFinal");
    }

    void UpdatePowerUpTimer()
    {
        if (powerupTimerText == null)
        {
            Debug.LogWarning("PowerUpTimerText no asignado en el GameManager.");
            return;
        }
        powerupTimerText.text = $"Power-Up: {remainingTime:F1}s";
    }


    public void ResetGameManager()
        {
        Destroy(gameObject);
    }

}