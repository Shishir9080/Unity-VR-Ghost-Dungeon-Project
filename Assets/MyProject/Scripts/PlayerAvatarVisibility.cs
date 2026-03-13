using Unity.Netcode;
using UnityEngine;

public class PlayerAvatarVisibility : NetworkBehaviour
{
    public GameObject avatarRoot;

    void Start()
    {
        if (IsOwner && avatarRoot != null)
        {
            avatarRoot.SetActive(false);
        }
    }
}