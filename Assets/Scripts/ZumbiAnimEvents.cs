using UnityEngine;
using System.Collections;

public class ZumbiAnimEvents : MonoBehaviour {

	private Zumbi zumbi;

	public void addSpeed() {
        EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        Vector2 direction = (zumbi.moveDirection + em.avoidFriends(zumbi)).normalized;
        float x = direction.x;
        float y = direction.y;
        if (Mathf.Abs(direction.x * zumbi.speed) > zumbi.horizontalSpeed)
            x = Mathf.Sign(direction.x * zumbi.speed) * zumbi.horizontalSpeed / zumbi.speed;
        if (Mathf.Abs(direction.y * zumbi.speed) > zumbi.verticalSpeed)
            y = Mathf.Sign(direction.y * zumbi.speed) * zumbi.verticalSpeed / zumbi.speed;
        if ((direction.x > 0 && !zumbi.sr.flipX) || (direction.x < 0 && zumbi.sr.flipX))
            x = 0f;



        zumbi.rb.velocity = new Vector2(x,y) * zumbi.speed;
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
