using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Classe encarregada de generar enemics segons el mode de joc.
 * En modes Normal i Difícil es generen enemics fixes.
 * En mode Endless es generen enemics extra al llarg del temps.
 * @author Straffe
 */
public class EnemySpawner : MonoBehaviour
{
    [Header("Configuració general")]
    public GameObject enemyPrefab;
    public GameManager gameManager;
    private string dificultat;
    private int maxInitialEnemies;

    [Header("Àrea de generació")]
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    [Header("Configuració Endless")]
    public float extraSpawnInterval = 10f; // Segons entre nous enemics
    private bool endlessSpawning = false;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        dificultat = PlayerPrefs.GetString("Difficulty");
        maxInitialEnemies = gameManager.maxEnemies;

        SpawnEnemies(maxInitialEnemies);

        if (dificultat == "Endless")
        {
            endlessSpawning = true;
            StartCoroutine(SpawnExtraEnemiesOverTime());
        }
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int maxAttempts = 50;
        bool positionValid = false;
        Vector3 spawnPosition = Vector3.zero;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                spawnAreaMax.y + 5f,
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            if (Physics.Raycast(candidate, Vector3.down, out RaycastHit hit, 20f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    Vector3 potentialPosition = hit.point + Vector3.up * 0.5f;

                    // Zona d'exclusió al voltant del (0,0,0)
                    if (Mathf.Abs(potentialPosition.x) < 10f && Mathf.Abs(potentialPosition.z) < 10f)
                    {
                        Debug.Log("Posició exclosa: " + potentialPosition);
                        continue; // Prova una altra posició
                    }

                    spawnPosition = potentialPosition;
                    positionValid = true;
                    break;
                }
            }
        }

        if (!positionValid)
        {
            Debug.LogWarning("No s'ha trobat una posició vàlida per a l’enemic.");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.tag = "Enemy";
        activeEnemies.Add(enemy);
        Debug.Log("Enemic generat a la posició: " + spawnPosition);
    }

    private IEnumerator SpawnExtraEnemiesOverTime()
    {
        while (endlessSpawning)
        {
            yield return new WaitForSeconds(extraSpawnInterval);
            SpawnEnemy();
        }
    }
}
