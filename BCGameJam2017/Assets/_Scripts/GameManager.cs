using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int carbonDioxideCounter, methaneCounter, h2oCounter,n2oCounter;
    private Vector3 randomPosition;
    private float x, y;
    public GameObject carbonDioxide, methane, h2o, n2o;

    public float moleculeSpawnMinX = -200, moleculeSpawnMaxX = 200, moleculeSpawnMinY = 40, moleculeSpawnMaxY = 100;

    public float startTemp = 20;
    public float maxTemp = 100;

    public float tempIncreaseRate = 10;
    public float tempDecreaseRate = 5;

    private float currentTemp;

    void Start() {
        currentTemp = startTemp;

        carbonDioxideCounter = 10;
        methaneCounter = 0;
        spawnMolecules(carbonDioxide, 7);
        spawnMolecules(methane, 3);
        spawnMolecules(h2o, 5);
        spawnMolecules(n2o, 2);
    }

    void Update() {
        currentTemp += tempIncreaseRate * Time.deltaTime;
        Debug.Log(currentTemp);
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
