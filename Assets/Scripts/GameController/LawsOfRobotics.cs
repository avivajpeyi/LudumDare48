using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class LawsOfRobotics : MonoBehaviour
{
    public bool testing = false;
    public bool lawChanged = false;

    private string[] lawTitleStrings =
    {
        "<alpha=#{0}>First Law",
        "<alpha=#{0}>Second Law",
        "<alpha=#{0}>Third Law",
    };

    private string[] lawContentStrings =
    {
        "<alpha=#{1}>A robot may <alpha=#{0}>not<alpha=#{1}> injure a human being or, through inaction, allow a human being to come to harm.",
        "<alpha=#{0}>A robot must obey the orders given it by human beings except where such orders would conflict with the First Law.",
        "<alpha=#{0}>A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.",
    };

    private string mainTitleString = "<alpha=#{0}>Laws of Robotics";


    public GameObject[] LawContainers;

    private List <TMP_Text> lawContentTMP = new List<TMP_Text>();
    private List <TMP_Text>  lawTitleTMP= new List<TMP_Text>();
    private TMP_Text mainTitleTMP;

    // Start is called before the first frame update
    void Start()
    {
        SetInitialReferences();
        SetTextValues(255, 255);
    }

    void SetTextValues(int mainAlpha, int metaAlpha)
    {
        for (int i = 0; i < lawContentTMP.Count; i++)
        {
            SetLaw(i, mainAlpha, metaAlpha );
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

        mainTitleTMP = transform.Find("Panel/TitlePanel/Text (TMP)").GetComponent<TMP_Text>();
    }


    void SetTitle(int alpha)
    {
        string mainHex = alpha.ToString("X2");
        mainTitleTMP.text = string.Format(mainTitleString, mainHex);
    }

    void SetLaw(int lawIdx, int mainTextAlpha,
        int metaTextAlpha)
    {
        string mainHex = mainTextAlpha.ToString("X2");
        string metaHex = metaTextAlpha.ToString("X2");
        string currentLaw = string.Format(lawContentStrings[lawIdx], mainHex, metaHex);
        string currentLawTitle = string.Format(lawTitleStrings[lawIdx], mainHex);
        lawTitleTMP[lawIdx].text = currentLawTitle;
        lawContentTMP[lawIdx].text = currentLaw;
    }

    void ChangeLawAnimation()
    {
        lawChanged = true;
        Debug.Log("Change Law");
        SetTextValues(0, 255);
    }


    private void Update()
    {
        if (testing && Input.anyKey && !lawChanged)
            ChangeLawAnimation();
    }
}