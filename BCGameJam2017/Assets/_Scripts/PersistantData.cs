using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantData : MonoBehaviour {
    public static bool created = false;

    public int carbonDioxideCounter, methaneCounter, h2oCounter, n2oCounter;
    public int questionNumber;
    public bool firstTimePlaying = true;

    void Awake() {

         if (!created) {
                DontDestroyOnLoad(this.gameObject);
                created = true;
            }

            else {
                Destroy(this.gameObject);
            }
   }

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown("z"))
            SceneManager.LoadScene("GameplayScene");

        if (Input.GetKeyDown("x"))
            SceneManager.LoadScene("QuestionScene");
    }
}
