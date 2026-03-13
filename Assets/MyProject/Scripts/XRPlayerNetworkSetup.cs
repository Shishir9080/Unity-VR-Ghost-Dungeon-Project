using Unity.Netcode;
using UnityEngine;

public class XRPlayerNetworkSetup : NetworkBehaviour
{
    public Camera playerCamera;
    public AudioListener playerAudioListener;

    void Start()
    {
        if (!IsOwner)
        {
            if (playerCamera != null)
                playerCamera.enabled = false;

            if (playerAudioListener != null)
                playerAudioListener.enabled = false;
        }
    }
}