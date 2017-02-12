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

    void Start() {
        persistantData = (PersistantData)FindObjectOfType(typeof(PersistantData));

        currentTemp = startTemp;

        spawnMolecules(carbonDioxide, persistantData.carbonDioxideCounter);
        spawnMolecules(methane, persistantData.methaneCounter);
        spawnMolecules(h2o, persistantData.h2oCounter);
        spawnMolecules(n2o, persistantData.n2oCounter);

        waterStartY = water.position.y;
        iceStartY = iceberg.position.y;
    }

    void Update() {
        currentTemp += tempIncreaseRate * Time.deltaTime;
        // Debug.Log(currentTemp);
        thermometer.temp = currentTemp;
    }
    private void smoothedWaterMovement() {
        Vector3 waterTarget = new Vector3(water.position.x, waterStartY + WaterMovementRange * currentTemp);
      //  water.position = Vector3.MoveTowards(transform.position, target.position, waterSpeed * Time.deltaTime);
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
