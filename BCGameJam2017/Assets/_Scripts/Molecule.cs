using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour {

    public float speed = 250f;
    private int x, y;
    private Vector3 direction, reflect;
    private Rigidbody rb;
    private bool moveMolecules = false;
    
    void Start () {
        randomizeDirection();
        
        rb = GetComponent<Rigidbody>();
    }
	
	void Update () {
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        reflect = collision.contacts[0].normal;
        direction = Vector3.Reflect(direction, reflect);
    }

    private void randomizeDirection()
    {
        x = Random.Range(-1, 1);
        y = Random.Range(-1, 1);
        if(x == 0 && y == 0)
        {
            x = 1;
            y = 1;
        }
        direction = new Vector3(x, y, 0);
    }
}
