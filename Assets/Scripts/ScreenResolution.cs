using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour {
    private float pastWidth;
    private float pastHeight;

    void Start () {
        pastWidth = Screen.width;
        pastHeight = Screen.height;
    }
	void Update () {
        if ((float) Screen.width / (float) Screen.height != 4f/3f) {
		    if (Screen.width != pastWidth) {
                var heightAccordingToWidth = Screen.width / 4f * 3f;
                Screen.SetResolution(Screen.width, (int) Mathf.Round(heightAccordingToWidth), Screen.fullScreen, 0);
            }
            else if (Screen.height != pastHeight) {
                var widthAccordingToHeight = Screen.height / 3f * 4f;
                Screen.SetResolution((int) Mathf.Round(widthAccordingToHeight), Screen.height, Screen.fullScreen, 0);
            }
        }
        pastWidth = Screen.width;
        pastHeight = Screen.height;
    }
}
