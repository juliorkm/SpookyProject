using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Globalization;

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

    [SerializeField]
    private GameObject optionsSign;
    [SerializeField]
    private TextMeshProUGUI[] optionsTexts;
    [SerializeField]
    private TextMeshProUGUI[] optionsVarTexts;
    private ImagePulse[] optionsTextPulses;
    [SerializeField]
    private Slider[] optionsSliders;
    [SerializeField]
    private Image[] handles;
    [SerializeField]
    private Sprite[] handleOptions;

    [SerializeField]
    private AudioClip[] sounds;
    private AudioSource aS;

    private string attackString, itemString;
    private string attackStringJ, itemStringJ;

    private Color notSelected;
    private Vector2 logoVanish;
    private Color saveColor, cancelColor;

    private int position = 1;
    private int optionsPosition = 0;
    private bool inputStuff = true;

    private bool mainMenu = true;
    private bool optionsMenu = false;

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

    public void selectOptionsButton(int x) {
        for (int i = 0; i < optionsTexts.Length; i++) {
            if (i != x) {
                optionsTextPulses[i].enabled = false;
                optionsTexts[i].color = new Color(1f, 1f, 1f, .2f);
            }
        }

        optionsTexts[2].text = "Som ambiente: " + optionsSliders[0].value * 10 + "%";
        optionsTexts[3].text = "Efeitos sonoros: " + optionsSliders[1].value * 10 + "%";

        if (x == 2)
            handles[0].rectTransform.localScale = new Vector2(1 * (1 + .1f * Mathf.Sin(Time.time * 2.5f)), 1 * (1 + .1f * Mathf.Sin(Time.time * 2.5f)));
        else
            handles[0].rectTransform.localScale = Vector2.one;
        
        if (x == 3)
            handles[1].rectTransform.localScale = new Vector2(1 * (1 + .1f * Mathf.Sin(Time.time * 2.5f)), 1 * (1 + .1f * Mathf.Sin(Time.time * 2.5f)));
        else
            handles[1].rectTransform.localScale = Vector2.one;

        if (x != 4) {
            optionsTextPulses[4].enabled = false;
            //optionsTexts[4].color = new Color(saveColor.r, saveColor.g, saveColor.b, .2f);
        }
        
        if (x != 5) {
            optionsTextPulses[5].enabled = false;
            //optionsTexts[5].color = new Color(cancelColor.r, cancelColor.g, cancelColor.b, .2f);
        }

        for (int i = 0; i < optionsVarTexts.Length; i++) {
            if (i != x)
                optionsVarTexts[i].color = new Color(optionsVarTexts[i].color.r, optionsVarTexts[i].color.g, optionsVarTexts[i].color.b, .5f);
            else
                optionsVarTexts[i].color = Color.white;
        }

        optionsTextPulses[x].enabled = true;
    }

    public void clickButton() {
        aS.PlayOneShot(sounds[1]);
		if (position == 0) {
            mainMenu = false;
            StartCoroutine(ToOptions());
        }
		else if (position == 1)
            StartCoroutine(StartGame());
        else if (position == 2)
            StartCoroutine(Exit());
    }

	IEnumerator StartGame () {
        mainMenu = false;
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
        Destroy(optionsSign.gameObject);
        StartCoroutine(GetComponent<StartAnimation>().StartingAnimation());
    }

    IEnumerator ToOptions() {
        while (90 - sign.transform.eulerAngles.x > .5) {
            sign.transform.localRotation = Quaternion.Lerp(sign.transform.localRotation, signAngle, .22f);
            yield return new WaitForSeconds(.02f);
        }
        sign.gameObject.SetActive(false);
        optionsSign.gameObject.SetActive(true);
        yield return new WaitForSeconds(.02f);
        foreach (Image h in handles) {
            h.sprite = handleOptions[Random.Range(0, handleOptions.Length)];
            h.rectTransform.localRotation = Quaternion.Euler(0, 0, Random.Range(-60, 60));
        }

        if (!PlayerPrefs.HasKey("atk")) PlayerPrefs.SetString("atk", "z");

        if (!PlayerPrefs.HasKey("item")) PlayerPrefs.SetString("item", "x");

        if (!PlayerPrefs.HasKey("atkJ")) PlayerPrefs.SetString("atkJ", "joystick button 2");

        if (!PlayerPrefs.HasKey("itemJ")) PlayerPrefs.SetString("itemJ", "joystick button 3");

        if (!PlayerPrefs.HasKey("music")) PlayerPrefs.SetFloat("music", 1f);
        optionsSliders[0].value = 10 * PlayerPrefs.GetFloat("music");

        if (!PlayerPrefs.HasKey("sfx")) PlayerPrefs.SetFloat("sfx", 1f);
        optionsSliders[1].value = 10 * PlayerPrefs.GetFloat("sfx");

        attackString = PlayerPrefs.GetString("atk");
        attackStringJ = PlayerPrefs.GetString("atkJ");
        itemString = PlayerPrefs.GetString("item");
        itemStringJ = PlayerPrefs.GetString("itemJ");

        optionsVarTexts[0].text = attackString.ToUpper() + " / " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(attackStringJ);
        optionsVarTexts[1].text = itemString.ToUpper() + " / " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(itemStringJ);

        selectOptionsButton(optionsPosition);
        while (90 - optionsSign.transform.eulerAngles.x < 89.5f) {
            optionsSign.transform.localRotation = Quaternion.Lerp(optionsSign.transform.localRotation, Quaternion.identity, .22f);
            yield return new WaitForSeconds(.02f);
        }
        optionsMenu = true;
    }
    
    IEnumerator FromOptions() {
        while (90 - optionsSign.transform.eulerAngles.x > .5) {
            optionsSign.transform.localRotation = Quaternion.Lerp(optionsSign.transform.localRotation, signAngle, .22f);
            yield return new WaitForSeconds(.02f);
        }
        optionsSign.gameObject.SetActive(false);
        sign.gameObject.SetActive(true);
        while (90 - sign.transform.eulerAngles.x < 89.5f) {
            sign.transform.localRotation = Quaternion.Lerp(sign.transform.localRotation, Quaternion.identity, .22f);
            yield return new WaitForSeconds(.02f);
        }
        optionsPosition = 0;
        mainMenu = true;
    }

	IEnumerator Exit () {
		float fadeTime = GetComponent<FadeInOut>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.Quit();
	}
	
    void keyBindings() {
        for (char i = 'a'; i <= 'z'; i++) {
            if (Input.GetKeyDown("" + i)) {
                aS.PlayOneShot(sounds[0]);
                if (optionsPosition == 0) {
                    if (itemString.Equals("" + i))
                        itemString = attackString;
                    attackString = "" + i;
                }
                else if (optionsPosition == 1) {
                    if (attackString.Equals("" + i))
                        attackString = itemString;
                    itemString = "" + i;
                }
            }
        }

        for (char i = ','; i <= '9'; i++) {
            if (i != '/')
            if (Input.GetKeyDown("" + i)) {
                aS.PlayOneShot(sounds[0]);
                if (optionsPosition == 0) {
                    if (itemString.Equals("" + i))
                        itemString = attackString;
                    attackString = "" + i;
                }
                else if (optionsPosition == 1) {
                    if (attackString.Equals("" + i))
                        attackString = itemString;
                    itemString = "" + i;
                }
            }
        }

        if (Input.GetKeyDown("=")) {
            aS.PlayOneShot(sounds[0]);
            if (optionsPosition == 0)
            {
                if (itemString.Equals("="))
                    itemString = attackString;
                attackString = "=";
            }
            else if (optionsPosition == 1)
            {
                if (attackString.Equals("="))
                    attackString = itemString;
                itemString = "=";
            }
        }

        if (Input.GetKeyDown("left ctrl")) {
            aS.PlayOneShot(sounds[0]);
            if (optionsPosition == 0) {
                if (itemString.Equals("left ctrl"))
                    itemString = attackString;
                attackString = "left ctrl";
            }
            else if (optionsPosition == 1) {
                if (attackString.Equals("left ctrl"))
                    attackString = itemString;
                itemString = "left ctrl";
            }
        }

        if (Input.GetKeyDown("left shift")) {
            aS.PlayOneShot(sounds[0]);
            if (optionsPosition == 0)
            {
                if (itemString.Equals("left shift"))
                    itemString = attackString;
                attackString = "left shift";
            }
            else if (optionsPosition == 1)
            {
                if (attackString.Equals("left shift"))
                    attackString = itemString;
                itemString = "left shift";
            }
        }

        if (Input.GetKeyDown("left alt")) {
            aS.PlayOneShot(sounds[0]);
            if (optionsPosition == 0)
            {
                if (itemString.Equals("left alt"))
                    itemString = attackString;
                attackString = "left alt";
            }
            else if (optionsPosition == 1)
            {
                if (attackString.Equals("left alt"))
                    attackString = itemString;
                itemString = "left alt";
            }
        }

        if (Input.GetKeyDown("right ctrl")) {
            aS.PlayOneShot(sounds[0]);
            if (optionsPosition == 0) {
                if (itemString.Equals("right ctrl"))
                    itemString = attackString;
                attackString = "right ctrl";
            }
            else if (optionsPosition == 1) {
                if (attackString.Equals("right ctrl"))
                    attackString = itemString;
                itemString = "right ctrl";
            }
        }

        if (Input.GetKeyDown("right shift")) {
            aS.PlayOneShot(sounds[0]);
            if (optionsPosition == 0)
            {
                if (itemString.Equals("right shift"))
                    itemString = attackString;
                attackString = "right shift";
            }
            else if (optionsPosition == 1)
            {
                if (attackString.Equals("right shift"))
                    attackString = itemString;
                itemString = "right shift";
            }
        }

        if (Input.GetKeyDown("right alt")) {
            aS.PlayOneShot(sounds[0]);
            if (optionsPosition == 0)
            {
                if (itemString.Equals("right alt"))
                    itemString = attackString;
                attackString = "right alt";
            }
            else if (optionsPosition == 1)
            {
                if (attackString.Equals("right alt"))
                    attackString = itemString;
                itemString = "right alt";
            }
        }

        for (int i = 0; i < 20; i++) {
            if (Input.GetKeyDown("joystick button " + i)) {
                aS.PlayOneShot(sounds[0]);
                if (optionsPosition == 0) {
                    if (itemStringJ.Equals("joystick button " + i))
                        itemStringJ = attackStringJ;
                    attackStringJ = "joystick button " + i;
                }
                else if (optionsPosition == 1) {
                    if (attackStringJ.Equals("joystick button " + i))
                        attackStringJ = itemStringJ;
                    itemStringJ = "joystick button " + i;
                }
                break;
            }
        }
    }

    void Start() {
        aS = GetComponent<AudioSource>();

        signAngle = Quaternion.Euler(90f, 0f, 0f);
        notSelected = new Color(.8f, .8f, .8f, .8f);
        logoVanish = new Vector2(2f, 0f);

        saveColor = optionsTexts[optionsTexts.Length - 2].color;
        cancelColor = optionsTexts[optionsTexts.Length - 1].color;

        optionsTextPulses = new ImagePulse[optionsTexts.Length + optionsVarTexts.Length];
        for (int i = 0; i < optionsTexts.Length; i++)
            optionsTextPulses[i] = optionsTexts[i].GetComponent<ImagePulse>();
        for (int i = optionsTexts.Length; i < optionsVarTexts.Length; i++)
            optionsTextPulses[i] = optionsVarTexts[i- optionsTexts.Length].GetComponent<ImagePulse>();
    }
    
	void Update () {
        //if (Input.anyKeyDown) Debug.Log(Input.inputString);
	    if (mainMenu) {
		    if (Input.GetAxisRaw("Horizontal") == 1)
			    if (inputStuff) {
                    aS.PlayOneShot(sounds[0]);
				    position = (position + 1) % spriteOff.Length;
				    inputStuff = false;
			    }

		    if (Input.GetAxisRaw("Horizontal") == -1)
			    if (inputStuff) {
                    aS.PlayOneShot(sounds[0]);
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

        if (optionsMenu) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                aS.PlayOneShot(sounds[3]);
                optionsMenu = false;
                StartCoroutine(FromOptions());
            }
            
		    if (Input.GetAxisRaw("Vertical") == -1)
			    if (inputStuff) {
                    aS.PlayOneShot(sounds[0]);
                    if (optionsPosition == 5)
                        optionsPosition = 0;
                    else
				        optionsPosition = (optionsPosition + 1) % 5;
				    inputStuff = false;
			    }

		    if (Input.GetAxisRaw("Vertical") == 1)
			    if (inputStuff) {
                    aS.PlayOneShot(sounds[0]);
                    if (optionsPosition == 0)
                        optionsPosition = 5 - 1;
                    else if (optionsPosition == 5)
                        optionsPosition = 3;
                    else
                        optionsPosition = (optionsPosition - 1) % 5;
				    inputStuff = false;
			    }


            if (Input.GetAxisRaw("Horizontal") != 0 && inputStuff)
            {
                if (optionsPosition == 2) {
                    aS.PlayOneShot(sounds[0]);
                    optionsSliders[0].value += Input.GetAxisRaw("Horizontal");
                    inputStuff = false;
                }
                else if (optionsPosition == 3) {
                    aS.PlayOneShot(sounds[0]);
                    optionsSliders[1].value += Input.GetAxisRaw("Horizontal");
                    inputStuff = false;
                }
                else if (optionsPosition == 4) {
                    aS.PlayOneShot(sounds[0]);
                    optionsPosition = 5;
                    inputStuff = false;
                } else if (optionsPosition == 5) {
                    aS.PlayOneShot(sounds[0]);
                    optionsPosition = 4;
                    inputStuff = false;
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
			inputStuff = true;

		    if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.JoystickButton0) || Input.GetKeyDown (KeyCode.JoystickButton7))
                if (optionsPosition == 4) {
                    aS.PlayOneShot(sounds[2]);
                    PlayerPrefs.SetString("atk", attackString);
                    PlayerPrefs.SetString("item", itemString);
                    PlayerPrefs.SetString("atkJ", attackStringJ);
                    PlayerPrefs.SetString("itemJ", itemStringJ);
                    PlayerPrefs.SetFloat("music", optionsSliders[0].value / 10);
                    PlayerPrefs.SetFloat("sfx", optionsSliders[1].value / 10);
                    PlayerPrefs.Save();
                    optionsMenu = false;
                    StartCoroutine(FromOptions());

                }
                else if (optionsPosition == 5) {
                    aS.PlayOneShot(sounds[3]);
                    optionsMenu = false;
                    StartCoroutine(FromOptions());
                }

            if(optionsPosition <= 1) keyBindings();

            optionsVarTexts[0].text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(attackString + " / " + attackStringJ);
            optionsVarTexts[1].text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(itemString + " / " + itemStringJ);

            selectOptionsButton (optionsPosition);

        }
	}
}
