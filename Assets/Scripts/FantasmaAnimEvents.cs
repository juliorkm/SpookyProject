using UnityEngine;
using System.Collections;

public class FantasmaAnimEvents : MonoBehaviour {

	private Fantasma fantasma;

	public void throwBall() {
		var bola = Instantiate(fantasma.fantasmaBola, fantasma.transform.position, Quaternion.identity).GetComponent<FantasmaBall>();
		bola.ballSprite.transform.localPosition = new Vector2(0f, bola.ballSprite.transform.localPosition.y + fantasma.spriteBody.transform.localPosition.y);
		bola.speed = (fantasma.spriteBody.flipX) ? fantasma.velocidadeBola : - fantasma.velocidadeBola;
		bola.setAcceleration();
	}
	
	void Start() {
		fantasma = GetComponentsInParent<Fantasma>()[0];
	}
}
