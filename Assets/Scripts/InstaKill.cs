using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstaKill : MonoBehaviour {
    
    [SerializeField, Range(0,10)]
    private float secondsUntilStart;
    [SerializeField, Range(0, 10)]
    private float darknessDuration;
    [SerializeField, Range(.1f, 1)]
    private float minSpeedMultiplier;
    [SerializeField, Range(0, 100)]
    private float damagePerSecond;

    [HideInInspector]
    public float timer = 0;

    public Image black;
    public GameObject warning;
    private Player player;

    [HideInInspector]
    public PortraitAnim pa;

    private Vector2 viewportPoint;

    IEnumerator showWarning() {
        //StopCoroutine(showWarning());
        StopCoroutine(removeWarning());
        StopCoroutine(removeWarning());
        StopCoroutine(removeWarning());
        StopCoroutine(removeWarning());
        warning.SetActive(true);
        warning.transform.localScale = new Vector2(.5f, 0f);
        while (warning.transform.localScale.y < .999f) {
            warning.transform.localScale = Vector2.Lerp(warning.transform.localScale, Vector2.one, .5f);
            yield return new WaitForSeconds(.02f);
        }
    }

    IEnumerator removeWarning() {
        StopCoroutine(showWarning());
        StopCoroutine(showWarning());
        StopCoroutine(showWarning());
        StopCoroutine(showWarning());
        //StopCoroutine(removeWarning());
        warning.SetActive(true);
        warning.transform.localScale = Vector2.one;
        Vector2 dest = new Vector2(.5f, 0f);
        while (warning.transform.localScale.y > .001f) {
            warning.transform.localScale = Vector2.Lerp(warning.transform.localScale, dest, .5f);
            yield return new WaitForSeconds(.02f);
        }
        warning.SetActive(false);
    }

	// Use this for initialization
	void Awake () {
        pa = FindObjectOfType<PortraitAnim>();
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < secondsUntilStart + darknessDuration) timer += Time.deltaTime;

        if (timer >= secondsUntilStart) {
            if (!warning.activeSelf) StartCoroutine(showWarning());
            if(player != null && !player.dead) viewportPoint = Camera.main.WorldToViewportPoint(new Vector2(player.transform.position.x, player.transform.position.y + .5f));
            black.rectTransform.anchorMin = viewportPoint;
            black.rectTransform.anchorMax = viewportPoint;
            pa.anim.SetBool("Fear", true);
            black.transform.localScale = Vector2.Lerp(black.transform.localScale, 
                    new Vector2(-11 * ((timer - secondsUntilStart) / darknessDuration) + 12, -11 * ((timer - secondsUntilStart) / darknessDuration) + 12), .2f);
            if (timer >= secondsUntilStart + darknessDuration) {
                player.health -= damagePerSecond * Time.deltaTime;
            }
        }
        else {
            black.transform.localScale = Vector2.Lerp(black.transform.localScale, new Vector2(12f, 12f), .2f);
            
            if (warning.activeSelf) StartCoroutine(removeWarning());
        }

        player.speedMult = ((black.transform.localScale.x - 1) * (1 - minSpeedMultiplier) / 11) + minSpeedMultiplier;
    }
}
