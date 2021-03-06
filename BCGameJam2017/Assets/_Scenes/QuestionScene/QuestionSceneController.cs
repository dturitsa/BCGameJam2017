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
            Cards[1] = card1;
            Cards[2] = card2;
            Cards[3] = card3;
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

        public int GetTotalBadness()
        {
            return CO2Add + MethaneAdd + NosAdd;
        }
    }

    //portability or something
    const float CARBON_GAIN_RATE = 1.0f;
    const string MAIN_SCENE_NAME = "GameplayScene";
    //const string CO2_COUNT_VARNAME = "CO2Count";
    //const string METHANE_COUNT_VARNAME = "MethaneCount";
    //const string NOS_COUNT_VARNAME = "NosCount";
    //const string QUESTION_NUMBER_VARNAME = "QuestionsComplete"; //0-indexed
    const string BACK_RESOURCE_FMTSTRING = "Questions/Question{0}Background";
    const string CARD_RESOURCE_FMTSTRING = "Questions/Question{0}Card{1}Side{2}";


    //I believe I speak for us all when I say to hell with proper datastructures
    //Frank would murder me, oh well
    //private readonly string[] QUESTIONS = {"What is the correct answer?","",""};

    private readonly QuestionStruct[] QUESTIONS =   { new QuestionStruct("What is the correct answer?", new CardStruct(2,0,0), new CardStruct(0,0,0), new CardStruct(0,0,0)),
                                                      new QuestionStruct("Raj dropped his phone and shattered the screen! \n What should he do with it now?", new CardStruct(1,0,0), new CardStruct(2,0,0), new CardStruct(3,0,0)),
                                                      new QuestionStruct("Suzy lives far away from school and needs a car to drive there... \n What kind of car should she buy?", new CardStruct(4,0,2), new CardStruct(8,0,0), new CardStruct(2,0,0)),
                                                      new QuestionStruct("Chen is planting a garden now that spring has sprung \n What kind of plants should he grow?", new CardStruct(-2,0,0), new CardStruct(-4,0,0), new CardStruct(-3,0,0)),
                                                      new QuestionStruct("Farmer Joe is contemplating what he will produce on his farm... \n What would be a good choice for him?", new CardStruct(6,6,0), new CardStruct(6,2,0), new CardStruct(4,0,0)),
                                                      new QuestionStruct("Janet is the CEO for a big company responsible for providing power to the city... \n How should she plan to provide power?", new CardStruct(5,0,0), new CardStruct(4,0,0), new CardStruct(20,0,5))

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
    public Text CarbonText;

    public AudioSource CardFlipSound;
    public AudioSource CardSlideSound;
    public AudioSource MusicSound;
    public AudioSource ButtonSound;

    private int animationState;
    private int cardNumber; //actually question number, that's 11PM coding for you
    private int selectedCard;
    private ControllerState state;
    private float elapsed;
    private PersistantData persistantData;

    // Use this for initialization
    void Start ()
    {
        //for testing; TODO move to main menu
        //PlayerPrefs.DeleteAll();
        persistantData = (PersistantData)FindObjectOfType(typeof(PersistantData));

        //set state and elapsed
        state = ControllerState.Opening;
        elapsed = 0f;

        //grab card number
        if(persistantData.questionNumber > 0)
        {
            cardNumber = persistantData.questionNumber;
        }
        else
        {
            cardNumber = 1;
            persistantData.questionNumber = 1;
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

        persistantData.firstTimePlaying = false;
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

                    activeCard.transform.Translate(Vector3.back * 20.0f * Time.deltaTime, Space.World);

                    if(elapsed >= 0.25f)
                    {
                        animationState++;
                        elapsed = 0;
                        CardFlipSound.Play();
                    }

                }
                else if(animationState == 1)
                {
                    //flip
                    float rotateSpeed = 180f;

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

                if(!CarbonText.gameObject.activeSelf)
                {
                    //fill and fade in  carbon text
                    FillAndFadeCarbonText();
                }

                break;
            case ControllerState.ContinueSelected:
                //continue button pressed: play closing animation and await

                //TODO: push card up and fadeout
                if(animationState == 0)
                {
                    //push card up
                    var activeCard = Cards[selectedCard];

                    activeCard.transform.Translate(Vector3.up * 40.0f * Time.deltaTime, Space.World);

                    if (elapsed >= 0.5f)
                    {
                        animationState++;
                        elapsed = 0;
                        StartFade();
                        StartCoroutine(FadeOutMusicCoroutine());
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

                //Debug.Log(QUESTIONS[cardNumber].Question);
                //Debug.Log(QUESTIONS[cardNumber].Cards[selectedCard].CO2Add);

                //persistantData.carbonDioxideCounter += Mathf.CeilToInt((float)cardNumber * CARBON_GAIN_RATE);
                persistantData.carbonDioxideCounter += Mathf.CeilToInt(CARBON_GAIN_RATE);

                persistantData.carbonDioxideCounter += QUESTIONS[cardNumber].Cards[selectedCard].CO2Add;
                if (persistantData.carbonDioxideCounter < 0)
                    persistantData.carbonDioxideCounter = 0;
                persistantData.methaneCounter += QUESTIONS[cardNumber].Cards[selectedCard].MethaneAdd;
                if (persistantData.methaneCounter < 0)
                    persistantData.methaneCounter = 0;
                persistantData.n2oCounter += QUESTIONS[cardNumber].Cards[selectedCard].NosAdd;
                if (persistantData.n2oCounter < 0)
                    persistantData.n2oCounter = 0;
                persistantData.questionNumber += 1;

                
                //Debug.Log(persistantData.carbonDioxideCounter);

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
        ButtonSound.Play();
        CardSlideSound.Play();
        state = ControllerState.ContinueSelected;
    }

    const float CARBON_FADE_TIME = 0.5f;

    private void FillAndFadeCarbonText()
    {
        var text = CarbonText;
        string str;
        int co2 = QUESTIONS[cardNumber].Cards[selectedCard].CO2Add;
        int nos = QUESTIONS[cardNumber].Cards[selectedCard].NosAdd;
        int meth = QUESTIONS[cardNumber].Cards[selectedCard].MethaneAdd;

        int totalBadness = co2 + nos + meth;

        if (co2 > 0)
            str = string.Format("CO2: +{0}", co2);
        else
            str = string.Format("CO2: {0}", co2);

        if (nos > 0)
            str += string.Format(", N2O: +{0}", nos);
        if (meth > 0)
            str += string.Format(", Methane: +{0}", meth);

        text.text = str;
        text.gameObject.SetActive(true);
        text.canvasRenderer.SetAlpha(0.01f);
        if(co2 < 0)
        {
            text.color = Color.green;
        }
        else if(totalBadness >= QUESTIONS[cardNumber].Cards[1].GetTotalBadness() && totalBadness >= QUESTIONS[cardNumber].Cards[2].GetTotalBadness() && totalBadness >= QUESTIONS[cardNumber].Cards[3].GetTotalBadness())
        {
            text.color = Color.red;
        }
        else if (totalBadness <= QUESTIONS[cardNumber].Cards[1].GetTotalBadness() && totalBadness <= QUESTIONS[cardNumber].Cards[2].GetTotalBadness() && totalBadness <= QUESTIONS[cardNumber].Cards[3].GetTotalBadness())
        {
            text.color = Color.blue;
        }
        else
        {
            text.color = Color.black;
        }
        text.CrossFadeAlpha(1.0f, CARBON_FADE_TIME, false);


    }

    const float FADE_TIME = 1.0f;

    private void StartFade()
    {
        var fadeImg = FadeCanvas.transform.FindChild("Image").gameObject.GetComponent<Image>();

        fadeImg.canvasRenderer.SetAlpha(0f);
        FadeCanvas.gameObject.SetActive(true);
        fadeImg.CrossFadeAlpha(1.0f, FADE_TIME, false);
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        float elapsed = 0.0001f;
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime;
            MusicSound.volume = 1.0f - elapsed; //this works as long as the fade time is one second
            //Debug.Log(FADE_TIME - elapsed);
            yield return new WaitForEndOfFrame();
        }

    }

}

internal class WhatTheFuckException : System.Exception
{

}
