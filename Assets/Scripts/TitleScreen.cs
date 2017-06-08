using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	[SerializeField]
	private Transform[] spriteOff;
	//[SerializeField]
	//private Transform[] spriteOn;

	private int position;
	private bool inputStuff = true;

	public void selectButton(int x) {
		position = x;
		spriteOff [x].localScale = new Vector3 (1.2f, 1.2f, 1f);
		foreach (Transform i in spriteOff) {
			if (i != null && i != spriteOff[x])
				i.localScale = new Vector3 (1f, 1f, 1f);
		}
	}

	public void clickButton() {
		//PlayAudio(1);
		if (position == 0)
			StartCoroutine(StartGame());
		else if (position == 1)
		{
			//TODO Instructions Scene
		}
		else if (position == 2)
		{
			//TODO Store Scene
		}
		else if (position == 3)
			StartCoroutine(Exit());
	}

	IEnumerator StartGame () {
		float fadeTime = GetComponent<FadeInOut>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(1);
	}

	IEnumerator Exit () {
		float fadeTime = GetComponent<FadeInOut>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.Quit();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetAxisRaw("Vertical") == -1)
			if (inputStuff)
			{
				//PlayAudio(0);
				position = (position + 1) % spriteOff.Length;
				inputStuff = false;
			}

		if (Input.GetAxisRaw("Vertical") == 1)
			if (inputStuff)
			{
				//PlayAudio(0);
				if (position == 0)
					position = spriteOff.Length - 1;
				else
					position = (position - 1) % spriteOff.Length;
					inputStuff = false;
			}

		if (Input.GetAxisRaw("Vertical") == 0)
			inputStuff = true;

		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.JoystickButton0) || Input.GetKeyDown (KeyCode.JoystickButton7))
			clickButton ();

		selectButton (position);
	}
}
