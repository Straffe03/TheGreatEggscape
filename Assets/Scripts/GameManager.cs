using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int keys = 0;
    public int maxKeys = 10;
    public int score = 0;

    public TextMeshProUGUI keytxt;
    public TextMeshProUGUI scoretxt;


    void Update()
    {
        keytxt.text = keys.ToString() + "/" + maxKeys.ToString();
        scoretxt.text = "Score: " + score.ToString(); 
        
    }

}