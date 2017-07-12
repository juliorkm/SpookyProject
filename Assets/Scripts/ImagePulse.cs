using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ImagePulse : MonoBehaviour {

    [SerializeField, Range(0, 1)]
    private float minOpacity;
    [SerializeField, Range(0, 1)]
    private float maxOpacity;

    private Image im;
    private TextMeshProUGUI tx;
    private float f;
    private Color initColor;

    private bool isText = false;

	void Start () {
        im = GetComponent<Image>();
        if (im == null) {
            isText = true;
            tx = GetComponent<TextMeshProUGUI>();
        }
        f = 0;
        initColor = (isText) ? tx.color : im.color;
        if (isText)
            tx.color = new Color(initColor.r, initColor.g, initColor.b, f);
        else
            im.color = new Color(initColor.r, initColor.g, initColor.b, f);
    }
	
	void Update () {
        f = (Mathf.Sin(Time.time * 5) + 1) / 2 * (maxOpacity-minOpacity) + minOpacity;
        if (isText)
            tx.color = new Color(initColor.r, initColor.g, initColor.b, f);
        else
            im.color = new Color(initColor.r, initColor.g, initColor.b, f);
    }
}
