using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
