using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float moveDistance = 1.5f;   // how far left/right
    public float moveSpeed = 2f;        // movement speed

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        transform.position = startPosition + new Vector3(movement, 0f, 0f);
    }
}