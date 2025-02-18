﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

///<summary>
/// Inherits from MemoryControllerBase
///</summary>
public class MemoryControlllerEasy : MemoryControllerBase
{
	[Header("Continue counting time during flip over?")]
	public bool continueTime;

    /* Custom code */
    [SerializeField] public GameObject ballObject;
    [SerializeField] public GameObject spawnObject; // Reposition location
    [SerializeField] public DistanceTracker distanceTracker;

	new void Start()
	{
        ballObject = GameObject.Find("Red Ball");
        spawnObject = GameObject.Find("SpawnObject");
        distanceTracker = ballObject.GetComponent<DistanceTracker>();
        base.Start();
	}

	void Update()
	{
        base.Update();

        //if game state is MEMORY, continue playing
        if (GameController.GameControllerProperties.CurrentGameState == GameState.MEMORY)
		{
			HandleTime();
		}

		//if game state is FLIP_OVER, continue counting time
		if (GameController.GameControllerProperties.CurrentGameState == GameState.FLIP_OVER)
		{
			if (continueTime)
				HandleTime();
		}

		//restart game
		if (GameController.GameControllerProperties.CurrentGameState == GameState.GAME_OVER)
			//HandleRestartGame();
			StartCoroutine(DestroyGameInstance());
    }

	#region EXTERNAL METHODS
	///<summary>
	/// Overrides from MemoryControllerBase
	/// Check wheter score or not
	///</summary>
	public override void CheckMatch()
	{
		if (!HasEnoughCardsFacedUp())
			return;

		if (DoWeHaveAMatch())
		{
			//stop game momentarily
			canPlay = false;

			//if combo, apply combo score
			if (canCombo)
			{
				//add a combo score
				AddScore(scorePerComboMatch);
				//increment combo
				countCombo++;
				//update gui showing combo
				guiInstance.showCombo.text = countCombo.ToString();
			}

			//otherwise, simple score
			else
			{
				//add a simple score
				AddScore(scorePerSimpleMatch);
			}

			//hide current cards faced up that are equals
			StartCoroutine(HideCardAfterMatch());
			StopCoroutine(HideCardAfterMatch());
		}

		//if they are not equals, get back to game
		else
		{
			//Desativa o combo
			canCombo = false;
			//Vira a peca
			StartCoroutine(FaceCardDown());
			StopCoroutine(FaceCardDown());
		}
		// }
	}

	#endregion

	#region INTERNAL METHODS

	///<summary>
	/// Handle restart game
	///</summary>
	void HandleRestartGame()
	{
		if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
		{
			GameController.GameControllerProperties.CurrentGameState = GameState.MEMORY;
			SceneManager.LoadScene(0);
		}
	}

	///<summary>
	/// Handle game time
	///</summary>
	void HandleTime()
	{
		if (canStartTime)
		{
			countSeconds += Time.deltaTime;
		}

		if (countSeconds >= 60)
		{
			countMinutes++;
			countSeconds = 0;
		}

		guiInstance.showMinuteTime.text = string.Format("{00:00}:", countMinutes);
		guiInstance.showSecondTime.text = countSeconds.ToString("F0");

		//game over
		if (countMatch >= memoryCards.Length / 2)
		{
			//end game, do something
			ShowPlayerScoreLevel((countMinutes * 60) + countSeconds);
			GameController.GameControllerProperties.CurrentGameState = GameState.GAME_OVER;
            RepositionBall();
            countMatch = 0;
        }
	}

	///<summary>
	/// Hide card after playe match
	///</summary>
	IEnumerator HideCardAfterMatch()
	{
		//wait to proceed
		yield return new WaitForSeconds(0.6f);

		ResetCardsAfterMatch();

		canPlay = true;
	}

	void RepositionBall()
	{
		ballObject.transform.position = spawnObject.transform.position;
		distanceTracker.EnableTracking();
	}

	IEnumerator DestroyGameInstance()
	{
        yield return new WaitForSeconds(1f);  // Wait for 1 second
		//Debug.Log("Destroying!");
		GameController.GameControllerProperties.ResetGameState();
        Destroy(transform.parent.gameObject);
    }

    #endregion
}
