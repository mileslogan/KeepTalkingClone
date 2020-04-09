using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public Color red;
    public Color white;
    public Color yellow;
    public Color blue;
    public Color black;

    public Color currentColor;

    public Color stripColor0;
    public Color stripColor1;
    public Color stripColor2;

    public string detonate;
    public string abort;
    public string hold;
    public string press;

    public string currentString;

    int randomRoller;

    public bool complete;
    public bool pressing;

    public TextMesh buttonWords;

    // Start is called before the first frame update
    void Start()
    {
        complete = false;
        pressing = false;

        //Generate Color & Word on Bomb, and Strip Color for each strike
        currentColor = ButtonColorGen();

        currentString = ButtonTextGen();

        stripColor0 = StripColorGen();
        stripColor1 = StripColorGen();
        stripColor2 = StripColorGen();

        //Set Button Color and Text
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", currentColor);
        buttonWords.text = currentString;
    }

    // Update is called once per frame
    void Update()
    {
        //Functionaltiy based on what is on bomb and what color and word there is
        if (currentString == detonate)
        {
            //if there is more than one battery, press and immediately release
            //else, hold
        }
        else if (currentColor == red && currentString == hold)
        {
            //press and immediately release
        }
        //else if (lit indicator FRK)
        //if there are more than two batteries, press and immediately release
        //else, hold
        else
        {
            //holding instructions
        }
    }

    //Pick a color: Red, White, Yellow, Blue, or Black; Set Text Color to fit
    Color ButtonColorGen()
    {
        Color buttonColor;
        randomRoller = Random.Range(1, 6);
        if (randomRoller == 1)
        {
            buttonColor = red;
            buttonWords.color = white;
        }
        else if (randomRoller == 2)
        {
            buttonColor = white;
            buttonWords.color = black;
        }
        else if (randomRoller == 3)
        {
            buttonColor = yellow;
            buttonWords.color = black;
        }
        else if (randomRoller == 4)
        {
            buttonColor = blue;
            buttonWords.color = white;
        }
        else
        {
            buttonColor = black;
            buttonWords.color = white;
        }
        return buttonColor;
    }

    //Pick a color: Red, White, Yellow, or Blue
    Color StripColorGen()
    {
        Color genColor;
        randomRoller = Random.Range(1, 5);
        if (randomRoller == 1)
        {
            genColor = red;
        }
        else if (randomRoller == 2)
        {
            genColor = white;
        }
        else if (randomRoller == 3)
        {
            genColor = yellow;
        }
        else
        {
            genColor = blue;
        }
        return genColor;
    }

    //Pick one of the four text options
    string ButtonTextGen()
    {
        string buttonText;
        randomRoller = Random.Range(1, 5);
        if (randomRoller == 1)
        {
            buttonText = detonate;
        }
        else if (randomRoller == 2)
        {
            buttonText = abort;
        }
        else if (randomRoller == 3)
        {
            buttonText = hold;
        }
        else
        {
            buttonText = press;
        }
        return buttonText;
    }
}
