using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    private TMP_Text _text;
    public float duration=1; 
    
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        StartCoroutine(StartFade());

    }

    IEnumerator StartFade()
    {
        yield return new WaitForSeconds(duration);
        _text.DOFade(0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
