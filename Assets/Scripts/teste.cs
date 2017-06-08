using UnityEngine;
using System.Collections;

public class teste : MonoBehaviour {
	Rigidbody2D rb;
	SpriteRenderer sr;
	public float x;
	// Use this for initialization
	void Start () {
		this.rb = GetComponent<Rigidbody2D> ();
		this.sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.J))
			rb.velocity = new Vector2 (x, 0);
		else
			rb.velocity = new Vector2 (0, 0);
		if (x > 0)
			this.sr.flipX = true;
		else
			this.sr.flipX = false;
		if (Input.GetKeyDown (KeyCode.K))
			x= -x;
	}
}
