using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    public Transform obstacleSpawnPoint;
    public float obstacleSpawnDelayMin = 2f;
    public float obstacleSpawnDelayMax = 5f;

    [Header("Grass Settings")]
    public GameObject[] grassPrefabs;
    public Transform grassSpawnPoint;
    public float grassWidth = 2f;
    public int initialGrassCount = 10;

    private bool isSpawning = false;
    private float lastGrassX;

    public void StartSpawning()
    {
        isSpawning = true;

        // Spawn initial grass
        for (int i = 0; i < initialGrassCount; i++)
        {
            SpawnGrass(grassSpawnPoint.position.x + (i * grassWidth));
        }

        StartCoroutine(SpawnObstacles());
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    public void RestartSpawning()
    {
        StopSpawning();
        DestroyAllWithTag("Obstacle");
        DestroyAllWithTag("Grass");
        lastGrassX = grassSpawnPoint.position.x;
        StartSpawning();
    }

    private IEnumerator SpawnObstacles()
    {
        while (isSpawning)
        {
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(obstaclePrefab, obstacleSpawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(obstacleSpawnDelayMin, obstacleSpawnDelayMax));
        }
    }

    private void SpawnGrass(float xPosition)
    {
        GameObject grassPrefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
        Vector3 spawnPosition = new Vector3(xPosition, grassSpawnPoint.position.y, grassSpawnPoint.position.z);

        Instantiate(grassPrefab, spawnPosition, Quaternion.identity);

        lastGrassX = xPosition;
    }

    private void Update()
    {
        if (!isSpawning) return;

        if (Camera.main.transform.position.x + (grassWidth * 5) > lastGrassX)
        {
            SpawnGrass(lastGrassX + grassWidth);
        }
    }

    private void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}
