using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartAnimation : MonoBehaviour {

    [SerializeField]
    GameObject ScoreSign;
    [SerializeField]
    GameObject LifeHUD;

    [SerializeField]
    GameObject[] GameManagers;
    [SerializeField]
    GameObject[] HUD;

    [SerializeField]
    Image Lightning;

    [SerializeField]
    GameObject Player;
    [SerializeField]
    GameObject Rain, Mist;

    private Vector3 LifePosition;
    private Quaternion SignRotation;
    private Quaternion LifeRotation;

    // Use this for initialization
    void Start () {
        //LifePosition = LifeHUD.transform.localPosition;
        SignRotation = ScoreSign.transform.localRotation;
        LifeRotation = LifeHUD.transform.localRotation;

        //LifeHUD.transform.localPosition = new Vector3(0f, 200f, 0f);
        ScoreSign.transform.localRotation = Quaternion.Euler(0f, 0f, -60f);
        LifeHUD.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        foreach (GameObject g in GameManagers) g.SetActive(false);
        foreach (GameObject h in HUD) h.SetActive(false);

        //StartCoroutine(StartingAnimation());
	}
	
	public IEnumerator StartingAnimation () {
        yield return new WaitForSeconds(1f);
        GameObject p;

        Lightning.gameObject.SetActive(true);

        for (int i = 0; i <= 4; i++) {
            yield return new WaitForSeconds(.01f);
            Lightning.color = new Color(1f, 1f, 1f, (float)i / 5);
        }
        for (int i = 4; i >= 2; i--) {
            yield return new WaitForSeconds(.01f);
            Lightning.color = new Color(1f, 1f, 1f, (float)i / 5);
        }
        for (int i = 2; i <= 4; i++) {
            yield return new WaitForSeconds(.01f);
            Lightning.color = new Color(1f, 1f, 1f, (float)i / 5);
        }
        for (int i = 4; i >= 0; i--) {
            yield return new WaitForSeconds(.01f);
            Lightning.color = new Color(1f, 1f, 1f, (float)i / 5);
        }

        yield return new WaitForSeconds(.1f);

        for (int i = 0; i <= 4; i++) {
            yield return new WaitForSeconds(.01f);
            Lightning.color = new Color(1f, 1f, 1f, (float)i / 5);
        }
        p = Instantiate(Player);
        for (int i = 4; i >= 0; i--) {
            yield return new WaitForSeconds(.01f);
            Lightning.color = new Color(1f, 1f, 1f, (float)i / 5);
        }
        Instantiate(Rain);

        foreach (GameObject h in HUD) h.SetActive(true);
        yield return new WaitForSeconds(.4f);
        foreach (GameObject g in GameManagers) g.SetActive(true);
        while (LifeHUD.transform.localEulerAngles.z - LifeRotation.eulerAngles.z > 10f) {
            ScoreSign.transform.localRotation = Quaternion.Lerp(ScoreSign.transform.localRotation, SignRotation, .1f);
            LifeHUD.transform.localRotation = Quaternion.Lerp(LifeHUD.transform.localRotation, LifeRotation, .1f);
            //LifeHUD.transform.localPosition = Vector3.Lerp(LifeHUD.transform.localPosition, LifePosition, .1f);
            yield return new WaitForSeconds(.02f);
        }
        p.GetComponent<Player>().paused = false;
        while (LifeHUD.transform.localEulerAngles.z - LifeRotation.eulerAngles.z > .01f) {
            ScoreSign.transform.localRotation = Quaternion.Lerp(ScoreSign.transform.localRotation, SignRotation, .1f);
            LifeHUD.transform.localRotation = Quaternion.Lerp(LifeHUD.transform.localRotation, LifeRotation, .1f);
            //LifeHUD.transform.localPosition = Vector3.Lerp(LifeHUD.transform.localPosition, LifePosition, .1f);
            yield return new WaitForSeconds(.02f);
        }
    }
}
