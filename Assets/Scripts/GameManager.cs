using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Text enemiesDefeated;
	public Image healthBar;
	public Player player;
	public GameObject[] items;
	public GameObject[] enemies;

	public int enemiesInt = 0;

	//private Transform[] hitboxes;

	IEnumerator GameOver() {
		//player.death ();
		yield return new WaitForSeconds (1.5f);
		StartCoroutine (toTitle ());
	}

	IEnumerator toTitle() {
		float fadeTime = GetComponent<FadeInOut>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(0);
	}

	void spawnItens() {
		foreach (GameObject go in items) {
			Instantiate (go, new Vector3 (Random.Range (-4.5f, 4.5f), Random.Range(-3.5f, .5f)), Quaternion.identity);
		}
	}

	void spawnEnemy() {
		if (Random.Range(0, 2) > 0)
			Instantiate (enemies[0], new Vector3 (8, Random.Range(-5f, 0f)), Quaternion.identity);
		else
			Instantiate (enemies[0], new Vector3 (-8, Random.Range(-5f, 0f)), Quaternion.identity);
	}

	void Start() {
		//hitboxes = GameObject.Find ("Player").GetComponentsInChildren<Transform> (true);
	}

	// Update is called once per frame
	void Update () {
	
		/*
		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.JoystickButton4))
			spawnEnemy ();
		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.JoystickButton5))
			//player.health += 15;
			spawnItens();
		if (Input.GetKeyDown (KeyCode.Alpha3))
			//enemiesInt++;
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			foreach (Transform hb in hitboxes)
				if (hb.name.Equals ("DebugSprite"))
					hb.gameObject.SetActive (!hb.gameObject.activeSelf);
			
		}
		*/

		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.JoystickButton6))
			StartCoroutine (toTitle ());



		if (player.health > player.maxHealth)
			player.health = player.maxHealth;
		if (player.health < 0) {
			player.health = 0;
			if (player.gameObject != null && !player.dead)
				player.death ();
			StartCoroutine (GameOver ());
		}


		healthBar.fillAmount = (float) player.health / (float) player.maxHealth;
		enemiesDefeated.text = enemiesInt.ToString ();
	}
}
