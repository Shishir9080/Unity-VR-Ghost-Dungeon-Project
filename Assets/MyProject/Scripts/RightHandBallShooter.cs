using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class RightHandBallShooter : MonoBehaviour
{
    public XRBaseInteractor rightInteractor;
    public Transform shootPoint;
    public float shootForce = 15f;

    [Header("Optional - assign if you want")]
    public InputActionReference triggerAction;

    private bool triggerWasPressedLastFrame = false;

    void Awake()
    {
        if (triggerAction == null)
        {
            TryFindTriggerAction();
        }
    }

    void OnEnable()
    {
        if (triggerAction != null && triggerAction.action != null)
        {
            triggerAction.action.Enable();
        }
    }

    void Update()
    {
        if (triggerAction == null || triggerAction.action == null) return;

        float triggerValue = triggerAction.action.ReadValue<float>();
        bool triggerPressed = triggerValue > 0.5f;

        if (triggerPressed && !triggerWasPressedLastFrame)
        {
            TryShootHeldBall();
        }

        triggerWasPressedLastFrame = triggerPressed;
    }

    void TryShootHeldBall()
    {
        if (rightInteractor == null) return;
        if (rightInteractor.interactablesSelected.Count == 0) return;

        XRGrabInteractable grabbedBall =
            rightInteractor.interactablesSelected[0] as XRGrabInteractable;

        if (grabbedBall == null) return;
        if (!grabbedBall.CompareTag("Ball")) return;

        Rigidbody rb = grabbedBall.GetComponent<Rigidbody>();
        if (rb == null) return;

        StartCoroutine(ReleaseAndShoot(grabbedBall, rb));
    }

    IEnumerator ReleaseAndShoot(XRGrabInteractable grabbedBall, Rigidbody rb)
    {
        grabbedBall.enabled = false;

        yield return null;

        grabbedBall.enabled = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
    }

    void TryFindTriggerAction()
    {
        var inputManagers = FindObjectsByType<InputActionManager>(FindObjectsSortMode.None);

        foreach (var manager in inputManagers)
        {
            if (manager == null || manager.actionAssets == null) continue;

            foreach (var asset in manager.actionAssets)
            {
                if (asset == null) continue;

                var action = asset.FindAction("XRI Right Interaction/Activate Value", true);
                if (action != null)
                {
                    triggerAction = InputActionReference.Create(action);
                    Debug.Log("RightHandBallShooter: Found trigger action at runtime.");
                    return;
                }
            }
        }

        Debug.LogWarning("RightHandBallShooter: Could not find 'XRI Right Interaction/Activate Value'.");
    }
}