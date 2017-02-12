using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject carbonDioxide, methane, h2o, n2o;
    public float moleculeSpawnMinX = -200, moleculeSpawnMaxX = 200, moleculeSpawnMinY = 40, moleculeSpawnMaxY = 100;
    public float startTemp = 20;
    public float maxTemp = 100;
    public float tempIncreaseRate = 10;
    public float tempDecreaseRate = 5;
    public float WaterMovementRange = 100, iceMovementRange = 200;
    public float waterSpeed = 10, iceSpeed = 20;
    public Transform water, iceberg;
    public temperature thermometer;


    private PersistantData persistantData;
    private Vector3 randomPosition;
    private float x, y;
    private float currentTemp;
    private float waterStartY, iceStartY;

    private Vector3 maxicebergSize;

    void Start() {
        persistantData = (PersistantData)FindObjectOfType(typeof(PersistantData));

        currentTemp = startTemp;

        spawnMolecules(carbonDioxide, persistantData.carbonDioxideCounter);
        spawnMolecules(methane, persistantData.methaneCounter);
        spawnMolecules(h2o, persistantData.h2oCounter);
        spawnMolecules(n2o, persistantData.n2oCounter);

        waterStartY = water.position.y;
        iceStartY = iceberg.position.y;
        maxicebergSize = iceberg.localScale;
    }

    void Update() {
        currentTemp += tempIncreaseRate * Time.deltaTime;
        if (currentTemp > 100)
            currentTemp = 100;
        if (currentTemp < 0)
            currentTemp = 0;
        // Debug.Log(currentTemp);
        thermometer.temp = currentTemp;
        smoothedWaterMovement();
    }
    private void smoothedWaterMovement() {
        float tempDecimal = currentTemp / 100;

        Vector3 waterTarget = new Vector3(water.position.x, waterStartY + WaterMovementRange * tempDecimal, water.position.z);
        //Vector3 movePos = Vector3.MoveTowards(transform.position, waterTarget, .0001f * Time.deltaTime);
        if(water.position.y < waterTarget.y)
            water.Translate(Vector3.up * waterSpeed *  Time.deltaTime, Space.World);
        if (water.position.y > waterTarget.y)
            water.Translate(Vector3.up * -waterSpeed * Time.deltaTime, Space.World);

        Vector3 icebergTargetSize = new Vector3(maxicebergSize.x * (1 - tempDecimal), maxicebergSize.y * (1 - tempDecimal), maxicebergSize.z * (1 - tempDecimal));
        if (iceberg.localScale.x < icebergTargetSize.x) {
            iceberg.localScale += new Vector3(iceSpeed * Time.deltaTime, iceSpeed * Time.deltaTime, iceSpeed * Time.deltaTime);
        }
        else {
            iceberg.localScale += new Vector3(-iceSpeed * Time.deltaTime, -iceSpeed * Time.deltaTime, -iceSpeed * Time.deltaTime);
        }      
    }

    public void reduceTemp() {
        currentTemp -= tempDecreaseRate;
    }

    private void spawnMolecules(GameObject molecule, int moleculeCounter) {
        for (int i = 0; i < moleculeCounter; i++) {
            x = Random.Range(moleculeSpawnMinX, moleculeSpawnMaxX);
            y = Random.Range(40, 120);
            randomPosition = new Vector3(x, y, 0);
           // var CO2 = (GameObject)Instantiate(carbonDioxide, randomPosition, Quaternion.identity);
            GameObject p = Instantiate(molecule) as GameObject;
            p.transform.position = randomPosition;
        }
     
    }
}
