using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImagePulse : MonoBehaviour {

    [SerializeField, Range(0, 1)]
    private float minOpacity;
    [SerializeField, Range(0, 1)]
    private float maxOpacity;

    private Image im;
    //private bool b = true;
    private float f;

	void Start () {
        im = GetComponent<Image>();
        f = 0;
        im.color = new Color(1f, 1f, 1f, f);
    }
	
	void Update () {
        /*
        if (b) {
            if (maxOpacity - im.color.a > .01)
                if (f < (maxOpacity + minOpacity) / 2)
                    f += rate*10 * Time.timeScale * (im.color.a - minOpacity);
                else
                    f += rate * 10 * Time.timeScale * (maxOpacity - im.color.a);
                //f = Mathf.Lerp(f, maxOpacity, rate*10);
            else
                b = false;
        }
        else {
            if (im.color.a - minOpacity > .01)
                if (f > (maxOpacity + minOpacity)/2)
                    f -= rate * 10 * Time.timeScale * (maxOpacity - im.color.a);
                else
                    f -= rate * 10 * Time.timeScale * (im.color.a - minOpacity);
                //f = Mathf.Lerp(f, minOpacity, rate*10);
            else
                b = true;
        }
        */
        f = (Mathf.Sin(Time.time * 5) + 1) / 2 * (maxOpacity-minOpacity) + minOpacity;
        im.color = new Color(1f, 1f, 1f, f);
    }
}
