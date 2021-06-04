using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LawsOfRobotics : MonoBehaviour
{
    public float fadeInDuration = 4.0f;
    public float fadeOutDuration = 2.0f;
    public int fadeLevelOut = 1;

    private string[] lawTitleStrings =
    {
        "<alpha=#{0}>First Law",
        "<alpha=#{0}>Second Law",
        "<alpha=#{0}>Third Law",
    };

    private string[] lawContentStrings =
    {
        "<alpha=#{1}>A robot may <alpha=#{0}>not injure a human being or, through inaction, allow a human being to come to <alpha=#{1}>harm.",
        "<alpha=#{0}>A robot must obey the orders given it by human beings except where such orders would conflict with the First Law.",
        "<alpha=#{0}>A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.",
    };

    private string mainTitleString = "<alpha=#{0}>Laws of Robotics";
    private GlitchEffect _glitchEffect;
    private Fading  _fader;
    
    public GameObject[] LawContainers;

    private List<TMP_Text> lawContentTMP = new List<TMP_Text>();
    private List<TMP_Text> lawTitleTMP = new List<TMP_Text>();
    private TMP_Text mainTitleTMP;

    // Start is called before the first frame update
    void Start()
    {
        SetInitialReferences();
        SetTextValues(0, 0);
        StartCoroutine(ChangeLawAnimation());
        StartCoroutine(BeginGlitching());
       
    }

    IEnumerator BeginGlitching()
    {
        yield return new WaitForSeconds(fadeInDuration*2.0f/3.0f);
        _glitchEffect.enabled = true;
    }

    void SetTextValues(int mainAlpha, int metaAlpha)
    {
        for (int i = 0; i < lawContentTMP.Count; i++)
        {
            SetLaw(i, mainAlpha, metaAlpha);
        }

        SetTitle(mainAlpha);
    }

    void SetInitialReferences()
    {
        if (LawContainers.Length != lawContentStrings.Length)
        {
            Debug.LogError("There must be 1 textbox for each laws");
        }

        for (int i = 0; i < LawContainers.Length; i++)
        {
            Transform container = LawContainers[i].transform;
            TMP_Text lawTitle =
                container.Find("LawPanel/Text (TMP)").GetComponent<TMP_Text>();
            TMP_Text lawContent = container.Find("ContentPanel/Text (TMP)")
                .GetComponent<TMP_Text>();
            lawTitleTMP.Add(lawTitle);
            lawContentTMP.Add(lawContent);
        }

        mainTitleTMP = transform.Find("Panel/TitlePanel/Text (TMP)")
            .GetComponent<TMP_Text>();

        _glitchEffect = FindObjectOfType<GlitchEffect>();
        _glitchEffect.enabled = false;

        _fader = FindObjectOfType<Fading>();
    }


    void SetTitle(int alpha)
    {
        string mainHex = alpha.ToString("X2");
        mainTitleTMP.text = string.Format(mainTitleString, mainHex);
    }

    void SetLaw(int lawIdx, int mainTextAlpha, int metaTextAlpha)
    {
        string mainHex = ((int) mainTextAlpha).ToString("X2");
        string metaHex = ((int) metaTextAlpha).ToString("X2");
        string currentLaw = string.Format(lawContentStrings[lawIdx], mainHex, metaHex);
        string currentLawTitle = string.Format(lawTitleStrings[lawIdx], mainHex);
        lawTitleTMP[lawIdx].text = currentLawTitle;
        lawContentTMP[lawIdx].text = currentLaw;
    }

    IEnumerator ChangeLawAnimation()
    {
        yield return StartCoroutine(AlphaLerpAllContents(fadeInDuration, 0, 255));
        yield return StartCoroutine(AlphaLerpMainContents(fadeOutDuration, 255, 0));
        StartCoroutine(StartLevelChange());
    }

    IEnumerator StartLevelChange()
    {
        Debug.Log("Begin level fade");
        _fader.BeginFade(fadeLevelOut);
        yield return new WaitForSeconds(fadeLevelOut);
        SceneManager.LoadScene("Game");
    }


    IEnumerator AlphaLerpAllContents(float lerpDuration, int startAlpha, int endAlpha)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            int lerpAlpha =
                (int) Mathf.Lerp(startAlpha, endAlpha, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            SetTextValues(lerpAlpha, lerpAlpha);

            yield return null;
        }

        SetTextValues(endAlpha, endAlpha);
    }


    IEnumerator AlphaLerpMainContents(float lerpDuration, int startAlpha,
        int endAlpha)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            int lerpAlpha =
                (int) Mathf.Lerp(startAlpha, endAlpha, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            SetTextValues(lerpAlpha, startAlpha);

            yield return null;
        }

        SetTextValues(endAlpha, startAlpha);
    }
}