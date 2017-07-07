using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class TitleScreen : MonoBehaviour {

	[SerializeField]
	private Image[] spriteOff;
	[SerializeField]
	private GameObject[] spriteOn;

    public Image sign;
    private Quaternion signAngle;
    public Image logo;
    [SerializeField]
    TextMeshProUGUI description;

    private Color notSelected;
    private Vector2 logoVanish;

    public int position = 1;
	private bool inputStuff = true;

    private bool canMove = true;

	public void selectButton(int x) {
		//position = x;
        //spriteOff [x].transform.localScale = new Vector3 (1.2f, 1.2f, 1f);
		for (int i = 0; i < spriteOn.Length; i++)
            if (i != x) {
                spriteOn[i].gameObject.SetActive(false);
                spriteOff[i].color = notSelected;
            }
        spriteOn[x].gameObject.SetActive(true);
        spriteOff[x].color = Color.white;
        switch (x) {
            case 0:
                description.text = "Config";
                break;
            case 1:
                description.text = "Jogar";
                break;
            case 2:
                description.text = "Sair";
                break;
            default:
                break;
        }
    }

	public void clickButton() {
		//PlayAudio(1);
		if (position == 0) {
            //TODO Config Scene
        }
		else if (position == 1)
            StartCoroutine(StartGame());
        else if (position == 2)
            StartCoroutine(Exit());
    }

	IEnumerator StartGame () {
        canMove = false;
        while (logo.transform.localScale.y > .1) {
            logo.transform.localScale = Vector2.Lerp(logo.transform.localScale, logoVanish, .5f);
            yield return new WaitForSeconds(.02f);
        }
        while (90 - sign.transform.eulerAngles.x > .1) {
            logo.transform.localScale = Vector2.Lerp(logo.transform.localScale, logoVanish, .5f);
            sign.transform.localRotation = Quaternion.Lerp(sign.transform.localRotation, signAngle, .1f);
            yield return new WaitForSeconds(.02f);
        }
        Destroy(sign.gameObject);
        Destroy(logo.gameObject);
        StartCoroutine(GetComponent<StartAnimation>().StartingAnimation());
    }

	IEnumerator Exit () {
		float fadeTime = GetComponent<FadeInOut>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.Quit();
	}
	
    void Start() {
        signAngle = Quaternion.Euler(90f, 0f, 0f);
        notSelected = new Color(.8f, .8f, .8f, .8f);
        logoVanish = new Vector2(2f, 0f);
    }

	// Update is called once per frame
	void Update () {
	    if (canMove) {
		    if (Input.GetAxisRaw("Horizontal") == 1)
			    if (inputStuff) {
				    //PlayAudio(0);
				    position = (position + 1) % spriteOff.Length;
				    inputStuff = false;
			    }

		    if (Input.GetAxisRaw("Horizontal") == -1)
			    if (inputStuff) {
				    //PlayAudio(0);
				    if (position == 0)
					    position = spriteOff.Length - 1;
				    else
					    position = (position - 1) % spriteOff.Length;
				    inputStuff = false;
			    }

		    if (Input.GetAxisRaw("Horizontal") == 0)
			    inputStuff = true;

		    if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.JoystickButton0) || Input.GetKeyDown (KeyCode.JoystickButton7))
			    clickButton ();

		    selectButton (position);
        }
	}
}
