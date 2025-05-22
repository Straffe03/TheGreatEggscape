using System.Collections.Generic;
using UnityEngine;

/**
 * Classe que gestiona la generació i manteniment de les claus (llaves) en el mapa segons el mode de joc.
 * @author Straffe
 */
public class KeySpawner : MonoBehaviour
{
    [Header("Configuració general")]
    public GameObject keyPrefab;
    public GameManager gameManager;
    public int scorePerKey;
    string dificultad;
    public int maxKeys;
    [Header("Àrea de generació")]
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    private int totalKeysGenerated = 0;
    private List<GameObject> activeKeys = new List<GameObject>();

    void Start()
    {
        dificultad = PlayerPrefs.GetString("Difficulty");
        maxKeys = gameManager.maxKeys;
        switch (dificultad)
        {
            case "Normal":
                SpawnInitialKeys(maxKeys);
                break;
            case "Dificil":
                SpawnInitialKeys(maxKeys);
                break;
            case "Endless":
                for (int i = 0; i < maxKeys; i++)
                    SpawnKey();
                break;
        }
    }

    void Update()
    {
        if (dificultad == "Endless")
        {
            // Si falta alguna clau, en generem una altra
            while (activeKeys.Count < maxKeys)
                SpawnKey();
        }
    }

    private void SpawnInitialKeys(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnKey();
            totalKeysGenerated++;
        }
    }

    private void SpawnKey()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        GameObject key = Instantiate(keyPrefab, spawnPosition, Quaternion.identity);
        activeKeys.Add(key);

        key.GetComponent<Key>().onCollected += () =>
        {
            activeKeys.Remove(key);
            Destroy(key);

            if (dificultad != "Endless" && totalKeysGenerated >= maxKeys)
                return;

            totalKeysGenerated++;
        };
    }
}
