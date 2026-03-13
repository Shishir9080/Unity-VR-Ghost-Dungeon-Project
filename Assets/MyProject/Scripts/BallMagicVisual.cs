using UnityEngine;

public class BallMagicVisual : MonoBehaviour
{
    public Renderer ballRenderer;
    public Light ballLight;

    public float pulseSpeed = 3f;
    public float glowStrength = 1.5f;

    private Material materialInstance;
    private TargetManager targetManager;

    void Start()
    {
        if (ballRenderer == null)
            ballRenderer = GetComponentInChildren<Renderer>();

        if (ballRenderer != null)
            materialInstance = ballRenderer.material;

        targetManager = FindFirstObjectByType<TargetManager>();
    }

    void Update()
    {
        if (materialInstance == null || targetManager == null) return;

        int score = targetManager.GetScore();
        Color baseColor = GetColorForScore(score);

        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(0.7f, glowStrength, pulse);

        Color finalColor = baseColor * intensity;

        // Main material color
        materialInstance.color = finalColor;

        // Emission glow
        if (materialInstance.HasProperty("_EmissionColor"))
        {
            materialInstance.EnableKeyword("_EMISSION");
            materialInstance.SetColor("_EmissionColor", finalColor);
        }

        // Optional light glow
        if (ballLight != null)
        {
            ballLight.color = baseColor;
            ballLight.intensity = Mathf.Lerp(0.5f, 2f, pulse);
        }
    }

    Color GetColorForScore(int score)
    {
        if (score <= 3)
            return Color.white;
        else if (score <= 8)
            return new Color(0.5f, 0.8f, 1f); // light blue
        else if (score <= 13)
            return new Color(0.1f, 0.4f, 1f); // darker blue
        else if (score <= 17)
            return Color.yellow;
        else
            return Color.red;
    }
}