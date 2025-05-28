using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.GetString("Difficulty").Length == 0)
        {
            PlayerPrefs.SetString("Difficulty", "Normal");
        }
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject); // Evitar duplicats del GameManager
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Surt del joc!");
        Application.Quit();
    }
    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
