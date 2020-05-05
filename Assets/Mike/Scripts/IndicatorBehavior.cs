using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IndicatorBehavior : MonoBehaviour
{
    
    public int IndicatorIndex; //which index it is referring to, used to respawn self if colliding with another object
    public TextMeshPro TopSideText;
    public TextMeshPro FlipSideText;
    public Light FrontLight;
    public Light BackLight;

    //set 3 letter word on indicator
    public void SetText(string text)
    {
        TopSideText.text = text;

        if (FlipSideText != null)
        {
            FlipSideText.text = text;
        }
        
    }

    //set light on or off-to be replaced with model that has a light on it
    public void SetLight(bool isOn)
    {
        FrontLight.enabled = isOn;
        if (BackLight != null)
        {
            BackLight.enabled = isOn;
        }
        
    }
}
