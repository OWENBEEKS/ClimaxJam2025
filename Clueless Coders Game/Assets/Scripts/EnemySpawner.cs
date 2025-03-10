using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    private List<Transform> floorTransforms = new List<Transform>();
    private float spawnInterval = 0.5f; // Initial spawn interval

    // Start is called before the first frame update
    void Start()
    {
        // Find all objects with the tag "Floor" and store their transforms
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        foreach (GameObject floor in floors)
        {
            floorTransforms.Add(floor.transform);
        }

        // Start the spawning coroutine
        StartCoroutine(SpawnEnemies());

        // Start the coroutine to decrease the spawn interval
        StartCoroutine(DecreaseSpawnInterval());
    }

    // Coroutine to spawn enemies at intervals
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Method to spawn an enemy at a random position on a random floor
    void SpawnEnemy()
    {
        if (floorTransforms.Count == 0) return;

        // Select a random floor
        Transform randomFloor = floorTransforms[Random.Range(0, floorTransforms.Count)];

        // Get the bounds of the floor
        Renderer floorRenderer = randomFloor.GetComponent<Renderer>();
        if (floorRenderer == null) return;

        Bounds bounds = floorRenderer.bounds;

        // Generate a random position within the bounds of the floor
        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        // Instantiate the enemy prefab at the random position
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }

    // Coroutine to decrease the spawn interval every 60 seconds
    IEnumerator DecreaseSpawnInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(60); // Wait for 60 seconds
            spawnInterval = Mathf.Max(0.1f, spawnInterval - 0.1f); // Decrease spawn interval by 0.1 seconds, but not below 0.1 seconds
        }
    }
}
