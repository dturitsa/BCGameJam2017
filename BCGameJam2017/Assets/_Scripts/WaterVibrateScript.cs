using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVibrateScript : MonoBehaviour
{
    public float Displacement = 0.5f;
    public float Rate = 0.1f;
    public float RandomChance = 0.25f;
    public Vector3 Direction;


    private Vector3 originalPosition;
    private int internalDirection;

	void Start ()
    {
        originalPosition = transform.localPosition;
        internalDirection = 1;
	}
	

	void Update ()
    {
		//if we've gone too far, invert!
        if((originalPosition - transform.localPosition).magnitude > Displacement)
        {
            internalDirection = internalDirection * -1;
        }
        else
        {
            if(Random.Range(0f, 1f) < RandomChance)
            {
                internalDirection = internalDirection * -1;
            }
        }

        float random = Random.Range(0, Rate);

        transform.Translate(Direction * Time.deltaTime * random * internalDirection, Space.Self);

	}
}
