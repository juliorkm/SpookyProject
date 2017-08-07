using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantasma : Enemy {
    
	[HideInInspector]
    public SpriteRenderer spriteBody;
    private SpriteRenderer spriteHands;
    private Animator animatorBody;

    private float randomFloat;

    [SerializeField]
    private float cooldown;
    private float timer = 0;

	public GameObject fantasmaBola;
	public float velocidadeBola;

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

                if (!((Mathf.Abs (x) < horizontalRange + .001f && Mathf.Abs(x) > horizontalRange - .001f) && (Mathf.Abs (y) < verticalRange + .001f && Mathf.Abs(y) > verticalRange - .001f))) {
                    
                    float dirX, dirY;
                    if (x > -.001f)
                        dirX = target.transform.position.x + horizontalRange - .01f;
                    else if (x < .001f)
                        dirX = target.transform.position.x - horizontalRange + .01f;
                    else
                        dirX = 0f;

                    if (y > -.001f)
                        dirY = target.transform.position.y + verticalRange - .01f;
                    else if (y < .001f)
                        dirY = target.transform.position.y - verticalRange + .01f;
                    else
                        dirY = 0f;

                    moveDirection = new Vector2(dirX, dirY);
                    if (Mathf.Abs(y) > verticalRange && ((moveDirection.x > 0 && x < 0) || (moveDirection.x < 0 && x > 0)))
                        moveDirection = new Vector2(moveDirection.x / 2, moveDirection.y);
                    if (Vector2.Distance(gameObject.transform.position, moveDirection) > .05f) {
                        moveDirection = moveDirection - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                        moveDirection.Normalize();
                        speed = horizontalSpeed;
					    Vector2 direction = (moveDirection + em.avoidFriends (this)).normalized;

    					rb.velocity = direction * speed;
                    }
                    else {
                        rb.velocity = Vector2.zero;
                        transform.position = moveDirection;
                    }
                }
			} else {
				rb.velocity = Vector2.zero;
			}
			if (x < 0)
				sr.flipX = true;
			else if (x > 0)
				sr.flipX = false;
		}
	}

    private void attack() {
		if (!paused && !dead) {
			if (Mathf.Abs(target.transform.position.x - transform.position.x) <= horizontalRange &&
				Mathf.Abs(target.transform.position.y - transform.position.y) <= verticalRange) {
				if (timer == 0 || timer >= cooldown) {
					rb.velocity = Vector2.zero;
					anim.SetBool("atk", true);
					timer = 0;
				} else anim.SetBool("atk", false);
			} else anim.SetBool("atk", false);
			timer += Time.deltaTime;
		}
	}

	private void getHit(AnimatorStateInfo a) {
		if (stunDuration > 0) {
			if (!stunned) {
				anim.SetBool ("damaged", true);
                animatorBody.SetBool("damaged", true);
				anim.SetBool ("atk", false);
                stunned = true;
                if (health > 0 && knockedbackDuration != 0) {
                    StopCoroutine("getKnockedback");
                    StartCoroutine(getKnockedback(knockback, knockedbackDuration));
                }
                else
                    rb.velocity = Vector2.zero;
                hurtParticle.transform.rotation = (hitFrom) ? Quaternion.Euler(0f, 0f, 330f) : Quaternion.Euler(0f, 0f, 150f);
                hurtParticle.Play();
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
			if (a.IsName ("Base Layer.Chasing"))
				if (target != null) {
                    movement ();
                    attack ();
                }

			getHit (a);

			//if (!paused && timer != 0 && timer < cooldown)
			//	timer += Time.deltaTime;

			if (health <= 0) {
				die ();
				dead = true;
			}
		}
    }
}
