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
    string dificultad;
    int maxKeys;
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
                Debug.Log("En dificultad normal hacen spawn " + maxKeys + "llaves");
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
        int maxAttempts = 50;
        bool positionValid = false;
        Vector3 spawnPosition = Vector3.zero;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 candidatePosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                spawnAreaMax.y + 5f, // Elevado para lanzar raycast hacia abajo
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            // Raycast hacia abajo desde una altura para comprobar si hay suelo
            if (Physics.Raycast(candidatePosition, Vector3.down, out RaycastHit hit, 20f))
            {
                // Comprovem que l'etiqueta del terra sigui "Ground" (o el que tu vulguis)
                if (hit.collider.CompareTag("Ground"))
                {
                    spawnPosition = hit.point + Vector3.up * 0.5f; // Apareix just sobre el terra
                    positionValid = true;
                    break;
                }
                else
                {
                    Debug.Log("No es pot generar la clau aquí, el terra no és vàlid.");
                }
            }
        }

        if (!positionValid)
        {
            Debug.LogWarning("No s'ha pogut trobar una posició vàlida per generar la clau després de molts intents.");
            return;
        }

        GameObject key = Instantiate(keyPrefab, spawnPosition, Quaternion.identity);
        key.tag = "Key";
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
