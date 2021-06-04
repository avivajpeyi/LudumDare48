using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class InstructionTextManager : MonoBehaviour
{
    private TMP_Text _textBox;

    private string ClickStr = "Click to Charge";
    private string releaseToShootStr = "Release";
    private string rollingStr = "Invincible roll!";

    public bool textChanging = false;
    private ClickController _player;

    void Start()
    {
        _textBox = GetComponent<TMP_Text>();
        _player = FindObjectOfType<ClickController>();
        SwitchText(0, ClickStr);
    }

    // Update is called once per frame
    void Update()
    {
        if (!textChanging)
        {
            if (_player._canDash)
                SwitchText(0.1f, releaseToShootStr);
            else if (_player.isDashing)
                SwitchText(0.1f, rollingStr);
            else 
                SwitchText(0, ClickStr);
        }
    }

    void SwitchText(float transitionTime, string newText)
    {
        textChanging = true;
        _textBox.DOFade(0, transitionTime); // fade current text out
        _textBox.text = newText;
        _textBox.DOFade(255, transitionTime); // fade new text in
        textChanging = false;

    }
}
