using UnityEngine;
using System.Collections;

public class ItemPickUp : MonoBehaviour {
    [SerializeField]
    private ItemType itemID;
	private int itemSprite;

	[SerializeField]
	private float sizeVariation;
	[SerializeField]
	private float variationSpeed;
	[SerializeField]
	private float rotationAngle; 

	[SerializeField]
	private int healAmount;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private AudioClip ac;

    private CircleCollider2D cc;
	private SpriteRenderer sr;
	private float scale;

	private Vector2 spawnLocation;
    private float timeUntilDespawn = 7f;
    
    enum ItemType
    {
        PIRULITO, COOKIE, BOMBOM, BARRA_DE_CHOCOLATE, ABOBORA, ZUMBI, ZUMBI3
    }

    // Use this for initialization
    void Start () {
		cc = GetComponentInChildren<CircleCollider2D> ();
		cc.enabled = false;
		spawnLocation = transform.position;
		transform.position = new Vector2 (transform.position.x, transform.position.y + 1);

		sr = GetComponentInChildren<SpriteRenderer> ();
		scale = sr.transform.localScale.x;

		itemSprite = Random.Range (0, sprites.Length);
		sr.sprite = sprites[itemSprite];

		sr.transform.Rotate (0f, 0f, Random.Range(-rotationAngle,rotationAngle));

	}

	void spawnAnimation() {
		transform.position = new Vector2 (transform.position.x, transform.position.y - 2.5f *Time.deltaTime);
	}
	void idleAnimation() {
		sr.transform.localScale = new Vector3 (scale*(1 + sizeVariation * Mathf.Sin (Time.time*variationSpeed)), scale*(1 + sizeVariation * Mathf.Sin (Time.time*variationSpeed)), 1);
	}

    IEnumerator fadeUntilDestroy() {
        float duracao = 2f;
        while (sr.color.a > 0) {
            sr.color = new Color(1f, 1f, 1f, sr.color.a - .02f);
            yield return new WaitForSeconds(duracao * .02f);
        }
        if (gameObject != null) Destroy(gameObject);
    }

    void despawnItem() {
        if (timeUntilDespawn > 0) timeUntilDespawn -= Time.deltaTime;
        else {
            StartCoroutine(fadeUntilDestroy());
            timeUntilDespawn = 90;
        }
    }

	// Update is called once per frame
	void Update () {
		if (!cc.enabled) {
			if (transform.position.y > spawnLocation.y)
				spawnAnimation ();
			else {
				cc.enabled = true;
				idleAnimation ();
			}
		} else {
			idleAnimation ();
            despawnItem();
        }
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.gameObject.tag.Equals("PlayerHurtbox")) {
			Player player = coll.gameObject.GetComponentInParent<Player> ();
			if (player != null) {

				if ((int) itemID <= 3) {
					if (player.health < player.maxHealth) {
                        player.aS.PlayOneShot(ac);
                        player.health += healAmount;
                        player.healParticle.Play();
						if (gameObject != null) Destroy (gameObject);
					}
				} else {

                    //BOTOES SEPARADOS OU FILA
                    ///*
                    for (int i = 0; i < Player.N_ITEMS; i++) {
						if (player.item [i] == -1) {
                            player.aS.PlayOneShot(ac);
                            player.item [i] = (int) itemID;
							GameObject.Find ("ItemManager").GetComponent<ItemDisplay> ().ItemIcons [i] = sr.sprite;
							if (gameObject != null) Destroy (gameObject);
							break;
						}
					}
                    //*/

                    //PILHA
                    /*
				if (player.item [0] > -1) {
					for (int i = 1; i < Player.N_ITEMS; i++) {
						if (player.item [i] == -1) {
                            player.aS.PlayOneShot(ac);
							for (int j = i; j > 0; j--) {
								player.item [j] = player.item [j - 1];
							}
							player.item [0] = -1;
							break;
						}
					}
				}

				if (player.item [0] == -1) {
                    player.aS.PlayOneShot(ac);
					ItemDisplay iD = GameObject.Find ("ItemIcon").GetComponent<ItemDisplay> ();
					player.item [0] = itemID;
					for (int i = Player.N_ITEMS - 1; i > 0; i--)
						iD.ItemIcons [i] = iD.ItemIcons[i-1];
					iD.ItemIcons [0] = sr.sprite;
					Destroy (gameObject);
				}
				*/
                }
            }
		}
	}
}
