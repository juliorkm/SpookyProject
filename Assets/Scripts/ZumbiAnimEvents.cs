using UnityEngine;
using System.Collections;

public class ZumbiAnimEvents : MonoBehaviour {

	private Zumbi zumbi;

	public void addSpeed() {
		float x = zumbi.moveX * zumbi.horizontalSpeed;
		float y = zumbi.moveY * zumbi.verticalSpeed;
		zumbi.rb.velocity = new Vector2 (x, y);
	}

	public void stopSpeed() {
		zumbi.rb.velocity = new Vector2 (0f, 0f);
	}
		
	public void stopAttacking() {
		zumbi.stopAttacking ();
	}

	public void createHitbox() {
		zumbi.createHitbox();
	}

	public void removeHitbox() {
		zumbi.removeHitbox();
	}

	// Use this for initialization
	void Start () {
		zumbi = GetComponentsInParent<Zumbi> () [0];
	}
}
