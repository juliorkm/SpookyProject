using UnityEngine;
using System.Collections;

public class OrderInLayer : MonoBehaviour {

	private SpriteRenderer sr;
	private bool useHeight;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		Player p = GetComponentInParent<Player> ();
		ItemPickUp i = GetComponentInParent<ItemPickUp> ();
		if (p == null && i == null)
			useHeight = false;
		else
			useHeight = true;
	}

	void Update () {
		if (useHeight) {
			sr.sortingOrder = -(int)((transform.position.y + sr.sprite.bounds.min.y * transform.localScale.y) * 100);
			//Debug.Log (sr.sprite.bounds.min.y * transform.localScale.y + gameObject.name);
		}
		else
			sr.sortingOrder = - (int) ((transform.position.y) * 100);
	}
}
