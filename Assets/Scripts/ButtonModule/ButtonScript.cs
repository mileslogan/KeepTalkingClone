using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public Color currentColor;

    public Color stripColor0;
    public Color stripColor1;
    public Color stripColor2;

    string detonate = "DETONATE";
    string abort = "ABORT";
    string hold = "HOLD";
    string press = "PRESS";

    public string currentString;

    int randomRoller;

    public bool complete;
    public bool pressing;

    public bool needHold;

    public TextMesh buttonWords;
    public GameObject buttonStrip;

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

        //Check if generated button needs to be pressed or held and released
        HoldOrPress();
    }

    // Update is called once per frame
    // CURRENTLY A WORK IN PROGRESS
    void Update()
    {
        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "BUTTON" && Input.GetKeyDown(KeyCode.Mouse0))
            {
                
            }
        }
    }

    //Pick a color: Red, White, Yellow, Blue, or Black; Set Text Color to fit
    Color ButtonColorGen()
    {
        Color buttonColor;
        randomRoller = Random.Range(1, 6);
        if (randomRoller == 1)
        {
            buttonColor = Color.red;
            buttonWords.color = Color.white;
        }
        else if (randomRoller == 2)
        {
            buttonColor = Color.white;
            buttonWords.color = Color.black;
        }
        else if (randomRoller == 3)
        {
            buttonColor = Color.yellow;
            buttonWords.color = Color.black;
        }
        else if (randomRoller == 4)
        {
            buttonColor = Color.blue;
            buttonWords.color = Color.white;
        }
        else
        {
            buttonColor = Color.black;
            buttonWords.color = Color.white;
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
            genColor = Color.red;
        }
        else if (randomRoller == 2)
        {
            genColor = Color.white;
        }
        else if (randomRoller == 3)
        {
            genColor = Color.yellow;
        }
        else
        {
            genColor = Color.blue;
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

    //Figure out whether the bomb needs to be held or pressed; WORK IN PROGESS
    void HoldOrPress()
    {
        //Functionaltiy based on what is on bomb and what color and word there is
        if (currentColor == Color.blue && currentString == abort)
        {
            needHold = true;
        }
        else if (currentString == detonate) //ALSO if there is more than one battery
        {
            needHold = false;
        }
        else if (currentColor == Color.white) //ALSO if lit indicator CAR
        {
            needHold = true;
        }
        //else if (lit indicator FRK && more than two batteries)
        //press and immediately release
        else if (currentColor == Color.yellow)
        {
            needHold = true;
        }
        else if (currentColor == Color.red && currentString == hold)
        {
            needHold = false;
        }
        else
        {
            needHold = true;
        }
    }
}
