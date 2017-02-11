using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayProjectile : MonoBehaviour {
    public GameManager gameManager;
    public float lifeDuration = 10;
    public float topBoundary = 1000;
    public float bottomBoundary = -500;
	// Use this for initialization
	void Start () {
        Object.Destroy(gameObject, lifeDuration);
        gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
    }
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y > topBoundary) {
            gameManager.reduceTemp();
            Object.Destroy(gameObject);
        }
        if (transform.position.y < bottomBoundary) {
            //came back to earth
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "moleculeBoundary") {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

        }else {
            gameObject.GetComponent<Collider>().enabled = false;
            transform.rotation = Quaternion.Euler(90, 0, 0);
            GetComponent<Rigidbody>().velocity = transform.forward * 400;
        }

       
        
    }
}
