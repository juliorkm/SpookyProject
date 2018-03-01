using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PostGame : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI message;
    [SerializeField]
    private Transform sign;
    [SerializeField]
    private TextMeshProUGUI scoreText, hiscoreText;

    [SerializeField]
    private AudioClip clickSound;
    private AudioSource aS;

    [SerializeField]
    private int score, hiscore;
    private bool canPressButton = false;

    public bool pressButton() {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.JoystickButton0);
    }

    public bool pressSkipButton() {
        return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7);
    }

    IEnumerator ToTitle() {
        float fadeTime = FindObjectOfType<FadeInOut>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(1);
    }

    public IEnumerator Animation() {
        while (90 - sign.eulerAngles.x < 89.5f) {
            sign.localRotation = Quaternion.Lerp(sign.localRotation, Quaternion.identity, .08f);
            yield return new WaitForSeconds(.02f);
        }
        yield return new WaitForSeconds(1f);
        while (message.transform.localScale.y < 1) {
            message.transform.localScale = Vector2.Lerp(message.transform.localScale, Vector2.one, .5f);
            yield return new WaitForSeconds(.02f);
        }
        canPressButton = true;
    }

    // Use this for initialization
    void Start () {
        aS = GetComponent<AudioSource>();

        score = PlayerPrefs.GetInt("score");
        hiscore = (PlayerPrefs.HasKey("hiscore")) ? PlayerPrefs.GetInt("hiscore") : 0;

        scoreText.text = "" + score;
        hiscoreText.text = "" + hiscore;

        if (score > hiscore) {
            scoreText.enableVertexGradient = true;
            Color yellow = new Color(1f, .882f, .212f);
            scoreText.colorGradient = new VertexGradient(yellow, yellow, new Color(1f, .647f, 0f), Color.white);
            PlayerPrefs.SetInt("hiscore", score);
            PlayerPrefs.DeleteKey("score");
            PlayerPrefs.Save();
        }

        StartCoroutine(Animation());
	}
	
	// Update is called once per frame
	void Update () {
        if (canPressButton) {
            if (pressButton()) StartCoroutine(ToTitle());
        }
        if (pressSkipButton()) StartCoroutine(ToTitle());
    }
}
