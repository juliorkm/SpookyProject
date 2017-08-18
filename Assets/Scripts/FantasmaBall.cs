using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasmaBall : MonoBehaviour {
	private SpriteRenderer sr;
	[HideInInspector]
	public SpriteRenderer ballSprite;
	private Rigidbody2D rb;
	private AudioSource aS;

	[SerializeField]
	private string opponent;

	[SerializeField]
	private float damage, stunApplied;
	//[HideInInspector]
	public float speed = 0;

	[SerializeField]
	private float range;
	private float height;
	private float verticalSpeed;
	private float acceleration = 0;

	private Vector2 initPosition, finalPosition;
	private bool finalPositionSetted = false, isDed = false;


	[SerializeField]
	private AudioClip[] sounds;

	[SerializeField]
	private GameObject particle;

	// Use this for initialization
	void Awake () {
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		aS = GetComponent<AudioSource>();
		var srs = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer s in srs) {
			if (s.name == "BallProjectile") ballSprite = s;
		}
		initPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		rotate();
		movement();
		ballSprite.sortingOrder = sr.sortingOrder;
		if (!finalPositionSetted && speed != 0) {
			finalPosition = new Vector2(initPosition.x + ((speed > 0) ? range : -range), initPosition.y);
			height = ballSprite.transform.localPosition.y;
			finalPositionSetted = true;
		}
	}

	public void setAcceleration() {
		acceleration = - 2 * ballSprite.transform.localPosition.y * speed * speed / (range * range);
	}

	void rotate() {
		ballSprite.transform.rotation = Quaternion.Euler(new Vector3(ballSprite.transform.rotation.eulerAngles.x,
			ballSprite.transform.rotation.eulerAngles.y,
			ballSprite.transform.rotation.eulerAngles.z + Time.deltaTime * 1000 * - Mathf.Sign(speed)));
	}

	void movement() {
		if (isDed) return;
		rb.velocity = new Vector2(speed, 0f);
		if (finalPositionSetted) {
			var t = (transform.position.x - initPosition.x) / (finalPosition.x - initPosition.x);
			var y = height - Mathf.Pow(t, 2) * height;
			ballSprite.transform.localPosition = new Vector2(ballSprite.transform.localPosition.x, 
				y);
			if (ballSprite.transform.localPosition.y < 0) StartCoroutine(die());
		}
	}

	IEnumerator die() {
		isDed = true;
		sr.color = new Color(1f, 1f, 1f, 0f);
		ballSprite.color = new Color(1f, 1f, 1f, 0f);
		GetComponent<Collider2D>().enabled = false;
		Instantiate(particle, ballSprite.transform);
		ballSprite.GetComponent<TrailRenderer>().enabled = false;
		var ps = particle.GetComponent<ParticleSystem>();
		yield return new WaitForSeconds(ps.main.startLifetime.constant + ps.main.duration);
		if (gameObject != null) Destroy(gameObject);

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag.Equals(opponent + "Hurtbox")) {
			Player player = coll.gameObject.GetComponentInParent<Player>();
			if (player != null) {
				if (!player.dead) {
					aS.PlayOneShot(sounds[0]);
					player.health -= damage;
					player.stunnedFor = stunApplied;
					player.hitFrom = (transform.position.x < player.transform.position.x) ? false : true;
				}
			} else {
				aS.PlayOneShot(sounds[0]);
				Enemy enemy = coll.gameObject.GetComponentInParent<Enemy>();
				enemy.health -= damage;
				enemy.stunDuration = stunApplied;
				enemy.hitFrom = (transform.position.x < enemy.transform.position.x) ? false : true;
			}
			StartCoroutine(die());
		}
	}
}
