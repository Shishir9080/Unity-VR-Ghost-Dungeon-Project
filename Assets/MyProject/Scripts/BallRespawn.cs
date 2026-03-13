using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallRespawn : MonoBehaviour
{
    public BallSpawner spawner;

    private XRGrabInteractable grabInteractable;
    private bool hasTriggeredRespawn = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }
    }

    private void OnGrabbed(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (hasTriggeredRespawn) return;

        hasTriggeredRespawn = true;

        if (spawner != null)
        {
            spawner.BallTaken();
            Debug.Log("BallRespawn: Ball grabbed, told spawner to respawn.");
        }
    }
}