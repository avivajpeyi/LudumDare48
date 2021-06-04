using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGlitch : MonoBehaviour
{
    public float timeOn = 1.0f;
    public float timeOff = 20.0f;
    private GlitchEffect _glitch;
    private bool keepRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        _glitch = GetComponent<GlitchEffect>();
        _glitch.enabled = false;
        StartCoroutine(PeriodicallyToggleGlitch());
    }


// Start is called before the first frame update
    IEnumerator PeriodicallyToggleGlitch()
    {
        while (keepRunning)
        {
            yield return new WaitForSeconds(timeOff);
            _glitch.enabled = true;
            yield return new WaitForSeconds(timeOn);
            _glitch.enabled = false;
        }
    }
}