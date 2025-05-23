using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Awake()
    {
        if(PlayerPrefs.GetString("Difficulty").Length == 0)
        {
            PlayerPrefs.SetString("Difficulty", "Normal");
        }
        if(GameManager.Instance != null)
        {
           Destroy(GameManager.Instance.gameObject); // Evitar duplicats del GameManager
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame"); // Substitueix pel nom real de la teva escena principal
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu"); // O pots obrir un panell si vols fer-ho tot en una sola escena
    }

    public void QuitGame()
    {
        Debug.Log("Surt del joc!");
        Application.Quit();
    }
}
