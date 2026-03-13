using Unity.Netcode;
using UnityEngine;

namespace VRLabClass.Milestone3
{
    public class GrabPolicy : NetworkBehaviour
    {
        private NetworkVariable<bool> _isGrabbed = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        private NetworkObject _networkObject;

        private void Awake()
        {
            _networkObject = GetComponent<NetworkObject>();
        }

        public bool RequestAccess()
        {
            Debug.Log($"[GrabPolicy] RequestAccess by client {NetworkManager.Singleton.LocalClientId}");

            if (_isGrabbed.Value)
            {
                Debug.Log($"[GrabPolicy] Access denied. Object {_networkObject.NetworkObjectId} is already grabbed.");
                return false;
            }

            RequestAccessRpc(NetworkManager.Singleton.LocalClientId);
            return true;
        }

        public void Release()
        {
            Debug.Log($"[GrabPolicy] Release by client {NetworkManager.Singleton.LocalClientId}");
            ReleaseRpc();
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void RequestAccessRpc(ulong requestingClientId)
        {
            if (_isGrabbed.Value)
            {
                Debug.Log($"[GrabPolicy] Server denied access. Object {_networkObject.NetworkObjectId} already grabbed.");
                return;
            }

            _isGrabbed.Value = true;

            if (_networkObject != null)
            {
                _networkObject.ChangeOwnership(requestingClientId);
                Debug.Log($"[GrabPolicy] Ownership transferred to client {requestingClientId}. ObjectId: {_networkObject.NetworkObjectId}");
            }
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void ReleaseRpc()
        {
            _isGrabbed.Value = false;

            if (_networkObject != null)
            {
                Debug.Log($"[GrabPolicy] Released. ObjectId: {_networkObject.NetworkObjectId}");
            }
        }
    }
}