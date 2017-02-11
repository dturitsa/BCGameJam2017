using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//very bad!
public class IntroSceneController : MonoBehaviour
{
    const float FADE_TIME = 1.0f;
    const string NEXT_SCENE = "MainMenuScene";
    const KeyCode SKIP_KEY = KeyCode.Space;

    const float INTRO_STAY_TIME = 5.0f;
    const float INTRO_SLIDE_DELAY = 2.5f;
    const float INTRO_POSTROLL_DELAY = 1.0f;

    public GameObject Camera;
    public GameObject Intro1EntityGroup;
    public GameObject Intro2EntityGroup;
    public GameObject Intro3EntityGroup;
    public Text Intro1Text;
    public Text Intro2Text;
    public Text Intro3Text;

    public Canvas FadeCanvas;
    public AudioSource MusicSound;

    int introSlide;
    int animationState;

	void Start ()
    {
        StartInvertedFade();
        StartCoroutine(AwaitInitialFadeCoroutine());
	}
	
	void Update ()
    {
        Debug.Log("Intro slide: " + introSlide + " , Animation State: " + animationState);
        //on click advance? await initial fadein?

		//general sequence:

        //first fade in (do this before)
        //await fadein
        if(introSlide == 0)
        {
            return;
        }

        //onclick advance?

        //then pan to first slide (push camera down slightly, push entities up)
        if(introSlide == 1)
        {
            if(animationState == 0)
            {
                float time = 1.5f;
                

                //animationState is 0, start routine
                StartCoroutine(PushCameraDownCoroutine(Camera,1.0f,time));
                StartCoroutine(PushObjectsUpCoroutine(Intro1EntityGroup,5.5f, time));
                StartCoroutine(AwaitAnimationFinished(time));
                FadeTextOn(Intro1Text, time);

                animationState = 1;
            }
            else if(animationState == 1)
            {
                //awaiting objects moving in
            }
            else if(animationState == 2)
            {
                //objects are on screen but waiting hasn't begun

                StartCoroutine(AwaitAnimationFinished(INTRO_STAY_TIME));

                animationState = 3;
                
            }
            else if(animationState == 3)
            {
                //objects are on screen, await timeout or skip
                if(Input.GetKeyDown(SKIP_KEY))
                {
                    StopAllCoroutines();
                    animationState = 4;
                }

            }
            else if(animationState == 4)
            {
                //await is ended, start pushing offscreen
                float time = 1.5f;

                //StartCoroutine(PushCameraDownCoroutine(Camera, 1.0f, time));
                StartCoroutine(PushObjectsUpCoroutine(Intro1EntityGroup, 10.0f, time));
                StartCoroutine(AwaitAnimationFinished(time));
                FadeTextOff(Intro1Text, time);

                animationState = 5;
            }
            else if(animationState == 5)
            {
                //awaiting objects moving offscreen
            }
            else if(animationState == 6)
            {
                //objects are offscreen, await and advance to next slide
                //introSlide++;

                StartCoroutine(AwaitSlideFinished(INTRO_SLIDE_DELAY));
                animationState = 7;
            }
        }

        //then pan to next slide (intro2)
        else if(introSlide == 2)
        {
            if (animationState == 0)
            {
                float time = 1.5f;


                //animationState is 0, start routine
                Intro2EntityGroup.SetActive(true);
                StartCoroutine(PushCameraDownCoroutine(Camera, 1.0f, time));
                StartCoroutine(AwaitAnimationFinished(time));
                FadeTextOn(Intro2Text, time);

                animationState = 1;
            }
            else if (animationState == 1)
            {
                //awaiting objects moving in
            }
            else if (animationState == 2)
            {
                //objects are on screen but waiting hasn't begun

                StartCoroutine(AwaitAnimationFinished(INTRO_STAY_TIME));

                animationState = 3;

            }
            else if (animationState == 3)
            {
                //objects are on screen, await timeout or skip
                if (Input.GetKeyDown(SKIP_KEY))
                {
                    StopAllCoroutines();
                    animationState = 4;
                }

            }
            else if (animationState == 4)
            {
                //await is ended, start pushing offscreen
                float time = 1.5f;

                //StartCoroutine(PushCameraDownCoroutine(Camera, 1.0f, time));
                //StartCoroutine(PushObjectsUpCoroutine(Intro1EntityGroup, 10.0f, time));
                Intro2EntityGroup.GetComponentInChildren<ParticleSystem>().Stop();
                StartCoroutine(AwaitAnimationFinished(time));
                FadeTextOff(Intro2Text, time);

                animationState = 5;
            }
            else if (animationState == 5)
            {
                //awaiting objects moving offscreen
            }
            else if (animationState == 6)
            {
                //objects are offscreen, await and advance to next slide
                //introSlide++;

                StartCoroutine(AwaitSlideFinished(INTRO_SLIDE_DELAY));
                animationState = 7;
            }
        }

        //then pan to last slide (intro3)
        else if(introSlide == 3)
        {
            if (animationState == 0)
            {
                float time = 1.5f;


                //animationState is 0, start routine
                Intro3EntityGroup.SetActive(true);
                StartCoroutine(PushCameraDownCoroutine(Camera, 1.0f, time));
                StartCoroutine(ScaleObjectsCoroutine(Intro3EntityGroup, 0.5f, time));
                StartCoroutine(AwaitAnimationFinished(time));
                FadeTextOn(Intro3Text, time);

                animationState = 1;
            }
            else if (animationState == 1)
            {
                //awaiting objects moving in
            }
            else if (animationState == 2)
            {
                //objects are on screen but waiting hasn't begun

                StartCoroutine(AwaitAnimationFinished(INTRO_STAY_TIME));

                animationState = 3;

            }
            else if (animationState == 3)
            {
                //objects are on screen, await timeout or skip
                if (Input.GetKeyDown(SKIP_KEY))
                {
                    StopAllCoroutines();
                    animationState = 4;
                }

            }
            else if (animationState == 4)
            {
                //await is ended, start pushing offscreen
                float time = 1.5f;

                //TODO: disappear the molecules
                StartCoroutine(AwaitAnimationFinished(time));
                StartCoroutine(PushObjectsUpCoroutine(Intro3EntityGroup, 10.0f, time));
                FadeTextOff(Intro3Text, time);

                animationState = 5;
            }
            else if (animationState == 5)
            {
                //awaiting objects moving offscreen
            }
            else if (animationState == 6)
            {
                //objects are offscreen, await and advance to next slide
                //introSlide++;
                
                StartCoroutine(AwaitSlideFinished(INTRO_POSTROLL_DELAY));
                animationState = 7;
            }
        }

        //finally, pan down/fadeout
        else if(introSlide == 4)
        {
            //fade to white
            StartFade();
            StartCoroutine(AwaitSlideFinished(FADE_TIME));
            StartCoroutine(FadeOutMusicCoroutine());
            ++introSlide;
        }
        //if 5 do nothing
        else if(introSlide == 6)
        {
            SceneManager.LoadScene(NEXT_SCENE);
        }
	}

    void FadeTextOn(Text text, float time)
    {
        text.gameObject.SetActive(true);
        text.canvasRenderer.SetAlpha(0.01f);
        text.CrossFadeAlpha(1.0f, time, false);
    }

    void FadeTextOff(Text text, float time)
    {
        text.CrossFadeAlpha(0f, time, false);
        //text.gameObject.SetActive(false);
    }
    
    IEnumerator ScaleObjectsCoroutine(GameObject objectGroup, float rate, float time)
    {
        float elapsed = 0;
        Vector3 baseScale = objectGroup.transform.localScale;
        do
        {
            elapsed += Time.deltaTime;
            objectGroup.transform.localScale = baseScale + Vector3.one * (elapsed * rate);

            yield return new WaitForEndOfFrame();
        } while (elapsed < time);
    }

    IEnumerator PushCameraDownCoroutine(GameObject camera, float speed, float time)
    {
        float elapsed = 0;
        do
        {
            elapsed += Time.deltaTime;
            //push!
            camera.transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

            yield return new WaitForEndOfFrame();
        } while (elapsed < time);
    }

    IEnumerator PushObjectsUpCoroutine(GameObject objectGroup, float speed, float time)
    {
        float elapsed = 0;
        do
        {
            elapsed += Time.deltaTime;
            //push!
            objectGroup.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);

            yield return new WaitForEndOfFrame();
        } while (elapsed < time);
    }

    IEnumerator AwaitAnimationFinished(float time)
    {
        yield return new WaitForSeconds(time);

        animationState++;
    }

    IEnumerator AwaitSlideFinished(float time)
    {
        yield return new WaitForSeconds(time);

        introSlide++;
        animationState = 0;
        
    }

    IEnumerator AwaitInitialFadeCoroutine()
    {
        yield return new WaitForSeconds(FADE_TIME);
        FadeCanvas.gameObject.SetActive(false);
        introSlide = 1;
        animationState = 0;
    }

    private void StartFade()
    {
        var fadeImg = FadeCanvas.transform.FindChild("Image").gameObject.GetComponent<Image>();

        fadeImg.canvasRenderer.SetAlpha(0f);
        FadeCanvas.gameObject.SetActive(true);
        fadeImg.CrossFadeAlpha(1.0f, FADE_TIME, false);
    }

    private void StartInvertedFade()
    {
        var fadeImg = FadeCanvas.transform.FindChild("Image").gameObject.GetComponent<Image>();

        fadeImg.canvasRenderer.SetAlpha(1.0f);
        FadeCanvas.gameObject.SetActive(true);
        fadeImg.CrossFadeAlpha(0f, FADE_TIME, false);
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
