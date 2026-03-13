using Unity.Netcode;
using UnityEngine;

public class NetworkStarter : MonoBehaviour
{
    public enum NetworkRole
    {
        None,
        Host,
        Client
    }

    public NetworkRole role = NetworkRole.None;

    void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is null.");
            return;
        }

        switch (role)
        {
            case NetworkRole.Host:
                NetworkManager.Singleton.StartHost();
                Debug.Log("Started as Host");
                break;

            case NetworkRole.Client:
                NetworkManager.Singleton.StartClient();
                Debug.Log("Started as Client");
                break;
        }
    }
}