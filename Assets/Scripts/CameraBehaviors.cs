using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviors : MonoBehaviour {

    [SerializeField]
    private Player player;

    [HideInInspector]
    public Vector2 position;
    [HideInInspector]
    public float size;

    private Camera cam;

    private bool isPulsing = false;

    // Use this for initialization
    void Start () {
        position = transform.position;
        cam = GetComponent<Camera>();

	}

    IEnumerator pulseCoRoutine(float dist, float dur)
    {
        float distance = dist;
        float playerDistance = Vector2.Distance(transform.position, player.transform.position) / 3;
        Vector2 aux;
        while (distance > 0)
        {
            aux = Vector2.MoveTowards(transform.position, player.transform.position, playerDistance/10);
            transform.position = new Vector3(aux.x, aux.y, transform.position.z);
            cam.orthographicSize -= dist/10;
            distance -= dist/10;
            yield return new WaitForSeconds(dur/20);
        }
        while (distance < dist)
        {
            aux = Vector2.MoveTowards(transform.position, Vector2.zero, playerDistance / 10);
            transform.position = new Vector3(aux.x, aux.y, transform.position.z);
            cam.orthographicSize += dist / 10;
            distance += dist/10;
            yield return new WaitForSeconds(dur/20);
        }
        isPulsing = false;
    }

    public void pulseCamera(float dist, float dur)
    {
        if (!isPulsing) {
            isPulsing = true;
            StartCoroutine(pulseCoRoutine(dist, dur));
        }
    }
}
