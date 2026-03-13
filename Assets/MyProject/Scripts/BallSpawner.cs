using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2f;

    private TargetManager targetManager;

    void Start()
    {
        targetManager = FindFirstObjectByType<TargetManager>();
        SpawnBall();
    }

    public void SpawnBall()
    {
        if (targetManager != null && targetManager.IsGameEnded()) return;
        if (ballPrefab == null || spawnPoint == null) return;

        GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        BallRespawn respawn = newBall.GetComponent<BallRespawn>();
        if (respawn == null)
            respawn = newBall.AddComponent<BallRespawn>();

        respawn.spawner = this;

        Debug.Log("BallSpawner: Spawned a new ball.");
    }

    public void BallTaken()
    {
        if (targetManager != null && targetManager.IsGameEnded()) return;

        CancelInvoke(nameof(SpawnBall));
        Invoke(nameof(SpawnBall), spawnDelay);

        Debug.Log("BallSpawner: Ball taken, scheduling respawn.");
    }
}