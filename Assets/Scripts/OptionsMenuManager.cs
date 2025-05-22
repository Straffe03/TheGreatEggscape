using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenuManager : MonoBehaviour
{
    public Button normalButton;
    public Button dificilButton;
    public Button endlessButton;
    public Toggle musicToggle;

    public AudioSource musicSource;

    private string selectedDifficulty;

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu"); //escena menu principal
    }

    private void Start()
    {
        selectedDifficulty = PlayerPrefs.GetString("Difficulty");
        // Assignar listeners
        normalButton.onClick.AddListener(() => SelectDifficulty("Normal"));
        dificilButton.onClick.AddListener(() => SelectDifficulty("Dificil"));
        endlessButton.onClick.AddListener(() => SelectDifficulty("Endless"));

        musicToggle.onValueChanged.AddListener(ToggleMusic);

        // Inicialitzar estat
        SelectDifficulty(selectedDifficulty);
        musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
    }

    public void SelectDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;
        //Para recuperar la dificultad en otras escenas sera string dificultad = PlayerPrefs.GetString("Difficulty", "Normal");
        PlayerPrefs.SetString("Difficulty", difficulty);
        PlayerPrefs.Save();
        UpdateButtonColors();
    }


    public void ToggleMusic(bool isOn)
    {
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetMute(!isOn);
    }

    private void UpdateButtonColors()
    {
        Color selectedColor = Color.green;
        Color defaultColor = Color.gray;

        normalButton.GetComponent<Image>().color = (selectedDifficulty == "Normal") ? selectedColor : defaultColor;
        dificilButton.GetComponent<Image>().color = (selectedDifficulty == "Dificil") ? selectedColor : defaultColor;
        endlessButton.GetComponent<Image>().color = (selectedDifficulty == "Endless") ? selectedColor : defaultColor;
    }
}
