using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantasma : Enemy {
    
    private SpriteRenderer spriteBody;
    private SpriteRenderer spriteHands;
    private Animator animatorBody;

    private float randomFloat;

    [SerializeField]
    private float cooldown;
    private float timer = 0;

    private EnemyManager em;


    void Start () {
        em = FindObjectOfType<EnemyManager>();
        foreach (Animator i in GetComponentsInChildren<Animator>())
            if (i.name.EndsWith("Body")) {
                animatorBody = i;
                break;
            }
        foreach(SpriteRenderer i in GetComponentsInChildren<SpriteRenderer>())
            if (i.name.EndsWith("Body")) spriteBody = i;
            else if (i.name.EndsWith("Hands")) spriteHands = i;
        randomFloat = Random.Range(0f, Mathf.PI);
    }

	override protected void movement() {
		if (!paused && !dead) {
			float x = gameObject.transform.position.x - target.transform.position.x;
			float y = gameObject.transform.position.y - target.transform.position.y;
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Chasing")) {
				if (Mathf.Abs (x) < horizontalRange && Mathf.Abs (y) < verticalRange) {
					anim.SetBool ("chase", false);
					rb.velocity = Vector2.zero;
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
					} else {
                        Debug.Log(x);
						if (x > 0) {
							if (x > 1)
								moveX = 1;
							else
								moveX = x;
						} else {
							if (x < -1)
								moveX = -1;
							else
								moveX = x;
						}
                    }
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

                    Debug.Log("m" + moveX);
                    moveDirection = new Vector2(moveX * horizontalSpeed, moveY * verticalSpeed);
                    speed = moveDirection.magnitude;
                    moveDirection.Normalize();

                    Vector2 direction = (moveDirection + em.avoidFriends(this)).normalized;
                    x = direction.x;
                    y = direction.y;
                    if (Mathf.Abs(direction.x * speed) > horizontalSpeed)
                        x = Mathf.Sign(direction.x * speed) * horizontalSpeed / speed;
                    if (Mathf.Abs(direction.y * speed) > verticalSpeed)
                        y = Mathf.Sign(direction.y * speed) * verticalSpeed / speed;
                    if ((direction.x > 0 && !sr.flipX) || (direction.x < 0 && sr.flipX))
                        x = 0f;

                    rb.velocity = new Vector2(x, y) * speed;
                }
			} else {
				rb.velocity = Vector2.zero;
				if (Mathf.Abs (x) > horizontalRange || Mathf.Abs (y) > verticalRange)
					anim.SetBool ("chase", true);
			}
			if (rb.velocity.Equals(Vector2.zero)) {
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
				//anim.SetBool ("atk", true);
			}
		}
	}

	private void getHit(AnimatorStateInfo a) {
		if (stunDuration > 0) {
			if (!stunned) {
				anim.SetBool ("damaged", true);
				anim.SetBool ("chase", false);
				anim.SetBool ("atk", false);
                animatorBody.SetBool("damaged", true);
                stunned = true;
                if (health > 0 && knockedbackDuration != 0) {
                    StopCoroutine("getKnockedback");
                    StartCoroutine(getKnockedback(knockback, knockedbackDuration));
                }
                else
                    rb.velocity = Vector2.zero;
                var particle = Instantiate(hurtParticle, transform);
                particle.transform.rotation = (hitFrom) ? Quaternion.Euler(0f, 0f, 330f) : Quaternion.Euler(0f, 0f, 150f);
            }
			stunDuration -= Time.deltaTime;
		} else if(anim.GetBool("damaged") && a.IsName("Base Layer.Hurt")) {
			stunned = false;
			anim.SetBool ("damaged", false);
            animatorBody.SetBool("damaged", false);
        }
	}

    void Update () {
        spriteBody.transform.localPosition = new Vector2(0, Mathf.Sin(Time.time * 2 + randomFloat) * .2f);
        spriteHands.transform.localPosition = new Vector2(0, Mathf.Sin(Time.time * 2 + randomFloat) * .2f);

        spriteBody.sortingOrder = sr.sortingOrder;
        spriteHands.sortingOrder = sr.sortingOrder + 1;

        spriteBody.flipX = sr.flipX;
        spriteHands.flipX = sr.flipX;
        
		if (!dead) {
			AnimatorStateInfo a = anim.GetCurrentAnimatorStateInfo (0);
			float distance = 0;
			if (target != null) distance = Mathf.Sqrt (Mathf.Pow(target.transform.position.x - gameObject.transform.position.x, 2f) + Mathf.Pow(target.transform.position.y - gameObject.transform.position.y, 2f));
			if (target == null || distance > maxDistance)
				findTarget ();
			if (!a.IsName ("Base Layer.Hurt")) {
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
}
