using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntro : MonoBehaviour {

    private Vector3 finalPosition, initialPosition;
    public float speed = 5000f;
	// Use this for initialization
	void Start () {
        finalPosition = new Vector3(0f, 51.5f, -226f);
        initialPosition = new Vector3(0f, 51.5f, -4000f);
        transform.position = initialPosition;
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * 10 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, step);
	}
}
