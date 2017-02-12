using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cleanDebris();
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void cleanDebris()
    {
        Destroy(this, 4);
    }
}
