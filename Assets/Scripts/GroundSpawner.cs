using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab;  // Prefab of the ground tile
    public int initialTiles = 5;    // Number of ground tiles to spawn initially
    public Transform player;        // Reference to the player
    public float tileLength = 10f;  // Length of a single ground tile

    private List<GameObject> groundTiles = new List<GameObject>();
    private float spawnZ = 0f;      // Z position for the next tile spawn
    private float safeZone = 15f;  // Distance before despawning tiles

    public GameObject[] obstaclePrefabs;  // Array of obstacle prefabs

    private void SpawnTile()
    {
        // Instantiate ground tile
        GameObject newTile = Instantiate(groundPrefab, Vector3.right * spawnZ, Quaternion.identity);
        groundTiles.Add(newTile);

        // Spawn obstacles randomly
        int obstacleCount = Random.Range(1, 3);  // Random number of obstacles
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnZ, spawnZ + tileLength),
                newTile.transform.position.y + 1f,
                newTile.transform.position.z);
            Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], spawnPosition, Quaternion.identity);
        }

        spawnZ += tileLength;  // Update the next spawn position
    }

    void Start()
    {
        // Spawn initial ground tiles
        for (int i = 0; i < initialTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Spawn new tiles as the player progresses
        if (player.position.x > (spawnZ - initialTiles * tileLength))
        {
            SpawnTile();
            RemoveOldTile();
        }
    }

    private void RemoveOldTile()
    {
        // Remove the oldest tile if it's behind the safe zone
        if (groundTiles.Count > initialTiles)
        {
            Destroy(groundTiles[0]);
            groundTiles.RemoveAt(0);
        }
    }
}
