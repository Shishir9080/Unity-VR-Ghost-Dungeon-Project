using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNinjaSpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [Tooltip("Center point of the spawn area")]
    public Transform spawnAreaCenter;
    
    [Tooltip("Size of the spawn area (X and Z dimensions)")]
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);
    
    [Tooltip("Y position where objects spawn from (ground level)")]
    public float groundLevel = 0f;

    [Header("Spawn Settings")]
    [Tooltip("List of prefabs to spawn randomly")]
    public List<GameObject> objectPrefabs = new List<GameObject>();
    
    [Tooltip("Minimum number of objects to spawn (1-3)")]
    [Range(1, 3)]
    public int minSpawnCount = 1;
    
    [Tooltip("Maximum number of objects to spawn (1-3)")]
    [Range(1, 3)]
    public int maxSpawnCount = 3;
    
    [Tooltip("Maximum number of spawn points (will be distributed randomly)")]
    [Range(1, 3)]
    public int maxSpawnPoints = 3;

    [Header("Launch Settings")]
    [Tooltip("Upward launch force")]
    public float launchForce = 10f;
    
    [Tooltip("Random variation in launch force")]
    public float launchForceVariation = 3f;
    
    [Tooltip("Random horizontal velocity range")]
    public float horizontalVelocityRange = 2f;
    
    [Tooltip("Random rotation on spawn")]
    public bool randomRotation = true;

    [Header("Timing")]
    [Tooltip("Time between spawn waves")]
    public float spawnInterval = 2f;
    
    [Tooltip("Random variation in spawn interval")]
    public float spawnIntervalVariation = 1f;
    
    [Tooltip("Start spawning automatically on Start")]
    public bool autoStart = true;

    private Coroutine spawnCoroutine;
    private List<Vector3> spawnPoints = new List<Vector3>();

    void Start()
    {
        if (autoStart)
        {
            StartSpawning();
        }
    }

    /// <summary>
    /// Start the spawning coroutine
    /// </summary>
    public void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// Stop spawning
    /// </summary>
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnWave();
            
            float waitTime = spawnInterval + Random.Range(-spawnIntervalVariation, spawnIntervalVariation);
            waitTime = Mathf.Max(0.1f, waitTime); // Ensure minimum wait time
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// Spawn a wave of objects
    /// </summary>
    public void SpawnWave()
    {
        if (objectPrefabs == null || objectPrefabs.Count == 0)
        {
            Debug.LogWarning("No object prefabs assigned to FruitNinjaSpawner!", this);
            return;
        }

        // Random number of objects to spawn (1 to maxSpawnCount)
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        
        // Generate random spawn points (up to maxSpawnPoints)
        int numSpawnPoints = Mathf.Min(spawnCount, maxSpawnPoints);
        GenerateSpawnPoints(numSpawnPoints);

        // Spawn objects at the generated points
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(i);
            SpawnObject(spawnPosition);
        }
    }

    /// <summary>
    /// Generate random spawn points within the spawn area
    /// </summary>
    private void GenerateSpawnPoints(int count)
    {
        spawnPoints.Clear();
        
        for (int i = 0; i < count; i++)
        {
            Vector3 center = spawnAreaCenter != null ? spawnAreaCenter.position : transform.position;
            
            // Random position within spawn area
            float x = center.x + Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float z = center.z + Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
            Vector3 spawnPoint = new Vector3(x, groundLevel, z);
            
            spawnPoints.Add(spawnPoint);
        }
    }

    /// <summary>
    /// Get spawn position for a given index (cycles through spawn points if needed)
    /// </summary>
    private Vector3 GetSpawnPosition(int index)
    {
        if (spawnPoints.Count == 0)
        {
            GenerateSpawnPoints(maxSpawnPoints);
        }
        
        int pointIndex = index % spawnPoints.Count;
        return spawnPoints[pointIndex];
    }

    /// <summary>
    /// Spawn a single object at the given position
    /// </summary>
    private void SpawnObject(Vector3 position)
    {
        // Random prefab selection
        int prefabIndex = Random.Range(0, objectPrefabs.Count);
        GameObject prefab = objectPrefabs[prefabIndex];
        
        if (prefab == null)
        {
            Debug.LogWarning("Null prefab in objectPrefabs list!", this);
            return;
        }

        // Instantiate object
        GameObject spawnedObject = Instantiate(prefab, position, Quaternion.identity);
        
        // Random rotation if enabled
        if (randomRotation)
        {
            spawnedObject.transform.rotation = Random.rotation;
        }

        // Apply physics force (upward launch)
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = spawnedObject.AddComponent<Rigidbody>();
        }

        // Calculate launch force
        float force = launchForce + Random.Range(-launchForceVariation, launchForceVariation);
        Vector3 launchDirection = Vector3.up;
        
        // Add random horizontal velocity
        if (horizontalVelocityRange > 0)
        {
            float horizontalX = Random.Range(-horizontalVelocityRange, horizontalVelocityRange);
            float horizontalZ = Random.Range(-horizontalVelocityRange, horizontalVelocityRange);
            launchDirection += new Vector3(horizontalX, 0, horizontalZ).normalized * 0.3f;
            launchDirection.Normalize();
        }

        // Apply force
        rb.AddForce(launchDirection * force, ForceMode.VelocityChange);
        
        // Add random angular velocity for spinning effect
        if (randomRotation)
        {
            rb.angularVelocity = new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f)
            );
        }
    }

    /// <summary>
    /// Draw spawn area gizmo in editor
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Vector3 center = spawnAreaCenter != null ? spawnAreaCenter.position : transform.position;
        center.y = groundLevel;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
        
        // Draw spawn points if they exist
        Gizmos.color = Color.red;
        foreach (Vector3 point in spawnPoints)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
    }
}