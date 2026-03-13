using UnityEngine;
using TMPro;
using System.Collections;

public class TargetManager : MonoBehaviour
{
    [Header("Target Spawn")]
    public GameObject targetPrefab;
    public Transform[] spawnPoints;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;

    [Header("Game Settings")]
    public float roundDuration = 60f;

    [Header("Optional")]
    public BallSpawner ballSpawner;

    private GameObject currentTarget;
    private int score = 0;
    private float gameTime;
    private bool gameEnded = false;

    void Start()
    {
        StartNewRound();
    }

    void Update()
    {
        if (gameEnded) return;

        gameTime -= Time.deltaTime;

        if (gameTime <= 0f)
        {
            gameTime = 0f;
            EndGame();
        }

        UpdateTimerUI();
    }

    public void SpawnTarget()
    {
        if (gameEnded) return;
        if (targetPrefab == null) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        int index = Random.Range(0, spawnPoints.Length);
        currentTarget = Instantiate(targetPrefab, spawnPoints[index].position, spawnPoints[index].rotation);
    }

    public void AddScore()
    {
        if (gameEnded) return;

        score++;
        UpdateScoreUI();
    }

    public int GetScore()
    {

        return score;
        
    }


    public bool IsGameEnded()
    {
        return gameEnded;
    }

    void EndGame()
    {
        gameEnded = true;

        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }

        StartCoroutine(RestartCountdown());
    }

    IEnumerator RestartCountdown()
    {
        int countdown = 5;

        while (countdown > 0)
        {
            if (gameOverText != null)
            {
                gameOverText.gameObject.SetActive(true);
                gameOverText.text = "Next Round Starting In\n" + countdown;
            }

            yield return new WaitForSeconds(1f);
            countdown--;
        }

        ResetRound();
    }

    void ResetRound()
    {
        // Reset core values
        score = 0;
        gameTime = roundDuration;
        gameEnded = false;

        // Reset UI
        UpdateScoreUI();
        UpdateTimerUI();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        // Remove old target if somehow still there
        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }

        // Destroy all existing balls
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }

        // Spawn fresh target
        SpawnTarget();

        // Spawn fresh ready-ball
        if (ballSpawner != null)
        {
            ballSpawner.CancelInvoke();
            ballSpawner.SpawnBall();
        }
    }

    void StartNewRound()
    {
        score = 0;
        gameTime = roundDuration;
        gameEnded = false;

        UpdateScoreUI();
        UpdateTimerUI();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }

        SpawnTarget();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(gameTime);
        }
    }
}