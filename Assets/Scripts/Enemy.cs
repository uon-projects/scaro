using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] string robotType;
	[SerializeField] GameObject misslePrefab;
	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip fireSound;
	[SerializeField] AudioClip weakHitSound;

	Transform player;
	float timeLastFired;
	bool isDead;

	public int health;
	public int range;
	public float fireRate;
	public Transform missleFireSpot;
	public Animator robot;
	UnityEngine.AI.NavMeshAgent agent;

	// Start is called before the first frame update
	void Start()
	{
		//1
		isDead = false;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update()
	{
		//2
		if (isDead)
		{
			return;
		}

		//3
		transform.LookAt(player);

		//4
		agent.SetDestination(player.position);

		//5
		if (Vector3.Distance(transform.position, player.position) < range && Time.time - timeLastFired > fireRate)
		{
			//6
			timeLastFired = Time.time;
			Fire();
		}
	}

	void Fire()
	{
		GameObject missile = Instantiate(misslePrefab);
		missile.transform.position = missleFireSpot.transform.position;
		missile.transform.rotation = missleFireSpot.transform.rotation;
		robot.Play("Fire");
		GetComponent<AudioSource>().PlayOneShot(fireSound);
	}

	public void takeDamage(int amount)
	{
		if (isDead)
		{
			return;
		}

		health -= amount;
		if (health <= 0)
		{
			isDead = true;
			robot.Play("Die");
			StartCoroutine("DestroyRobot");
			// GameManager.RemoveEnemy();
			GetComponent<AudioSource>().PlayOneShot(deathSound);
		}
		else
		{
			GetComponent<AudioSource>().PlayOneShot(weakHitSound);
		}
	}

	IEnumerator DestroyRobot()
	{
		yield return new WaitForSeconds(1.5f);
		Destroy(gameObject);
	}
}