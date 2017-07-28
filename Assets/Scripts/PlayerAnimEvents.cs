using UnityEngine;
using System.Collections;

public class PlayerAnimEvents : MonoBehaviour {
	//GameObject playerGO;
	private Player player;

	//private SpriteRenderer sr;

	//Usar so porque as animacoes mexem com pivot
	/*public void pivotChange(int position) {
		if (position == 0) {
			GameObject aux = new GameObject ();
			aux.transform.position = new Vector2 (playerGO.transform.position.x, playerGO.transform.position.y + .5f * sr.sprite.pixelsPerUnit);
			playerGO.transform.parent = aux.transform.parent;
		}
		else if (position =
	}*/

    public void playSound(int i) {
        player.playSound(i);
    }

	public void stopAttacking(int attack) {
		player.stopAttacking (attack);
	}

	public void createHitbox(int attack) {
		player.createHitbox(attack);
	}

	public void removeHitbox() {
		player.removeHitbox ();
	}

	void Start () {
		player = GetComponentsInParent<Player> () [0];
		//sr = GetComponentsInChildren <SpriteRenderer> () [0];
		//playerGO = player.gameObject;
	}
}
