using UnityEngine;

public class TargetHit : MonoBehaviour
{
    private TargetManager targetManager;
    private bool hasBeenHit = false;

    private void Start()
    {
        targetManager = FindObjectOfType<TargetManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenHit) return;
        if (targetManager == null) return;
        if (targetManager.IsGameEnded()) return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            hasBeenHit = true;

            targetManager.AddScore();
            targetManager.SpawnTarget();

            Destroy(gameObject);
        }
    }
}