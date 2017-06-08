using UnityEngine;
using System.Collections;

public class Aureola : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer monster;
	//private SpriteRenderer sr;
	private float distance, height;

	void Start () {
		//sr = GetComponent <SpriteRenderer> ();
		distance = transform.localPosition.x;
		height = transform.localPosition.y;
		if (monster.flipX)
			distance = -distance;
	}

	// Update is called once per frame
	void Update () {
		if (monster.flipX)
			transform.localPosition = new Vector2(-distance, height);
		else
			transform.localPosition = new Vector2(distance, height);
	}
}
