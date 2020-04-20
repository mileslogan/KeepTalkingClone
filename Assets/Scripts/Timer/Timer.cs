using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float countdowntime;//enter the time of the bomb in the inspector
    float timeleft;// how many time left
    int minutes; // the number representation of minutes
    int seconds;//the number representation of seconds
    string str_minutes; //the string representation of minutes
    string str_seconds;//the string representation of seconds

    public GenerateBomb BombScript;
    public GameObject timeText; // Test of the time 
    public GameObject strikesText; // Test of the strikes

    void Start()
    {
        //start timer
        StartCoroutine(CountDown(countdowntime));
        BombScript = FindObjectOfType<GenerateBomb>();
    }

    
    // Update the strike shows on the top
    void Update()
    { 
        if (BombScript.CurrentStrikes == 1)
        {
            strikesText.GetComponent<TextMesh>().text = "x";
        }
        if (BombScript.CurrentStrikes == 2)
        {
            strikesText.GetComponent<TextMesh>().text = "x  x";
        }

    }
    
    //timer count donw
    private IEnumerator CountDown(float timer)
    {
        timeleft = timer;
        while (timeleft > 0)
        {
            TimeCalculator();
            timeText.GetComponent<TextMesh>().text = str_minutes + " : " + str_seconds;
            yield return new WaitForSeconds(1.0f);
            timeleft--;
        }

    }

    // convert time to string 
    void TimeCalculator()
    {
        minutes = (int)timeleft / 60;
        seconds = (int)timeleft % 60;
        str_minutes = "" + minutes;
        str_seconds = "" + seconds;
        if (minutes < 10)
        {
            str_minutes = "0" + minutes;
        }
        if (seconds < 10)
        {
            str_seconds = "0" + seconds;
        }
    }
}
