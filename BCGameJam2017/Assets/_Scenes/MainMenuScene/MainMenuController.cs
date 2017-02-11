using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    const string GAME_SCENE_NAME = "GameScene";
    const string ABOUT_SCENE_NAME = "AboutScene";

    public Canvas FadeCanvas;
    public AudioSource ClickButtonSound;
    public AudioSource MusicSound;

    bool fading;

	void Start ()
    {
        PlayerPrefs.DeleteAll();
        StartInvertedFade();
	}
	

	void Update ()
    {
		
	}

    public void ClickButtonStart()
    {
        if (fading) return;
        ClickButtonSound.Play();
        StartFade();
        StartCoroutine(FadeOutMusicCoroutine());
        StartCoroutine(LeaveSceneCoroutine(GAME_SCENE_NAME));
    }

    public void ClickButtonAbout()
    {
        if (fading) return;
        ClickButtonSound.Play();
        StartFade();
        StartCoroutine(LeaveSceneCoroutine(ABOUT_SCENE_NAME));

    }

    IEnumerator LeaveSceneCoroutine(string nextScene)
    {
        Debug.Log("started leaving scene");
        yield return new WaitForSeconds(FADE_TIME);
        Debug.Log("...and we're done!");
        SceneManager.LoadScene(nextScene);

    }

    IEnumerator LockAndReleaseCoroutine()
    {
        fading = true;
        yield return new WaitForSeconds(FADE_TIME);
        fading = false;
        FadeCanvas.gameObject.SetActive(false);
    }
    
    //ah, I love not needing forward declarations
    //it's great for doing stupid shit like this
    const float FADE_TIME = 1.0f;

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
        while(elapsed < FADE_TIME)
        {
            elapsed += Time.deltaTime;
            MusicSound.volume = FADE_TIME - elapsed; //this works as long as the fade time is one second
            //Debug.Log(FADE_TIME - elapsed);
            yield return new WaitForEndOfFrame();
        }
        
    }
}
