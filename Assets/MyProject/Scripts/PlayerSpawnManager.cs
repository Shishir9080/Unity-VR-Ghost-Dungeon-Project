using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform hostSpawnPoint;
    public Transform clientSpawnPoint;

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnServerStarted()
    {
        PositionAllPlayers();
    }

    private void OnClientConnected(ulong clientId)
    {
        PositionAllPlayers();
    }

    private void PositionAllPlayers()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        foreach (var clientPair in NetworkManager.Singleton.ConnectedClients)
        {
            ulong clientId = clientPair.Key;
            NetworkObject playerObject = clientPair.Value.PlayerObject;

            if (playerObject == null) continue;

            Transform targetSpawn = (clientId == NetworkManager.ServerClientId)
                ? hostSpawnPoint
                : clientSpawnPoint;

            if (targetSpawn != null)
            {
                playerObject.transform.SetPositionAndRotation(
                    targetSpawn.position,
                    targetSpawn.rotation
                );
            }
        }
    }
}