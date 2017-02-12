using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject carbonDioxide, methane, h2o, n2o;
    public AudioSource sizzleSOund, winSound, loseSound;
    public float moleculeSpawnMinX = -200, moleculeSpawnMaxX = 200, moleculeSpawnMinY = 40, moleculeSpawnMaxY = 100;
    public float startTemp = 20;
    public float maxTemp = 100;
    public float tempIncreaseRate = 10;
    public float tempDecreaseRate = 5;
    public float WaterMovementRange = 100, iceMovementRange = 200;
    public float waterSpeed = 10, iceSpeed = 20;
    public Transform water, iceberg;
    public temperature thermometer;
    public float levelTimeLimit = 30;
    public Text timerText;

    private PersistantData persistantData;
    private Vector3 randomPosition;
    private float x, y;
    private float currentTemp;
    private float waterStartY, iceStartY;
    private float timeLeft;
    private Vector3 maxicebergSize;
    private float gameOverDuration = 2;
    private bool lostGame = false;

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
        timeLeft = levelTimeLimit + 3;
    }

    void Update() {
        if(timeLeft <= levelTimeLimit)
            gameplayUpdate();
        else
            timeLeft -= Time.deltaTime;
    }

    private void gameplayUpdate() {
        currentTemp += tempIncreaseRate * Time.deltaTime;
        if (currentTemp > 100)
            currentTemp = 100;
        if (currentTemp < 0)
            currentTemp = 0;
        // Debug.Log(currentTemp);
        thermometer.temp = currentTemp;
        smoothedWaterMovement();

        if (currentTemp > 80 && !sizzleSOund.isPlaying && !lostGame)
            sizzleSOund.Play();
        else if (currentTemp < 80 || lostGame)
            sizzleSOund.Stop();

        if (timeLeft < -2 && !lostGame) {
            if(persistantData.questionNumber < 6)
                SceneManager.LoadScene("QuestionScene");
            else
                SceneManager.LoadScene("MainMenuScene");
        }
            


        if (currentTemp >= 100 || lostGame) {
            lostGame = true;
            timerText.text = "Game Over";
            currentTemp = 100;
            if(!loseSound.isPlaying)
                loseSound.Play();
            //timeLeft -= Time.deltaTime;
            gameOverDuration -= Time.deltaTime;
            if (gameOverDuration <= 0)
                SceneManager.LoadScene("MainMenuScene");
        }
        else {
            timeLeft -= Time.deltaTime;
            timerText.text = timeLeft.ToString("F0");
        }
        if (timeLeft <= 0) {
            timerText.text = "Success";
            currentTemp = 0;
            if(!winSound.isPlaying)
                winSound.Play();
        }
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
