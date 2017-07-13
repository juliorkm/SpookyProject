using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstaKill : MonoBehaviour {
    
    [SerializeField, Range(0,10)]
    private float secondsUntilStart;
    [SerializeField, Range(0, 10)]
    private float darknessDuration;

    [HideInInspector]
    public float timer = 0;

    public Image black;
    private Player player;

    [HideInInspector]
    public PortraitAnim pa;
    private bool iKilledHer = false;

	// Use this for initialization
	void Awake () {
        pa = FindObjectOfType<PortraitAnim>();
        player = FindObjectOfType<Player>();
        black.color = new Color(0f, 0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (!iKilledHer)
            if (timer >= secondsUntilStart) {
                pa.anim.SetBool("Fear", true);
                black.color = Color.Lerp(black.color, new Color(0f, 0f, 0f, (timer - secondsUntilStart) / darknessDuration), .2f);
                if (timer >= secondsUntilStart + darknessDuration) {
                    iKilledHer = true;
                    player.health = 0;
                }
            }
            else
                black.color = Color.Lerp(black.color, new Color(0f, 0f, 0f, 0f), .2f);
        else
            black.color = Color.black;
    }
}
