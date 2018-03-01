using UnityEngine;
using System.Collections;

public class ItemEffects : MonoBehaviour {
	public GameObject[] Fumacas;
    public AudioClip[] Sounds;
    public int PirulitoHeal;
	public int CookieHeal;
	public int BombomHeal;
	public int BarraHeal;
	public int AboboraHeal;
	public GameObject Zumbi;
	private Player player;

	public void useItem(int pos) {
		if (player.item [pos] == -1) {
			//nao tem item
		} else if (player.item [pos] == 0) {
			//pirulito
			player.health += PirulitoHeal;
            player.aS.PlayOneShot(Sounds[1]);
        } else if (player.item [pos] == 1) {
			//cookie
			player.health += CookieHeal;
            player.aS.PlayOneShot(Sounds[1]);
        } else if (player.item [pos] == 2) {
			//bombom
			player.health += BombomHeal;
            player.aS.PlayOneShot(Sounds[1]);
        } else if (player.item [pos] == 3) {
			//barra de chocolate
			player.health += BarraHeal;
            player.aS.PlayOneShot(Sounds[1]);
        } else if (player.item [pos] == 4) {
			//abobora
			player.health += AboboraHeal;
            player.aS.PlayOneShot(Sounds[0]);
            var v = Instantiate(Fumacas[1], player.transform.position, Quaternion.identity);
        } else if (player.item [pos] == 5) {
			//zumbi
			int d;
			if (player.direction)
				d = 1;
			else
				d = -1;
			Vector3 v = new Vector2 (player.transform.position.x + d, player.transform.position.y);
			Instantiate (Fumacas[0], v, Quaternion.identity);
			GameObject zumb = (GameObject) Instantiate(Zumbi, v, Quaternion.identity);
			zumb.GetComponent<Zumbi> ().setFlip (player.direction);
		} else if (player.item [pos] == 6) {
			//zumbi3
			int d;
			if (player.direction)
				d = 1;
			else
				d = -1;
			Vector3 v = new Vector2 (player.transform.position.x + d, player.transform.position.y);
			Instantiate (Fumacas[0], v, Quaternion.identity);
			GameObject zumb = (GameObject) Instantiate(Zumbi, v, Quaternion.identity);
			zumb.GetComponent<Zumbi> ().setFlip (player.direction);

            v = new Vector2(player.transform.position.x + d, player.transform.position.y - .7f);
            Instantiate(Fumacas[0], v, Quaternion.identity);
            zumb = (GameObject)Instantiate(Zumbi, v, Quaternion.identity);
            zumb.GetComponent<Zumbi>().setFlip(player.direction);

            v = new Vector2(player.transform.position.x + d, player.transform.position.y + .7f);
            Instantiate(Fumacas[0], v, Quaternion.identity);
            zumb = (GameObject)Instantiate(Zumbi, v, Quaternion.identity);
            zumb.GetComponent<Zumbi>().setFlip(player.direction);

        }
		player.item[pos] = -1;
	}

	// Use this for initialization
	void Start () {
		player = gameObject.GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
