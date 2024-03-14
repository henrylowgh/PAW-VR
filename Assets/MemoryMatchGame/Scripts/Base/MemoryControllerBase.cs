using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

///<summary>
/// Gui of the game
///</summary>
[System.Serializable]
public class memory_GUI
{
    [Tooltip("Show player scores")]
    public TextMesh showScore;
    [Tooltip("Show player combo")]
    public TextMesh showCombo;
    [Tooltip("Show minutes")]
    public TextMesh showMinuteTime;
    [Tooltip("Show seconds")]
    public TextMesh showSecondTime;
}

///<summary>
/// Abstract class that controls the memory match flow. You can simply inherit from this class to add as
/// many levels as you can.
///</summary>
public abstract class MemoryControllerBase : MonoBehaviour
{
    [Header("GUI of the game")]
    public memory_GUI guiInstance;
    [Header("Cards, drop them here")]
    public TouchCard[] memoryCards;
    //search cards automatically
    public bool searchCardsAutomatically;

    [Header("Diamonds score level")]
    public GameObject[] diamondsLevel;

    #region ADJUSTABLE VARIABLES
    [Header("Time (in seconds) to player score level")]
    [Tooltip("Player got a excelent time to finish, 3 diamonds")]
    public float excelentTime;
    [Tooltip("Player got a good time to finish, 2 diamonds, greater than this, 1 diamond")]
    public float goodTime;
    [Header("Score per simple and combo match")]
    [Tooltip("Player got a simple match")]
    public int scorePerSimpleMatch;
    [Tooltip("Player got a combo match")]
    public int scorePerComboMatch;
    [Header("Time (in seconds) to face card down")]
    [Tooltip("Time to face card down")]
    public float timeToFaceCardDown;
    [Header("Time (in seconds) to start the game")]
    [Tooltip("Time to start the game")]
    public float timeToStartGame;
    [Header("is using FlipOver at the beginning?")]
    [Tooltip("if using flip over at the beginning, check this, otherwise, uncheck")]
    public bool isUsingFlipOver;
    #endregion

    #region HIDEININSPECTOR VARIABLES
    //player can play the game
    public bool canPlay = true;
    //player can combo
    [HideInInspector]
    public bool canCombo = true;
    //scores
    [HideInInspector]
    public int scores;
    //count combo
    [HideInInspector]
    public int countCombo;
    //count player match
    [HideInInspector]
    public int countMatch;
    //start game time
    [HideInInspector]
    public bool canStartTime;
    //count seconds
    [HideInInspector]
    public float countSeconds;
    //count minutes
    [HideInInspector]
    public float countMinutes;
    private List<TouchCard> facedUpCards = new List<TouchCard>();

    #endregion

    private float matchBufferTime = 1.0f; // 1 second buffer to prevent immediate clicking after successful match
    private float bufferTimer;

    protected void Start()
    {
        if (searchCardsAutomatically)
            memoryCards = FindObjectsOfType<TouchCard>();

        SetCardsEvents();

        //wait to start game
        if (!isUsingFlipOver)
        {
            StartCoroutine(StartTime(timeToStartGame));
            StopCoroutine("StartTime");
        }

        //hide diamonds
        HideDiamonds();

    }

    protected void Update()
    {
        if (bufferTimer > 0)
        {
            bufferTimer -= Time.deltaTime;
        }
    }

    private void SetCardsEvents()
    {
        foreach (var card in memoryCards)
        {
            card.SetEvents(OnAddCard, CheckCardFaceUp);
        }
    }

    private void OnAddCard(TouchCard card)
    {
        facedUpCards.Add(card);
    }

    #region INTERNAL METHODS

    ///<summary>
    /// Hide diamonds score level
    ///</summary>
    void HideDiamonds()
    {
        for (int i = 0; i < diamondsLevel.Length; i++)
        {
            if (diamondsLevel[i] != null)
            {
                diamondsLevel[i].SetActive(false);
            }
        }
    }

    ///<summary>
    /// start game time to ontinue game
    ///</summary>
    public IEnumerator StartTime(float time)
    {
        yield return new WaitForSeconds(time);
        //start game time
        canStartTime = true;
    }

    #endregion

    #region EXTERNAL METHODS

    ///<summary>
    /// Check whether a card is face up, if so, increment the player move 
    /// and check if got score
    ///</summary>
    private void CheckCardFaceUp()
    {
        if (canPlay && GameController.GameControllerProperties.CurrentGameState == GameState.MEMORY)
        {
            //check if got score
            CheckMatch();
        }
    }

    //Method overrided in MemoryControllerEasy
    public abstract void CheckMatch();

    ///<summary>
    /// Add scores and update textmesh
    ///</summary>
    public void AddScore(int addScore)
    {
        //increment player match
        countMatch++;
        //add score
        scores += addScore;
        //update the gui showing the scores, avoid showing it in Update
        guiInstance.showScore.text = scores.ToString();
    }

    ///<summary>
    /// Show score level according to player time
    /// There are 3 levels: normal, good, excelent
    ///</summary>
    public void ShowPlayerScoreLevel(float currentTime)
    {
        //got 3 diamonds, excelent
        if (currentTime < excelentTime)
        {
            if (diamondsLevel[0] != null)
            {
                diamondsLevel[0].SetActive(true);
            }

            if (diamondsLevel[1] != null)
            {
                diamondsLevel[1].SetActive(true);
            }

            if (diamondsLevel[2] != null)
            {
                diamondsLevel[2].SetActive(true);
            }
        }

        //got 2 diamonds, good
        else if (currentTime > excelentTime && currentTime < goodTime)
        {
            if (diamondsLevel[0] != null)
            {
                diamondsLevel[0].SetActive(true);
            }

            if (diamondsLevel[1] != null)
            {
                diamondsLevel[1].SetActive(true);
            }

            if (diamondsLevel[2] != null)
            {
                diamondsLevel[2].SetActive(false);
            }
        }

        //got 1 diamond, normal
        else
        {
            if (diamondsLevel[0] != null)
            {
                diamondsLevel[0].SetActive(true);
            }

            if (diamondsLevel[1] != null)
            {
                diamondsLevel[1].SetActive(false);
            }

            if (diamondsLevel[2] != null)
            {
                diamondsLevel[2].SetActive(false);
            }
        }
    }
    ///<summary>
    /// Face down the current face up card
    ///</summary>
    public IEnumerator FaceCardDown()
    {
        //wait some seconds to proceed
        yield return new WaitForSeconds(timeToFaceCardDown);

        foreach (var card in facedUpCards)
        {
            card.SetAnimation(faceUp: false);
            card.SetIsClicked(false);
        }

        canPlay = true;

        facedUpCards.Clear();
    }

    public bool DoWeHaveAMatch()
    {
        if (facedUpCards.Count < 2)
            return false;

        return facedUpCards[0].cardId == facedUpCards[1].cardId;
    }

    public bool HasEnoughCardsFacedUp() => facedUpCards.Any() && facedUpCards.Count >= 2;

    public void ResetCardsAfterMatch()
    {
        if (!facedUpCards.Any())
            return;

        foreach (var card in facedUpCards)
        {
            card.ResetCollider();
            //show match effect
            card.EnableVictoryParticle(true);
            //hide card
            card.mesh.SetActive(false);
            //uncheck cards faced up
            card.SetIsClicked(false);
        }

        facedUpCards.Clear();
        bufferTimer = matchBufferTime; //reset buffer time after successful match
    }

    #endregion

    //Custom code to make sure that user cannot flip more than 2 cards at a time
    public bool CanFlipAnotherCard()
    {
        return facedUpCards.Count < 2;
    }

    // check if the buffer time has elapsed
    public bool IsBufferTimeElapsed()
    {
        return bufferTimer <= 0;
    }

}

