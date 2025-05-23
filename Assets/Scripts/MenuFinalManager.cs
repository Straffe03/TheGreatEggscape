using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFinalManager : MonoBehaviour
{
    bool victory;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pickUpText;
    public TextMeshProUGUI keyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        victory = GameManager.victory;
        if (victory)
        {
            winText.text = "You Won!";
            winText.color = Color.green;
        }
        else
        {
            winText.text = "You Lost!";
            winText.color = Color.red;
        }
        scoreText.text = "YOUR FINAL SCORE " + GameManager.score.ToString();
        pickUpText.text = "PICKUPS: " + GameManager.pickUps.ToString();
        keyText.text = "KEYS: " + GameManager.keys.ToString();
    }

    public void BackToMenu()
    {
        Debug.Log("Tornant al menú principal...");
        SceneManager.LoadScene("MainMenu");
        GameManager.score = 0;
        GameManager.keys = 0;
        GameManager.pickUps = 0;
    }

    public void QuitGame()
    {
        Debug.Log("Surt del joc!");
        Application.Quit();
    }
}
