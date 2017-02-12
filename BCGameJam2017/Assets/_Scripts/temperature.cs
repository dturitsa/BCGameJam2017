using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class temperature : MonoBehaviour {

	public float temp = 0;
	public GameObject slider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		slider.GetComponent<Slider> ().value = temp;
	}
}
