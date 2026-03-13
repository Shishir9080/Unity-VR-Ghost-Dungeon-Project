using UnityEngine;

public class CanvasEventCameraSetter : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        TryAssignCamera();
    }

    void Update()
    {
        if (canvas != null && canvas.worldCamera == null)
        {
            TryAssignCamera();
        }
    }

    void TryAssignCamera()
    {
        Camera cam = Camera.main;

        if (cam != null && canvas != null)
        {
            canvas.worldCamera = cam;
        }
    }
}