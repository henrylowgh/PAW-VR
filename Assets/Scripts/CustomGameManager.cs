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

    public GameObject dogObject; 
    public GameObject wallObject; // Invisible wall to put in front of dog
    public GameObject wallSpawnPositionObject;
    public GameObject boundsObject; // Assign the large empty GameObject Cube in the inspector
    private float lastBreathInputTime = 0f;
    private float breathBufferPeriod = 10f;
    private bool hasInstantiatedWall = false;


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
        CheckBreathInput();
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

    public void CheckBreathInput()
    {
        // Check for breath input
        if (Input.GetKeyDown(KeyCode.Space)) // Temporary use space button as placeholder input
        {
            lastBreathInputTime = Time.time;
        }

        // Check if 10 seconds have passed since the last breath
        // Debug.Log("Within bounds: " + IsWithinBounds());
        // Debug.Log("Time since last breath input: " + (Time.time - lastBreathInputTime));
        if (Time.time - lastBreathInputTime > breathBufferPeriod)
        {
            // Check if dog object is outside the bounds of boundary box
            if (!IsWithinBounds() && !hasInstantiatedWall)
            {
                //wallObject.SetActive(true);
                Instantiate(wallObject, wallSpawnPositionObject.transform.position, dogObject.transform.rotation);
                hasInstantiatedWall = true;
            }
        }  
        else
        {
            DestroyWalls();
        }

        if (IsWithinBounds()) {
            DestroyWalls();
        }
    }

    bool IsWithinBounds()
    {
        // Checking if dog object is outside of the boundsObject
        Bounds bounds = boundsObject.GetComponent<Collider>().bounds;
        return bounds.Contains(dogObject.transform.position);
    }

    public void DestroyWalls()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        hasInstantiatedWall = false;
    }

}
