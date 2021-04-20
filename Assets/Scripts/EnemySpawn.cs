using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	[SerializeField] GameObject[] robots;

	int timeSpawned;

	int healthBonus = 0;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void SpawnRobot()
	{
		timeSpawned++;
		healthBonus += 1 * timeSpawned;
		GameObject robot = Instantiate(robots[Random.Range(0, robots.Length)]);
		robot.transform.position = transform.position;
		robot.GetComponent<Enemy>().health += healthBonus;
	}
}