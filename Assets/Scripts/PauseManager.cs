using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Slider volumeSlider;

    private bool isPaused = false;

    public AudioMixerGroup musicGroup;

    public GameObject playerController; // Assigna el script de moviment
    public GameObject playerCamera;     // Assigna el script de càmera

    public MonoBehaviour cameraControlScript;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        volumeSlider.value = AudioListener.volume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Activa components
        playerController.GetComponent<PlayerController>().enabled = true;
        cameraControlScript.enabled = true;
        
        AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allSources)
        {
            if (source.outputAudioMixerGroup != musicGroup)
            {
                source.UnPause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        AudioListener.pause = false; // mantenir àudio si vols
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Desactiva components
        playerController.GetComponent<PlayerController>().enabled = false;
        cameraControlScript.enabled = false;
        Time.timeScale = 0f;

        AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allSources)
        {
            if (source.outputAudioMixerGroup != musicGroup && source.isPlaying)
            {
                source.Pause();
            }
        }

    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


    public void LoadOptionsMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("OptionsMenu");
    }


    public void QuitGame()
    {
        Debug.Log("Sortint del joc...");
        Application.Quit();
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}

