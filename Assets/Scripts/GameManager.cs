using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int keys = 0;
    public int maxKeys;
    public int maxEnemies;
    public int maxPowerUps;
    public int score = 0;
    public int scorePerKey = 100;

    public string difficulty;



    public TextMeshProUGUI keytxt;
    public TextMeshProUGUI scoretxt;
    string maxKeyText;

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
            maxKeys = 6;
            maxEnemies = 6;
            maxPowerUps = 2;
        }
        else if (difficulty == "Dificil")
        {
            maxKeys = 7;
            maxKeyText = maxKeys.ToString();
            maxEnemies = 5;
            maxPowerUps = 3;
        }
        else
        {
            maxKeys = 5;
            maxKeyText = maxKeys.ToString();
            maxEnemies = 4;
            maxPowerUps = 3;
            Debug.Log(maxKeys);
        }

    }
    void Update()
    {


        keytxt.text = keys.ToString() + "/" + maxKeyText;
        scoretxt.text = "Score: " + score.ToString();

    }

}