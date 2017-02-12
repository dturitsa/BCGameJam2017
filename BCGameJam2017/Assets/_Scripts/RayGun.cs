using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGun : MonoBehaviour {
    public Transform spawnPoint;
    public RayProjectile rayProjectile;
    public AudioSource shootSound;
    public float gunMoveSpeed = 10;
    public float reloadTime = 1;
    public float spread = 10f;
    public bool isPlayerControlled = true;
    public float rayVelocity = 50;
    public float leftBound = -250, rightBound = 250;
    private float timeSinceFired = 0;
    // Use this for initialization
    void Start() {
        timeSinceFired = reloadTime;
    }

    // Update is called once per frame
    void Update() {
        timeSinceFired += Time.deltaTime;

        if (isPlayerControlled) {
            aimGun();
            if (Input.GetAxis("Fire1") > 0) {
                fire();
                if (shootSound != null && !shootSound.isPlaying)
                    shootSound.Play();   
            }
            else if (timeSinceFired > 2 * reloadTime &&shootSound != null) {
                shootSound.Stop();
            }
                
            
        }
        else {
            fire();
        }
    }


    private void aimGun() {
        //gun rotation
        Vector3 cursorInWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
           Input.mousePosition.y, -Camera.main.transform.position.z));
       // Debug.Log("Mouse Pos:" + Input.mousePosition + "Screen Width:" + Screen.width + "MouseInWorld:" + cursorInWorldPos);
        transform.LookAt(cursorInWorldPos);

        //left right movement
        transform.Translate(Vector3.left * gunMoveSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime, Space.World);
        if (transform.position.x < leftBound)
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);

        if (transform.position.x > rightBound)
            transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
    }

    public void fire() {
        if (timeSinceFired < reloadTime) {
            return;
        }
            
        timeSinceFired = 0;
        RayProjectile p = Instantiate(rayProjectile) as RayProjectile;
        p.transform.position = spawnPoint.position;

        float xDegrees = spawnPoint.eulerAngles.x + Random.Range(-spread, spread);
        float yDegrees = spawnPoint.eulerAngles.y + Random.Range(-spread, spread);
        float zDegrees = spawnPoint.eulerAngles.z;
        p.transform.rotation = Quaternion.Euler(xDegrees, yDegrees, zDegrees);

      //  p.transform.rotation = transform.rotation;
        p.GetComponent<Rigidbody>().velocity = p.transform.forward * rayVelocity;

           
    }
}
