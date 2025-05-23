using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Classe encarregada de generar i controlar els power-ups en funció del mode de joc.
 * En modes Normal i Difícil: només apareixen un nombre limitat de power-ups i no es regeneren.
 * En mode Endless: es manté un nombre màxim i es regeneren després de recollir-los amb un retard.
 * @author Straffe
 */
public class PowerUpSpawner : MonoBehaviour
{
    [Header("Configuració general")]
    public GameObject powerUpPrefab;
    public GameManager gameManager;
    private string dificultat;
    private int maxPowerUps;

    [Header("Àrea de generació")]
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    [Header("Configuració Endless")]
    public float respawnDelay = 5f; // Temps de retard abans de reapareixer un power-up

    private int totalGenerated = 0;
    private List<GameObject> activePowerUps = new List<GameObject>();

    void Start()
    {
        dificultat = PlayerPrefs.GetString("Difficulty");
        maxPowerUps = gameManager.maxPowerUps;

        switch (dificultat)
        {
            case "Normal":
            case "Dificil":
                SpawnInitialPowerUps(maxPowerUps);
                break;
            case "Endless":
                for (int i = 0; i < maxPowerUps; i++)
                    SpawnPowerUp();
                break;
        }
    }

    private void SpawnInitialPowerUps(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnPowerUp();
            totalGenerated++;
        }
    }

    private void SpawnPowerUp()
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
                    spawnPosition = hit.point + Vector3.up * 0.5f;
                    positionValid = true;
                    break;
                }
            }
        }

        if (!positionValid)
        {
            Debug.LogWarning("No s'ha trobat una posició vàlida per al power-up.");
            return;
        }

        GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        powerUp.tag = "PowerUp";
        activePowerUps.Add(powerUp);

        powerUp.GetComponent<PowerUpSpeed>().onCollected += () =>
        {
            activePowerUps.Remove(powerUp);
            Destroy(powerUp);

            if (dificultat != "Endless")
            {
                totalGenerated++;
                return;
            }

            // ENDLESS: espera i torna a generar
            StartCoroutine(RespawnPowerUpAfterDelay());
        };
    }

    private IEnumerator RespawnPowerUpAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        if (activePowerUps.Count < maxPowerUps)
            SpawnPowerUp();
    }
}
