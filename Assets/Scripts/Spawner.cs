using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
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

    [SerializeField]
    private int spawnFantasmaWhenScore;
    private int simulSpawn;
    [SerializeField]
    private int minSimulSpawn, maxSimulSpawn, increaseSimulSpawn;

    private GameManager gm;
    private int fantasmasSpawned = 0;

	// Use this for initialization
	void Start () {
        gm = GetComponent<GameManager>();
        simulSpawn = minSimulSpawn;
        time = cooldown - 1;
	}
	
    void spawnEnemy(int type) {
        if (type == 0) {
            if (Random.Range(0, 2) > 0)
                Instantiate(enemies[0], new Vector3(10, Random.Range(-5f, 0f)), Quaternion.identity);
            else
                Instantiate(enemies[0], new Vector3(-10, Random.Range(-5f, 0f)), Quaternion.identity);
        }
        else if (type == 1) {
            Instantiate(enemies[1], new Vector3(Random.Range(-7f, 7f), Random.Range(-5f, 0f)), Quaternion.identity);
        }
        enemiesSpawned++;
        enemiesAlive++;
    }

	// Update is called once per frame
	void Update () {
		if (enemiesAlive < maxEnemies) {
			if (time >= cooldown) {
				time -= cooldown;

                for (int i = 0; i < simulSpawn && enemiesAlive < maxEnemies; i++)
                    if (gm.score >= spawnFantasmaWhenScore * (fantasmasSpawned+1)) {
                        spawnEnemy(1);
                        fantasmasSpawned++;
                    } else spawnEnemy(0);
			}
			/*if (enemiesSpawned >= cooldownReduceWhen) {
				cooldown -= cooldownReduce;
				cooldownReduceWhen += enemiesSpawned;
			}*/
			time += Time.deltaTime;
		}
        if (simulSpawn < maxSimulSpawn) {
            var a = minSimulSpawn + enemiesSpawned / increaseSimulSpawn;
            if (a <= maxSimulSpawn) simulSpawn = a;
        }
	}
}
