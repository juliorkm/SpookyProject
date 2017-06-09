using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[HideInInspector]
	public static int N_ITEMS = 3;

	[HideInInspector]
	public bool paused = false;

	public int maxHealth;
	[HideInInspector]
	public int health;
	public float horizontalSpeed, verticalSpeed;
	public float[] cooldowns;
	public int[] item;

	public int[] damage;
	public float[] stunDurations;

	[SerializeField]
	private GameObject playerFumaca;

	private float attack1CD;

	//public AnimationClip attackAnim;

	private ItemEffects itemeff;

	[HideInInspector]
	public int currentDamage;
	[HideInInspector]
	public float currentStun;

	//[HideInInspector]
	public bool direction = false;
	//true = DIREITA; false = ESQUERDA
	//private bool canMove = true;
	private bool canAttack = true;

	private int attackCounter = 0;

	[HideInInspector]
	public bool hitAnEnemy = false;
	[HideInInspector]
	public bool stunned = false;
	[HideInInspector]
	public float stunnedFor = 0;
	[HideInInspector]
	public bool dead = false;

	private GameObject currentHitbox;

	[HideInInspector]
	public Rigidbody2D rb;
	private SpriteRenderer sr;
	private Animator anim;
    private CameraBehaviors cb;


	public void death() {
		dead = true;
		Instantiate (playerFumaca, gameObject.transform.position, Quaternion.identity);
		anim.SetBool ("Damaged", false);
		anim.SetBool ("Dead", true);
		if (gameObject != null) Destroy (gameObject, anim.GetCurrentAnimatorStateInfo (0).length);

	}

	private void playerMovement() {
		/*
		if (Input.GetAxisRaw ("Horizontal") > 0 && direction == false) {
			transform.rotation = new Quaternion (transform.rotation.x, 0f, transform.rotation.z, transform.rotation.w);
			direction = true;
		}
		if (Input.GetAxisRaw ("Horizontal") < 0 && direction == true) {
			transform.rotation = new Quaternion (transform.rotation.x, 180f, transform.rotation.z, transform.rotation.w);
			direction = false;
		}
		*/
		if (!paused) {
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("BaseLayer.PlayerIdle")) {
				if (Input.GetAxisRaw ("Horizontal") > 0 && direction == false) {
					sr.flipX = true;
					direction = true;
				}
				if (Input.GetAxisRaw ("Horizontal") < 0 && direction == true) {
					sr.flipX = false;
					direction = false;
				}

				float horizontal = Input.GetAxisRaw ("Horizontal");
				float vertical = Input.GetAxisRaw ("Vertical");
				rb.velocity = new Vector3 (horizontal * horizontalSpeed, vertical * verticalSpeed, 0f);

				if (horizontal == 0 && vertical == 0)
					anim.SetFloat ("Moving", 0);
				else
					anim.SetFloat ("Moving", 1);
			}
			else 
				rb.velocity = new Vector3 (0f, 0f, 0f);
		}
	}

	private void useItem() {
		if (!paused) {
			//UM ITEM
			/*
			if (Input.GetKeyDown (KeyCode.X)) {
				itemeff.useItem (0);
			}
			*/

			//TRES ITEMS
			/*
			if (Input.GetKeyDown (KeyCode.X)) {
				itemeff.useItem (0);
			}
			if (Input.GetKeyDown (KeyCode.C)) {
				itemeff.useItem (1);
			}
			if (Input.GetKeyDown (KeyCode.V)) {
				itemeff.useItem (2);
			}
			*/

			//FILA/PILHA
			///*
			if (Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.JoystickButton3)) {
				ItemDisplay iD = GameObject.Find ("ItemIcon").GetComponent<ItemDisplay> ();
				itemeff.useItem (0);
				for (int i = 0; i < N_ITEMS - 1; i++) {
					item [i] = item [i + 1];
					iD.ItemIcons [i] = iD.ItemIcons [i + 1];
				}
				item [N_ITEMS - 1] = -1;
			}
			//*/
		}
	}

	private void quickAttack() {
		if (!paused) {
			if (attack1CD < cooldowns [0])
				attack1CD += Time.deltaTime;

			if (attackCounter < 3 && (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.JoystickButton2))) {
				if (canAttack) {
					rb.velocity = new Vector3 (0f, 0f, 0f);
					Transform[] hitboxes = GetComponentsInChildren<Transform>(true);
					if ((Input.GetAxisRaw ("Horizontal") != -1 && direction) || (Input.GetAxisRaw ("Horizontal") != 1 && !direction)) {
						if (anim.GetBool ("Attack3")) {
							//---
						} else if (anim.GetBool ("Attack2")) {
							foreach (Transform hitbox in hitboxes) {
								if ((direction && hitbox.gameObject.name.Equals ("RightCollider1")) ||
								    !direction && hitbox.gameObject.name.Equals ("LeftCollider1")) {
									currentHitbox = hitbox.gameObject;
								}
							}
							anim.SetBool ("Attack3", true);
						} else if (anim.GetBool ("Attack1")) {
							foreach (Transform hitbox in hitboxes) {
								if ((direction && hitbox.gameObject.name.Equals ("RightCollider1")) ||
								    !direction && hitbox.gameObject.name.Equals ("LeftCollider1")) {
									currentHitbox = hitbox.gameObject;
								}
							}
							anim.SetBool ("Attack2", true);
						}
					}

					if (attack1CD >= cooldowns[0]) {
						foreach (Transform hitbox in hitboxes) {
							if ((direction && hitbox.gameObject.name.Equals ("RightCollider1")) ||
								!direction && hitbox.gameObject.name.Equals ("LeftCollider1")) {
								currentHitbox = hitbox.gameObject;
							}
						}
						anim.SetBool ("Attack1", true);
						attackCounter = 1;

						rb.velocity = new Vector3 (0f, 0f, 0f);
						//canMove = false;
						attack1CD = 0;
					}
				}
			}
		}
	}

	private void getHit(AnimatorStateInfo a) {
		if (stunnedFor > 0) {
			if (!stunned) {
				anim.SetBool ("Damaged", true);
				anim.SetBool ("Attack1", false);
				anim.SetBool ("Attack2", false);
				anim.SetBool ("Attack3", false);
				anim.SetBool ("toAttack2", false);
				anim.SetBool ("toAttack3", false);
				stunned = true;
                //cb.pulseCamera(.7f,.2f);
				rb.velocity = Vector2.zero;
			}
			stunnedFor -= Time.deltaTime;
		} else if(anim.GetBool("Damaged") && a.IsName("BaseLayer.Hurt")) {
			stunned = false;
			anim.SetBool ("Damaged", false);
		}
	}

	public void enableMovement() {
		//canMove = true;
	}

	public void stopAttacking(int attack) {
		if (attack >= 3 || !anim.GetBool ("Attack" + (attack + 1)) || !hitAnEnemy) {
			enableMovement ();
			for (int i = 1; i <= 3; i++) {
				anim.SetBool ("Attack" + i, false);
				if (i < 3) anim.SetBool ("toAttack" + (i + 1), false);
			}
		} else {
			anim.SetBool ("Attack" + attack, false);
			if (attack < 3) anim.SetBool ("toAttack" + (attack + 1), true);
		}
		canAttack = false;
		hitAnEnemy = false;
	}

	public void createHitbox(int attack) {
		currentHitbox.SetActive (true);
		currentDamage = damage[attack];
		currentStun = stunDurations[attack];
	}

	public void removeHitbox() {
		Transform[] hitboxes = GetComponentsInChildren<Transform>(true);
		foreach (Transform hitbox in hitboxes) {
			if (hitbox.name.StartsWith ("LeftCollider") || hitbox.name.StartsWith ("RightCollider"))
				hitbox.gameObject.SetActive (false);
		}
	}

	// Use this for initialization
	void Start () {
	
		health = maxHealth;

		rb = GetComponentInChildren<Rigidbody2D> ();
		sr = GetComponentInChildren<SpriteRenderer> ();
		anim = GetComponentInChildren<Animator> ();
        cb = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviors>();

		item = new int[N_ITEMS];
		for (int i = 0; i < N_ITEMS; i++) item[i] = -1;

		itemeff = GetComponent<ItemEffects> ();

		//cooldowns [0] = attackAnim.length;
		attack1CD = cooldowns [0];

		Transform[] hitboxes = GetComponentsInChildren<Transform>(true);
		foreach (Transform hitbox in hitboxes) {
			if ((direction && hitbox.gameObject.name.Equals ("RightCollider1")) ||
				!direction && hitbox.gameObject.name.Equals ("LeftCollider1")) {
				currentHitbox = hitbox.gameObject;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo a = anim.GetCurrentAnimatorStateInfo (0);
		if (a.IsName ("BaseLayer.Hurt")) {
			rb.velocity = new Vector2 (0f, 0f);
		} else {
			playerMovement ();
			quickAttack ();
			canAttack = true;
		}
		useItem ();
		/*
		if (stunned) {
			anim.SetBool ("Damaged", true);
			anim.SetBool ("Attack1", false);
			anim.SetBool ("Attack2", false);
			anim.SetBool ("Attack3", false);
			anim.SetBool ("toAttack2", false);
			anim.SetBool ("toAttack3", false);
			stunned = false;
			canMove = true;
		} else {
			anim.SetBool ("Damaged", false);
		}*/
		getHit (a);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag.Equals("EnemyHurtbox")) {
			Enemy enemy = coll.GetComponentInParent<Enemy> ();
			hitAnEnemy = true;
			enemy.stunDuration = currentStun;
			enemy.health -= currentDamage;
            enemy.hitFrom = (transform.position.x < enemy.transform.position.x) ? false : true;
			//StartCoroutine (enemy.hurt ());
			//enemy.stunned = true;
		}
	}
}
