using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingSceneController : MonoBehaviour
{

    public Canvas FadeCanvas;
    public AudioSource ClickButtonSound;
    public AudioSource MusicSound;

    const float FADE_TIME = 1.0f;
    bool fading;

    // Use this for initialization
    void Start ()
    {
        StartInvertedFade();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ClickButtonReturn()
    {
        if (fading) return;

        fading = true;

        ClickButtonSound.Play();
        StartFade();
        StartCoroutine(FadeOutMusicCoroutine());
        StartCoroutine(LeaveSceneCoroutine("MainMenuScene"));
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        float elapsed = 0.0001f;
        while (elapsed < FADE_TIME)
        {
            elapsed += Time.deltaTime;
            MusicSound.volume = FADE_TIME - elapsed; //this works as long as the fade time is one second
            //Debug.Log(FADE_TIME - elapsed);
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator LeaveSceneCoroutine(string nextScene)
    {
        Debug.Log("started leaving scene");
        yield return new WaitForSeconds(FADE_TIME);
        Debug.Log("...and we're done!");
        SceneManager.LoadScene(nextScene);

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
}
