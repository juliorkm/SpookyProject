using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum audioType{
    Ambiente, sFX
}

[RequireComponent(typeof(AudioSource))]
public class SoundVolume : MonoBehaviour {

    [SerializeField]
    private audioType audioType;
    [SerializeField]
    private bool fadesIn;
    [SerializeField, Range(0, 5)]
    private float fadeDuration;

    private float timer = 0;
    private AudioSource audiosrc;
    
	void Start () {
        if (!PlayerPrefs.HasKey("music")) PlayerPrefs.SetFloat("music",1f);
        if (!PlayerPrefs.HasKey("sfx")) PlayerPrefs.SetFloat("sfx", 1f);
        audiosrc = GetComponent<AudioSource>();
        if (fadesIn) audiosrc.volume = 0f;
    }
	
	void Update () {
        if (fadesIn && timer < 1f)
            timer += Time.deltaTime / fadeDuration;
        else
            timer = 1f;
		switch(audioType) {
            case audioType.Ambiente:
                audiosrc.volume = PlayerPrefs.GetFloat("music") * timer;
                break;
            case audioType.sFX:
                audiosrc.volume = PlayerPrefs.GetFloat("sfx") * timer;
                break;
        }
	}
}
