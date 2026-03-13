using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviour
{
    public GameObject xrPlayerPrefab;
    public GameObject desktopPlayerPrefab;

    public Transform hostSpawnPoint;
    public Transform clientSpawnPoint;

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        GameObject prefabToSpawn;
        Transform spawnPoint;

        if (clientId == NetworkManager.ServerClientId)
        {
            prefabToSpawn = xrPlayerPrefab;
            spawnPoint = hostSpawnPoint;
        }
        else
        {
            prefabToSpawn = desktopPlayerPrefab;
            spawnPoint = clientSpawnPoint;
        }

        GameObject player = Instantiate(
            prefabToSpawn,
            spawnPoint.position,
            spawnPoint.rotation
        );

        NetworkObject netObj = player.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId, true);
    }
}