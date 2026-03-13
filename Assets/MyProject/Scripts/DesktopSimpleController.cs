using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopSimpleController : NetworkBehaviour
{
    public Transform cameraTransform;
    public float moveSpeed = 3f;
    public float turnSpeed = 90f;
    public float lookSpeed = 70f;

    private float pitch = 0f;
    private Camera cam;
    private AudioListener audioListener;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        audioListener = GetComponentInChildren<AudioListener>();

        if (IsOwner)
        {
            if (cam != null) cam.enabled = true;
            if (audioListener != null) audioListener.enabled = true;
        }
        else
        {
            if (cam != null) cam.enabled = false;
            if (audioListener != null) audioListener.enabled = false;
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector3 move = Vector3.zero;

        if (keyboard.wKey.isPressed) move += transform.forward;
        if (keyboard.sKey.isPressed) move -= transform.forward;
        if (keyboard.aKey.isPressed) move -= transform.right;
        if (keyboard.dKey.isPressed) move += transform.right;

        move.y = 0f;

        if (move.sqrMagnitude > 0.001f)
            transform.position += move.normalized * moveSpeed * Time.deltaTime;

        if (keyboard.qKey.isPressed)
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);

        if (keyboard.eKey.isPressed)
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        if (cameraTransform != null)
        {
            if (keyboard.upArrowKey.isPressed)
                pitch -= lookSpeed * Time.deltaTime;

            if (keyboard.downArrowKey.isPressed)
                pitch += lookSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, -80f, 80f);
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}