using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Classe que controla la transició des de la pantalla inicial del joc cap al menú principal.
 * Mostra la splash screen durant un temps definit i després carrega l’escena del menú.
 */
public class SplashScreenController : MonoBehaviour
{
    // Durada de la splash screen en segons
    [SerializeField] private float splashDuration = 3f;

    // Nom de l’escena del menú principal (ha de coincidir amb el nom exacte)
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    /**
     * Inicia la corrutina que espera el temps de durada i carrega el menú principal.
     */
    private void Start()
    {
        StartCoroutine(LoadMainMenuAfterDelay());
    }

    /**
     * Espera uns segons i després carrega l’escena del menú.
     */
    private System.Collections.IEnumerator LoadMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(splashDuration);
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
