﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
	public bool showDebug;

	[SerializeField] private Material mazeMat1;
	[SerializeField] private Material mazeMat2;
	[SerializeField] private Material startMat;
	[SerializeField] private Material treasureMat;
	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private GameObject playerObj;

	public int[,] data { get; private set; }

	public float hallWidth { get; private set; }
	public float hallHeight { get; private set; }

	public int startRow { get; private set; }
	public int startCol { get; private set; }

	public int goalRow { get; private set; }
	public int goalCol { get; private set; }

	private MazeDataGenerator dataGenerator;
	private MazeMeshGenerator meshGenerator;
	private List<Vector2> elementsOnMap;

	void Awake()
	{
		dataGenerator = new MazeDataGenerator();
		meshGenerator = new MazeMeshGenerator();
		elementsOnMap = new List<Vector2>();

		// default to walls surrounding a single empty cell
		data = new[,]
		{
			{1, 1, 1},
			{1, 0, 1},
			{1, 1, 1}
		};
	}

	public void GenerateNewMaze(int sizeRows, int sizeCols,
		TriggerEventHandler startCallback = null, TriggerEventHandler goalCallback = null)
	{
		DisposeOldMaze();

		data = dataGenerator.FromDimensions(sizeRows, sizeCols);

		FindStartPosition();
		FindGoalPosition();

		// store values used to generate this mesh
		hallWidth = meshGenerator.width;
		hallHeight = meshGenerator.height;

		DisplayMaze();

		PlaceStartTrigger(startCallback);
		PlaceGoalTrigger(goalCallback);
		GenerateSpawnerPosition();
	}

	private void DisplayMaze()
	{
		GameObject go = new GameObject();
		go.transform.position = Vector3.zero;
		go.name = "Procedural Maze";
		go.tag = "Generated";

		MeshFilter mf = go.AddComponent<MeshFilter>();
		mf.mesh = meshGenerator.FromData(data);

		MeshCollider mc = go.AddComponent<MeshCollider>();
		mc.sharedMesh = mf.mesh;

		MeshRenderer mr = go.AddComponent<MeshRenderer>();
		mr.materials = new Material[2] {mazeMat1, mazeMat2};
	}

	public void DisposeOldMaze()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
		foreach (GameObject go in objects)
		{
			Destroy(go);
		}
	}

	private void FindStartPosition()
	{
		int[,] maze = data;
		int rMax = maze.GetUpperBound(0);
		int cMax = maze.GetUpperBound(1);

		for (int i = 0; i <= rMax; i++)
		{
			for (int j = 0; j <= cMax; j++)
			{
				if (maze[i, j] == 0)
				{
					elementsOnMap.Add(new Vector2(i, j));
					startRow = i;
					startCol = j;
					return;
				}
			}
		}
	}

	private void FindGoalPosition()
	{
		int[,] maze = data;
		int rMax = maze.GetUpperBound(0);
		int cMax = maze.GetUpperBound(1);

		for (int i = rMax; i >= 0; i--)
		{
			for (int j = cMax; j >= 0; j--)
			{
				if (maze[i, j] == 0 && !elementsOnMap.Contains(new Vector2(i, j)))
				{
					elementsOnMap.Add(new Vector2(i, j));
					goalRow = i;
					goalCol = j;
					return;
				}
			}
		}
	}

	private void GenerateSpawnerPosition()
	{
		int[,] maze = data;
		int rMax = maze.GetUpperBound(0);
		int cMax = maze.GetUpperBound(1);

		for (int i = rMax; i >= 0; i--)
		{
			for (int j = 0; j <= cMax; j++)
			{
				if (maze[i, j] == 0 && !elementsOnMap.Contains(new Vector2(i, j)))
				{
					elementsOnMap.Add(new Vector2(i, j));
					GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
					go.transform.position = new Vector3(i * hallWidth, .5f, j * hallWidth);
					go.transform.localScale = new Vector3(0, 0, 0);
					go.name = "Spawner";
					go.tag = "Generated";
					go.GetComponent<MeshRenderer>().sharedMaterial = startMat;
					go.AddComponent<EnemySpawner>().target = GameObject.Find("Player(Clone)");
					go.GetComponent<EnemySpawner>().spawnPoints.Add(
						new Vector3(
							go.transform.position.x,
							0.0f,
							go.transform.position.z
						)
					);
					return;
				}
			}
		}
	}

	private void PlaceStartTrigger(TriggerEventHandler callback)
	{
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.transform.position = new Vector3(startCol * hallWidth, .5f, startRow * hallWidth);
		go.name = "Start Trigger";
		go.tag = "Generated";

		go.GetComponent<BoxCollider>().isTrigger = true;
		go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

		playerObj = Instantiate(playerObj, go.transform.position, go.transform.rotation);

		TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
		tc.callback = callback;
	}

	private void PlaceGoalTrigger(TriggerEventHandler callback)
	{
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);
		go.name = "Treasure";
		go.tag = "Generated";

		go.GetComponent<BoxCollider>().isTrigger = true;
		go.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;

		TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
		tc.callback = callback;
	}

	// top-down debug display
	void OnGUI()
	{
		if (!showDebug)
		{
			return;
		}

		int[,] maze = data;
		int rMax = maze.GetUpperBound(0);
		int cMax = maze.GetUpperBound(1);

		string msg = "";

		// loop top to bottom, left to right
		for (int i = rMax; i >= 0; i--)
		{
			for (int j = 0; j <= cMax; j++)
			{
				if (maze[i, j] == 0)
				{
					msg += " ";
				}
				else
				{
					msg += "*";
				}
			}

			msg += "\n";
		}

		GUI.Label(new Rect(20, 20, 500, 500), msg);
	}
}