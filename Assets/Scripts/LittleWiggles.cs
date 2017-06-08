using UnityEngine;
using System.Collections;

public class LittleWiggles : MonoBehaviour {

	private bool b = false;
	private float x, y, z;

	void Start () {
		z = transform.position.z;
	}

	void Update () {

		x = transform.position.x;
		y = transform.position.y;

		if (b)
			transform.position = new Vector3 (x, y + 0.0001f, z);
		else
			transform.position = new Vector3 (x, y - 0.0001f, z);
		b = !b;

	}
}
