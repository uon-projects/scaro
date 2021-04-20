using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]
public class GameController : MonoBehaviour
{
	[SerializeField] private Text timeLabel;
	[SerializeField] private Text scoreLabel;

	private MazeConstructor generator;

	private DateTime startTime;
	private int timeLimit;
	private int reduceLimitBy;

	private int score;
	private bool goalReached;

	// Use this for initialization
	void Start()
	{
		generator = GetComponent<MazeConstructor>();
		StartNewGame();
	}

	private void StartNewGame()
	{
		timeLimit = 300;
		reduceLimitBy = 5;
		startTime = DateTime.Now;

		score = 0;
		scoreLabel.text = score.ToString();

		StartNewMaze();
	}

	private void StartNewMaze()
	{
		generator.GenerateNewMaze(40, 40, OnStartTrigger, OnGoalTrigger);

		float x = generator.startCol * generator.hallWidth;
		float y = 1;
		float z = generator.startRow * generator.hallWidth;

		goalReached = false;

		// restart timer
		timeLimit -= reduceLimitBy;
		startTime = DateTime.Now;
	}

	// Update is called once per frame
	void Update()
	{
		int timeUsed = (int) (DateTime.Now - startTime).TotalSeconds;
		int timeLeft = timeLimit - timeUsed;

		if (timeLeft > 0)
		{
			timeLabel.text = timeLeft.ToString();
		}
		else
		{
			timeLabel.text = "TIME UP";
			SceneManager.LoadScene(Constants.SceneGameOver);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// SceneManager.LoadScene(Constants.SceneGameMenu, LoadSceneMode.Single);
		}
	}

	private void OnGoalTrigger(GameObject trigger, GameObject other)
	{
		// Debug.Log("Goal!");
		goalReached = true;

		score += 1;
		scoreLabel.text = score.ToString();

		Destroy(trigger);
	}

	private void OnStartTrigger(GameObject trigger, GameObject other)
	{
		if (goalReached)
		{
			// Debug.Log("Finish!");
			SceneManager.LoadScene(Constants.SceneGameWon, LoadSceneMode.Single);
		}
	}
}