using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VRSYS.Core.Avatar;

namespace VRLabClass.Teleport
{
    public class AnchoredTeleportNavigation : MonoBehaviour
    {
        #region Teleport Parameters
        
        // Reference to the Teleport Action, e.g. Trigger
        public InputActionReference TeleportAction;
        
        // Maximum length of the selection ray.
        public float rayLength = 10.0f;
        
        // The threshold for entering Phase 1 of the teleportation process.
        public float rayActivationThreshold;
        
        // The threshold for entering Phase 2 of the teleportation process.
        public float setTargetThreshold;

        // Layer mask for selectable anchors
        public LayerMask anchorLayerMask;
        
        // Layer mask for navigatable surfaces
        public LayerMask groundLayerMask;
        
        // Selection ray visual
        public LineRenderer TeleportRay;
        
        // Visual feedback elements
        public GameObject TeleportVisuals;
        public GameObject TeleportAnchor;
        public GameObject TeleportTarget;
        public GameObject PreviewAvatar;
        
        #endregion

        #region Internal References and States

        private Transform headTransform;
        private Transform handTransform;
        
        // The current valid target point selected by raycasting
        private Vector3 currentHitPoint;
        
        // The point selected as the anchor point for the anchored teleport, to which the player will be oriented after the teleport.
        private Vector3 teleportAnchorPoint;
        
        // The point selected as the target point for the teleport, where the player will be teleported to
        private Vector3 teleportTargetPoint;

        #endregion
        
        void Start()
        {
            // TODO: (Topic 2.4) Enforce local-player control in multi-user scenarios
            
            headTransform = GetComponent<AvatarHMDAnatomy>().head;
            handTransform = GetComponent<AvatarHMDAnatomy>().rightHand;
            
            // TODO: Make sure the input action is enabled
            // TODO: Set the initial state of the ray and feedback elements
            TeleportVisuals.transform.localPosition = Vector3.one;
            PreviewAvatar.transform.localPosition = new Vector3(0, 1.5f, 0);
        }

        void Update()
        {
            // TODO: Read input and compare against thresholds
            // TODO: If Phase 3: Teleport the user to the target
            // TODO: Update the visibility and placement of the ray and feedback elements
        }
    }
}