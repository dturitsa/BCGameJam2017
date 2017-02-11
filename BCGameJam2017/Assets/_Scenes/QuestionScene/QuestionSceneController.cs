﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionSceneController : MonoBehaviour
{ 

    //TODO move structs to separate file
    private struct QuestionStruct
    {
        public string Question;
        public CardStruct[] Cards;

        public QuestionStruct(string question, CardStruct card1, CardStruct card2, CardStruct card3)
        {
            Question = question;
            Cards = new CardStruct[4];
        }
    }

    private struct CardStruct
    {
        public int CO2Add;
        public int MethaneAdd;
        public int NosAdd;

        public CardStruct(int co2add, int methaneadd, int nosadd)
        {
            CO2Add = co2add;
            MethaneAdd = methaneadd;
            NosAdd = nosadd;
        }
    }

    //portability or something
    const string MAIN_SCENE_NAME = "GameScene";
    const string CO2_COUNT_VARNAME = "CO2Count";
    const string METHANE_COUNT_VARNAME = "MethaneCount";
    const string NOS_COUNT_VARNAME = "NosCount";
    const string QUESTION_NUMBER_VARNAME = "QuestionsComplete"; //0-indexed
    const string BACK_RESOURCE_FMTSTRING = "Questions/Question{0}Background";
    const string CARD_RESOURCE_FMTSTRING = "Questions/Question{0}Card{1}Side{2}";


    //I believe I speak for us all when I say to hell with proper datastructures
    //Frank would murder me, oh well
    //private readonly string[] QUESTIONS = {"What is the correct answer?","",""};

    private readonly QuestionStruct[] QUESTIONS =   { new QuestionStruct("What is the correct answer?", new CardStruct(2,0,0), new CardStruct(0,0,0), new CardStruct(0,0,0)),
                                                      new QuestionStruct("What is the correct answer (Q1)?", new CardStruct(2,0,0), new CardStruct(0,0,0), new CardStruct(0,0,0)),
                                                      new QuestionStruct("Farmer Joe is contemplating what he will produce on his farm... \n What can he produce that will have the smallest contribution to green house gas emissions?", new CardStruct(0,5,0), new CardStruct(0,5,0), new CardStruct(0,3,0))
                                                    };

    enum ControllerState
    {
        Opening, QuestionsPresented, AnswerChosen, AnswersRevealed, ContinueSelected, Done
    }

    //public GameObject Card1;
    //public GameObject Card2;
    //public GameObject Card3;
    public GameObject[] Cards;
    public Text QuestionText;
    public GameObject ContinueButton;
    public GameObject BackgroundObject;
    public Canvas FadeCanvas;

    private int animationState;
    private int cardNumber; //actually question number, that's 11PM coding for you
    private int selectedCard;
    private ControllerState state;
    private float elapsed;

	// Use this for initialization
	void Start ()
    {
        //for testing; TODO move to main menu
        PlayerPrefs.DeleteAll();

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

        //testing
        //cardNumber = 2;

        //fill question text annd background
        QuestionText.text = QUESTIONS[cardNumber].Question;
        BackgroundObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(BACK_RESOURCE_FMTSTRING,cardNumber));

        //fill quads (please don't shoot me)
        Cards[1].transform.Find("Surf1").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 1, 1));
        Cards[1].transform.Find("Surf2").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 1, 2));

        Cards[2].transform.Find("Surf1").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 2, 1));
        Cards[2].transform.Find("Surf2").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 2, 2));

        Cards[3].transform.Find("Surf1").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 3, 1));
        Cards[3].transform.Find("Surf2").gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(string.Format(CARD_RESOURCE_FMTSTRING, cardNumber, 3, 2));

        state = ControllerState.Opening;

    }
	
	// Update is called once per frame
	void Update ()
    {
        elapsed += Time.deltaTime;

        switch (state)
        {
            case ControllerState.Opening:
                //opening: play animations and switch to presented state
                state = ControllerState.QuestionsPresented;
                break;
            case ControllerState.QuestionsPresented:
                //questions presented: await player input

                break;
            case ControllerState.AnswerChosen:
                //answer chosen: flip cards and await

                if(animationState == 0)
                {
                    //push
                    var activeCard = Cards[selectedCard];

                    activeCard.transform.Translate(Vector3.back * 10.0f * Time.deltaTime, Space.World);

                    if(elapsed >= 0.5f)
                    {
                        animationState++;
                        elapsed = 0;
                    }

                }
                else if(animationState == 1)
                {
                    //flip
                    float rotateSpeed = 120f;

                    Debug.Log(Cards[1].transform.eulerAngles.y);

                    Cards[1].transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
                    Cards[2].transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
                    Cards[3].transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

                    if (Cards[1].transform.eulerAngles.y >= 358 || Cards[1].transform.eulerAngles.y <= 2)
                    {
                        state = ControllerState.AnswersRevealed;
                        animationState = 0;
                        elapsed = 0;
                    }
                    
                }

                break;
            case ControllerState.AnswersRevealed:
                //answers revealed: card back showing, reveal continue button (if not revealed) and await player input

                if(!ContinueButton.activeSelf)
                {
                    ContinueButton.SetActive(true);
                }

                break;
            case ControllerState.ContinueSelected:
                //continue button pressed: play closing animation and await

                //TODO: push card up and fadeout
                if(animationState == 0)
                {
                    //push card up
                    var activeCard = Cards[selectedCard];

                    activeCard.transform.Translate(Vector3.up * 20.0f * Time.deltaTime, Space.World);

                    if (elapsed >= 1.0f)
                    {
                        animationState++;
                        elapsed = 0;
                        StartFade();
                    }                    

                }
                else if(animationState == 1)
                {
                    //fadeout

                    if(elapsed > FADE_TIME)
                    {
                        animationState = 0;
                        elapsed = 0;
                        state = ControllerState.Done;
                    }
                }

                //state = ControllerState.Done;

                break;
            case ControllerState.Done:
                //we're done so just end the scene already
                PlayerPrefs.SetInt(CO2_COUNT_VARNAME, QUESTIONS[cardNumber].Cards[selectedCard].CO2Add + PlayerPrefs.GetInt(CO2_COUNT_VARNAME));
                PlayerPrefs.SetInt(METHANE_COUNT_VARNAME, QUESTIONS[cardNumber].Cards[selectedCard].MethaneAdd + PlayerPrefs.GetInt(METHANE_COUNT_VARNAME));
                PlayerPrefs.SetInt(NOS_COUNT_VARNAME, QUESTIONS[cardNumber].Cards[selectedCard].NosAdd + PlayerPrefs.GetInt(NOS_COUNT_VARNAME));
                PlayerPrefs.SetInt(QUESTION_NUMBER_VARNAME, ++cardNumber);
                SceneManager.LoadScene(MAIN_SCENE_NAME);
                break;
            default:
                throw new WhatTheFuckException();
        }
    }

    public void ClickCardButton(int cardNum)
    {
        if(state != ControllerState.QuestionsPresented)
        {
            return;
        }

        Debug.Log("Clicked card : " + cardNum);
        selectedCard = cardNum;
        elapsed = 0;
        state = ControllerState.AnswerChosen;
    }

    public void ClickContinueButton()
    {
        if(state != ControllerState.AnswersRevealed)
        {
            throw new WhatTheFuckException();
        }

        Debug.Log("Clicked continue button");
        elapsed = 0;
        state = ControllerState.ContinueSelected;
    }

    const float FADE_TIME = 1.0f;

    private void StartFade()
    {
        var fadeImg = FadeCanvas.transform.FindChild("Image").gameObject.GetComponent<Image>();

        fadeImg.canvasRenderer.SetAlpha(0f);
        FadeCanvas.gameObject.SetActive(true);
        fadeImg.CrossFadeAlpha(1.0f, FADE_TIME, false);
    }

}

internal class WhatTheFuckException : System.Exception
{

}