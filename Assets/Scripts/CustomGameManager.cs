using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomGameManager : MonoBehaviour
{
    public GameObject memoryGameObject;
    private Vector3 memoryGamePosition;
    public TextMeshProUGUI ballScoreUI;
    public TextMeshProUGUI distanceScoreUI;
    [SerializeField] private int ballScore;
    [SerializeField] private int distanceScore;

    // Start is called before the first frame update
    void Start()
    {
        memoryGamePosition = new Vector3(0, 1.5f, 8);
        ballScore = 0;
        distanceScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ballScoreUI.text = "Balls: " + ballScore;
        distanceScoreUI.text = "Score: " + distanceScore;
    }

    public void ResetMemoryGame()
    {
        Instantiate(memoryGameObject, memoryGamePosition, Quaternion.identity);
    }

    public void IncreaseBallScore()
    {
        ballScore++;
        distanceScore += 100;
    }
}
