using UnityEngine;
using Unity.XR.CoreUtils;

public class GroundFollowingXR : MonoBehaviour
{
    public XROrigin xrOrigin;
    public LayerMask groundLayer;
    public float rayStartOffset = 2f;
    public float rayDistance = 10f;

    void Update()
    {
        if (xrOrigin == null) return;

        // Real-world head height above the real floor
        float realHeadHeight = xrOrigin.CameraInOriginSpacePos.y;

        // Raycast downward onto virtual ground
        Vector3 rayOrigin = transform.position + Vector3.up * rayStartOffset;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, groundLayer))
        {
            Vector3 pos = transform.position;

            // X and Z remain controlled by user/steering
            // Y is corrected by ground following
            pos.y = hit.point.y + realHeadHeight;

            transform.position = pos;
        }
    }
}