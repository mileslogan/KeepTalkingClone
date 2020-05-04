using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float countdowntime;//enter the time of the bomb in the inspector
    public static float timeleft;// how many time left
    int minutes; // the number representation of minutes
    int seconds;//the number representation of seconds
    public string str_minutes; //the string representation of minutes
    public string str_seconds;//the string representation of seconds
    public float waittime = 1f;

    public GenerateBomb BombScript;
    public GameObject timeText; // Test of the time 
    public GameObject strikeScreen;
     
    public Material timerStrike0; // The material of no strikes 
    public Material timerStrike1;// The material of 1 strikes 
    public Material timerStrike2;// The material of 2 strikes 

    public AudioSource audioData; // The audio source of the sounds of timer 

    bool blink = false;
    void Start()
    {
        //start timer
        StartCoroutine(CountDown(countdowntime));
        BombScript = FindObjectOfType<GenerateBomb>();

    }

    
    // Update the strike shows on the top
    void Update()
    {
        if (BombScript!= null && BombScript.CurrentStrikes == 1)
        {
            Material[] matArray = strikeScreen.GetComponent<MeshRenderer>().materials;
            matArray[1] = timerStrike1;
            strikeScreen.GetComponent<MeshRenderer>().materials = matArray;
            waittime = 0.75f;
        }
        if (BombScript != null && BombScript.CurrentStrikes == 2 && !blink)
        {
            blink = true;
            StartCoroutine(Blink());
            waittime = 0.5f;
        }

    }
    
    //timer count donw
    public IEnumerator CountDown(float timer)
    {
        timeleft = timer;
        while (timeleft > 0)
        {
            TimeCalculator();
            timeText.GetComponent<TextMesh>().text = str_minutes + " : " + str_seconds;
            //timeText.GetComponent<TextMeshPro>().text = str_minutes + " : " + str_seconds;
            audioData.Play(0);
            yield return new WaitForSeconds(waittime);
            timeleft--;
        }

    }

    public IEnumerator Blink()
    {
        Material[] matArray = strikeScreen.GetComponent<MeshRenderer>().materials;
        while (true)
        {
            matArray[1] = timerStrike2;
            strikeScreen.GetComponent<MeshRenderer>().materials = matArray;
            yield return new WaitForSeconds(0.5f);
            matArray[1] = timerStrike0;
            strikeScreen.GetComponent<MeshRenderer>().materials = matArray;
            yield return new WaitForSeconds(0.5f);
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
