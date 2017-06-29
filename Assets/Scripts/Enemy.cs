using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public string opponent;

	[HideInInspector]
	public bool paused = false;

	public int maxHealth;
	[HideInInspector]
	public int health;
	public float horizontalSpeed, verticalSpeed;
	[HideInInspector]
	public float moveX, moveY;
    [HideInInspector]
    public Vector2 moveDirection;
    [HideInInspector]
    public float speed;
    public float horizontalRange, verticalRange;

	[SerializeField]
	private GameObject[] drops;
	[SerializeField]
	private float[] dropRates;

	public GameObject target;

	[HideInInspector]
	public bool stunned = false;
	[HideInInspector]
	public float stunDuration = 0;
	[HideInInspector]
	public bool dead = false;

    [SerializeField]
    private GameObject fumaca;
    [SerializeField]
    protected GameObject hurtParticle;

    [HideInInspector]
	public Rigidbody2D rb;
    [HideInInspector]
    public SpriteRenderer sr;
    [HideInInspector]
    protected Animator anim;

    [HideInInspector]
    public bool hitFrom; //false = left; true = right;

    public float maxDistanceRadius;

    public void setFlip(bool direction) {
		sr.flipX = direction;
	}

	protected void findTarget() {
		if (!paused) {
			GameObject[] allPlayers = GameObject.FindGameObjectsWithTag (opponent);
			float dist = 99999;
			foreach (GameObject go in allPlayers) {
				float newDist = Mathf.Sqrt (Mathf.Pow(go.transform.position.x - gameObject.transform.position.x, 2f) + Mathf.Pow(go.transform.position.y - gameObject.transform.position.y, 2f));
				if (newDist < dist) {
					dist = newDist;
					target = go;
				}
			}
		}
	}

	protected virtual void movement () {}

	protected void spawnDrop() {
		float hit = Random.Range (0f, 1f);
		float aux=0;
		for (int i = 0; i < drops.Length; i++) {
			aux += dropRates [i];
			if (aux >= hit) {
				Instantiate (drops[i], gameObject.transform.position, Quaternion.identity);
				break;
			}
		}
	}

	protected void die() {
		GameObject es = GameObject.Find ("EventSystem");
		if (!dead && opponent.Equals("Player")) {
			es.GetComponent<GameManager> ().enemiesInt++;
			es.GetComponent<Spawner> ().enemiesAlive--;
		}
		Instantiate (fumaca, gameObject.transform.position, Quaternion.identity);
		spawnDrop ();
		anim.SetBool ("damaged", false);
		anim.SetBool ("dead", true);
		if (gameObject != null) Destroy (gameObject, anim.GetCurrentAnimatorStateInfo (0).length);
	}

	/*public IEnumerator hurt() {
		bool auxB = false;
		if (health <= 0)
			yield break;
		Vector3 auxV = sr.transform.localScale;
		while (sr.color.g > 0.5f && !auxB) {
			sr.transform.localScale = new Vector3 (sr.transform.localScale.x + .05f, sr.transform.localScale.y + .05f, 1f); 
			sr.color = new Color (1f, sr.color.g - .5f, sr.color.b - .5f, 1f);
			yield return new WaitForSeconds(.2f);
		}
		auxB = true;
		while (sr.color.g < 1f) {
			sr.transform.localScale = new Vector3 (sr.transform.localScale.x - .05f, sr.transform.localScale.y - .05f, 1f); 
			sr.color = new Color (1f, sr.color.g + .5f, sr.color.b + .5f, 1f);
			yield return new WaitForSeconds(.2f);
		}
		sr.transform.localScale = auxV; 
		sr.color = new Color (1f, 1f, 1f, 1f);

		stunned = false;
		yield break;
	}*/

	// Use this for initialization
	void Awake () {

		health = maxHealth;

		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponentInChildren<SpriteRenderer> ();
		anim = GetComponentInChildren<Animator> ();

		findTarget ();

        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().enemies.Add(this);
	}
}
