using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	[HideInInspector]
	public bool paused = false;

	public GameObject[] enemies;
	[SerializeField]
	private int maxEnemies;

	private float time = 0;
	private int enemiesSpawned = 0;
	[HideInInspector]
	public int enemiesAlive = 0;
	[SerializeField]
	private float cooldown;
	[SerializeField]
	private float cooldownReduce;
	[SerializeField]
	private int cooldownReduceWhen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!paused && enemiesAlive < maxEnemies) {
			time += Time.deltaTime;
			if (time >= cooldown) {
				time -= cooldown;
				if (Random.Range (0, 2) > 0)
					Instantiate (enemies [0], new Vector3 (8, Random.Range (-5f, 0f)), Quaternion.identity);
				else
					Instantiate (enemies [0], new Vector3 (-8, Random.Range (-5f, 0f)), Quaternion.identity);
				enemiesSpawned++;
				enemiesAlive++;
			}
			if (enemiesSpawned >= cooldownReduceWhen) {
				cooldown -= cooldownReduce;
				cooldownReduceWhen += enemiesSpawned;
			}
		}
	}
}
