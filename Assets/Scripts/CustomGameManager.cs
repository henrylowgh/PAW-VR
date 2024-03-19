using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomGameManager : MonoBehaviour
{
    public GameObject memoryGameObject;
    private Vector3 memoryGamePosition;
    public TextMeshProUGUI ballScoreUI;
    public TextMeshProUGUI distanceScoreUI;
    public DistanceTracker distanceTracker;
    public Button restartButton;
    [SerializeField] private int ballScore;
    [SerializeField] private int distanceScore;
    [SerializeField] private int bonusScore;

    public float timeLeft; // Start time in seconds (10 min)
    public TextMeshProUGUI timeUI; // Reference to a UI Text element to display the timer
    [SerializeField] private bool isCountingScore;


    // Start is called before the first frame update
    void Start()
    {
        memoryGamePosition = new Vector3(0, 1.5f, 8);
        ballScore = 0;
        distanceScore = 0;
        bonusScore = 0; //Bonus points from each ball
        isCountingScore = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        UpdateScores();
    }

    public void UpdateTime()
    {
        {
            if (timeLeft > 0)
            {
                // Decrease the time left
                timeLeft -= Time.deltaTime;

                // Update the display
                if (timeUI != null)
                {
                    timeUI.text = "Time Left: " + FormatTime(timeLeft);
                }
            }
            else
            {
                // Time is up
                timeLeft = 0;
                // Handle what happens when the timer reaches zero
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        distanceTracker.DisableTracking();
        isCountingScore = false;
        restartButton.gameObject.SetActive(true);
    }

    public void ReloadScene()
    {
        Debug.Log("clicked");
        Scene currentScene = SceneManager.GetActiveScene();
        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
    }

    public void UpdateScores()
    {
        if (isCountingScore)
        {
            distanceScore = Mathf.RoundToInt(distanceTracker.GetTotalDistance()) + bonusScore; // Can rescale #s
            ballScoreUI.text = "Balls: " + ballScore;
            distanceScoreUI.text = "Score: " + distanceScore;
        }
    }

    public void ResetMemoryGame()
    {
        Instantiate(memoryGameObject, memoryGamePosition, Quaternion.identity);
    }

    public void IncreaseBallScore()
    {
        ballScore++;
        bonusScore += 100;
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
