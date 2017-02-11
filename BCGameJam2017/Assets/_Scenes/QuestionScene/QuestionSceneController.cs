using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionSceneController : MonoBehaviour
{
    //portability or something
    const string MAIN_SCENE_NAME = "GameScene";
    const string CO2_COUNT_VARNAME = "CO2Count";
    const string METHANE_COUNT_VARNAME = "MethaneCount";
    const string NOS_COUNT_VARNAME = "NosCount";
    const string QUESTION_NUMBER_VARNAME = "QuestionsComplete"; //0-indexed
    const string BACK_RESOURCE_FMTSTRING = "Questions/Question{0}Background";
    const string CARD_RESOURCE_FMTSTRING = "Questions/Question{0}Card{1}Side{2}";

    private readonly string[] QUESTIONS = {"","",""};

    enum ControllerState
    {
        Opening, QuestionsPresented, AnswerChosen, AnswersRevealed, ContinueSelected, Done
    }

    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public Text QuestionText;
    public GameObject ContinueButton;
    public GameObject BackgroundObject;

    private int cardNumber; //actually question number, that's 11PM coding for you
    private ControllerState state;
    private float elapsed;

	// Use this for initialization
	void Start ()
    {
        //set state and elapsed
        state = ControllerState.Opening;
        elapsed = 0f;

        //grab card number
        if(PlayerPrefs.HasKey(QUESTION_NUMBER_VARNAME))
        {
            cardNumber = PlayerPrefs.GetInt(QUESTION_NUMBER_VARNAME);
        }
        else
        {
            cardNumber = 0;
        }

        //fill question text annd background
        QuestionText.text = QUESTIONS[cardNumber];
        //BackgroundObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(BACK_RESOURCE_FMTSTRING,cardNumber));

        //fill quads (please don't shoot me)
        Card1.transform.Find("Surf1").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 1, 1));
        Card1.transform.Find("Surf2").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 1, 2));

        //Card2.transform.Find("Surf1").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 2, 1));
        //Card2.transform.Find("Surf2").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 2, 2));

        //Card3.transform.Find("Surf1").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 3, 1));
        //Card3.transform.Find("Surf2").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 3, 2));
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (state)
        {
            case ControllerState.Opening:
                //opening: play animations and switch to presented state
                break;
            case ControllerState.QuestionsPresented:
                //questions presented: await player input

                break;
            case ControllerState.AnswerChosen:
                //answer chosen: flip cards and await

                break;
            case ControllerState.AnswersRevealed:
                //answers revealed: card back showing, reveal continue button (if not revealed) and await player input

                break;
            case ControllerState.ContinueSelected:
                //continue button pressed: play closing animation and await

                break;
            case ControllerState.Done:
                //we're done so just end the scene already
                PlayerPrefs.SetInt(QUESTION_NUMBER_VARNAME, ++cardNumber);
                SceneManager.LoadScene(MAIN_SCENE_NAME);
                break;
            default:
                throw new WhatTheFuckException();
        }
    }

    public void ClickCardButton(int cardNum)
    {
        Debug.Log("Clicked card : " + cardNum);
    }

    public void ClickContinueButton()
    {
        Debug.Log("Clicked continue button");
    }

}

internal class WhatTheFuckException : System.Exception
{

}
