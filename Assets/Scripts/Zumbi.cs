using UnityEngine;
using System.Collections;

public class Zumbi : Enemy {


	public int damage;
	[SerializeField]
	private float stunApplied;

	[SerializeField]
	private float maxDistance;

	private GameObject currentHitbox;


	[SerializeField]
	private float cooldown;
	private float timer = 0;
		
	override protected void movement() {
		if (!paused && !dead) {
			float x = gameObject.transform.position.x - target.transform.position.x;
			float y = gameObject.transform.position.y - target.transform.position.y;
			//if (target.name.Equals("Player"))  /*coisa do sprite da arale y += .8f /*****/;
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Chasing")) {
				if (Mathf.Abs (x) < horizontalRange && Mathf.Abs (y) < verticalRange) {
					anim.SetBool ("chase", false);
					rb.velocity = new Vector2 (0f, 0f);
				} else {
					if (Mathf.Abs (x) > horizontalRange) {
						if (x > 0) {
							if (x > 1)
								moveX = -1;
							else
								moveX = -x;
						} else {
							if (x < -1)
								moveX = 1;
							else
								moveX = -x;
						}
					} else
						moveX = 0f;
					if (Mathf.Abs (y) > verticalRange) {
						if (y > 0) {
							//if (y > 1)
							moveY = -1;
							//else
							//	moveY = -y;
						} else {
							//if (y < -1)
							moveY = 1;
							//else
							//	moveY = -y;
						}
					} else
						moveY = 0f;
				}
			} else {
				rb.velocity = new Vector2 (0f, 0f);
				if (Mathf.Abs (x) > horizontalRange || Mathf.Abs (y) > verticalRange)
					anim.SetBool ("chase", true);
			}
			if (rb.velocity.Equals(new Vector2(0, 0))) {
				if (x < 0)
					sr.flipX = true;
				else if (x > 0)
					sr.flipX = false;
			}
		}
	}

	private void attack() {
		if (!paused && !dead) {
			if (timer == 0 || timer >= cooldown) {
				Transform[] hitboxes = GetComponentsInChildren<Transform>(true);
				foreach (Transform hitbox in hitboxes) {
					if (!sr.flipX && hitbox.gameObject.name.Equals ("LeftCollider1"))
						currentHitbox = hitbox.gameObject;
					else if (sr.flipX && hitbox.gameObject.name.Equals ("RightCollider1"))
						currentHitbox = hitbox.gameObject;
				}
				anim.SetBool ("atk", true);
			}
		}
	}

	private void getHit(AnimatorStateInfo a) {
		if (stunDuration > 0) {
			if (!stunned) {
				anim.SetBool ("damaged", true);
				anim.SetBool ("chase", false);
				anim.SetBool ("atk", false);
				stunned = true;
                var particle = Instantiate(hurtParticle, transform);
                particle.transform.rotation = (sr.flipX) ? Quaternion.Euler(0f, 0f, 330f) : Quaternion.Euler(0f, 0f, 150f);
                rb.velocity = Vector2.zero;
			}
			stunDuration -= Time.deltaTime;
		} else if(anim.GetBool("damaged") && a.IsName("Base Layer.ZumbiHitstun")) {
			stunned = false;
			anim.SetBool ("damaged", false);
		}
	}

	public void stopAttacking() {
		timer = Time.deltaTime;
		anim.SetBool ("atk", false);
	}

	public void createHitbox() {
		currentHitbox.SetActive (true);
	}

	public void removeHitbox() {
		currentHitbox.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (!dead) {
			AnimatorStateInfo a = anim.GetCurrentAnimatorStateInfo (0);
			float distance = 0;
			if (target != null) distance = Mathf.Sqrt (Mathf.Pow(target.transform.position.x - gameObject.transform.position.x, 2f) + Mathf.Pow(target.transform.position.y - gameObject.transform.position.y, 2f));
			if (target == null || distance > maxDistance)
				findTarget ();
			if (a.IsName ("Base Layer.ZumbiHurt")) {
				rb.velocity = new Vector2 (0f, 0f);
			} else {
				if (a.IsName ("Base Layer.Chasing") || a.IsName ("Base Layer.Idle"))
					if (target != null) movement ();
				if (a.IsName ("Base Layer.Idle"))
					if (target != null)attack ();
			}

			getHit (a);

			if (!paused && timer != 0 && timer < cooldown)
				timer += Time.deltaTime;

			if (health <= 0) {
				die ();
				dead = true;
			}
		}
	}
		
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag.Equals(opponent + "Hurtbox")) {
			Player player = coll.gameObject.GetComponentInParent<Player> ();
			if (player != null) {
				player.health -= damage;
				player.stunnedFor = stunApplied;
			} else {
				Enemy enemy = coll.gameObject.GetComponentInParent<Enemy> ();
				enemy.health -= damage;
				enemy.stunDuration = stunApplied;
			}
		}
	}
}
