﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public Image healthBar;
	public Player player;
	public GameObject[] items;
	public GameObject[] enemies;

	public int score = 0;
    public int hiscore;

    private bool passedHiScore = false;

    //private CameraBehaviors cb;

    //private Transform[] hitboxes;

    IEnumerator GameOver() {
        //player.death ();
        PlayerPrefs.SetInt("score", score);
		yield return new WaitForSeconds (1.5f);
		StartCoroutine (ToPoints());
	}

    IEnumerator ToTitle() {
        float fadeTime = FindObjectOfType<FadeInOut>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(1);
    }

    IEnumerator ToPoints() {
        float fadeTime = FindObjectOfType<FadeInOut>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(2);
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

	void Awake() {
        //hitboxes = GameObject.Find ("Player").GetComponentsInChildren<Transform> (true);
        player = GameObject.FindObjectOfType<Player>();

        //cb = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviors>();
        if (PlayerPrefs.HasKey("hiscore")) hiscore = PlayerPrefs.GetInt("hiscore");
    }

	// Update is called once per frame
	void Update () {
	
		
		//if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.JoystickButton4))
        //    cb.pulseCamera(.7f, .2f);
        /*
		if (Input.GetKeyDown (KeyCode.Space))
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

		if (Input.GetKeyDown (KeyCode.Escape))
			StartCoroutine (ToTitle());



		if (player.health > player.maxHealth)
			player.health = player.maxHealth;
		if (player.health <= 0) {
			player.health = 0;
            try {
			    if (player.gameObject != null && !player.dead)
				    player.death ();
			    StartCoroutine (GameOver ());
            }
            catch (System.Exception e) { }
		}

        //orelhinha orelhao master race
        float fill = ((float)player.health / (float)player.maxHealth)*.71f + .19f;
		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, fill, .3f);

        if (!passedHiScore && score > hiscore) {
            scoreText.enableVertexGradient = true;
            Color yellow = new Color(1f, .882f, .212f);
            scoreText.colorGradient = new VertexGradient(yellow, yellow, new Color(1f, .647f, 0f), Color.white);
            passedHiScore = true;
        }
        scoreText.text = score.ToString ();
        hiscoreText.text = hiscore.ToString();
	}
}
