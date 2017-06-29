using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour {
	private Player player;
	private Image[] img;

	private Color invisible;
	private Color visible;

	public Sprite[] ItemIcons;
	//public Sprite itemIcon;

	void Start () {
		player = GameObject.Find ("Player").GetComponent<Player>();

		ItemIcons = new Sprite[Player.N_ITEMS];

		Image[] image_aux = GetComponentsInChildren<Image> ();
		img = new Image[Player.N_ITEMS];
		foreach (Image im in image_aux) {
			for (int i = 0; i < Player.N_ITEMS; i++) {
				if (im.gameObject.name.Equals ("Sprite " + i)) {
					img [i] = im;
				}
			}
		}

		invisible = new Color (1f, 1f, 1f, 0f);
		visible = Color.white;
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < Player.N_ITEMS; i++) {
			if (player.item [i] < 0) {
				img[i].color = invisible;
			} else {
				//img.sprite = ItemIcons [player.item];
				img[i].sprite = ItemIcons[i];
				img[i].color = visible;
			}
		}
	}
}
