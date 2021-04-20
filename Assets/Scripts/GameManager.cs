using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager singleton;

	[SerializeField] private List<EnemySpawn> spawns = new List<EnemySpawn>();

	public GameObject player;
	public int enemiesLeft;
	public int score;
	public int waveCountDown;
	public bool isGameOver;

	public void AddSpawner(EnemySpawn spawner)
	{
		spawns.Add(spawner);
	}

	private void Start()
	{
		singleton = this;
		StartCoroutine("increaseScoreEachSecond");
		isGameOver = false;
		Time.timeScale = 1;
		waveCountDown = 30;
		enemiesLeft = 0;
		StartCoroutine("updateWaveTimer");
		spawnRobots();
	}

	void spawnRobots()
	{
		foreach (EnemySpawn spawn in spawns)
		{
			// spawn.SpawnRobot();
			// enemiesLeft++;
		}
	}

	IEnumerator updateWaveTimer()
	{
		while (!isGameOver)
		{
			yield return new WaitForSeconds(1);
			waveCountDown--;

			if (waveCountDown == 0)
			{
				spawnRobots();
				waveCountDown = 30;
			}
		}
	}

	public static void RemoveEnemy()
	{
		singleton.enemiesLeft--;

		if (singleton.enemiesLeft == 0)
		{
			singleton.score += 50;
		}
	}

	public void AddRobotKillToScore()
	{
		score += 10;
	}

	IEnumerator increaseScoreEachSecond()
	{
		while (!isGameOver)
		{
			yield return new WaitForSeconds(1);
			score += 1;
		}
	}

	public void OnGUI()
	{
		if (isGameOver && Cursor.visible == false)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	public void gameOver()
	{
		isGameOver = true;
		Time.timeScale = 0;
		player.GetComponent<CharacterController>().enabled = false;
		SceneManager.LoadScene(Constants.SceneBattle);
	}

	public void restartGame()
	{
		SceneManager.LoadScene(Constants.SceneBattle);
	}

	public void exit()
	{
		Application.Quit();
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(Constants.SceneMenu);
	}
}