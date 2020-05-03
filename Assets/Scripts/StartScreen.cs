using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartScreen : MonoBehaviour
{
    //Number of Modules to put on bomb
    public static int numOfModules;

    //Amount of time to defuse bomb IN SECONDS
    public static int timeForDefusal;

    public Animator binderAnim;
    public Collider binderCollider;

    public TextMeshPro moduleNum;
    public TextMeshPro timerNum;

    int minutes;
    int seconds;

    string minuteString;
    string secondString;

    public AudioSource clickSource;
    public AudioClip clickClip;

    void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    void Start()
    {
        numOfModules = 3;
        timeForDefusal = 300;

    }
    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if(binderAnim.GetBool("Opened") == false)
            {
                if (hit.collider.tag == "Binder" && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    binderCollider.enabled = false;
                    binderAnim.SetBool("Opened", true);
                }
            }
            else
            {
                if (hit.collider.tag == "StartModMinus" && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    clickSource.PlayOneShot(clickClip);
                    MinusModule();
                }
                else if(hit.collider.tag == "StartModPlus" && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    clickSource.PlayOneShot(clickClip);
                    PlusModule();
                }
                else if(hit.collider.tag == "StartTimeMinus" && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    clickSource.PlayOneShot(clickClip);
                    MinusTime();
                }
                else if(hit.collider.tag == "StartTimePlus" && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    clickSource.PlayOneShot(clickClip);
                    PlusTime();
                }
                else if(hit.collider.tag == "StartStart" && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    clickSource.PlayOneShot(clickClip);
                    //REPLACE THIS WITH SCENE MANAGER SCENE SWITCHING FUNCTION
                    StartGame();
                }
            }
            
        }

        minutes = (int)timeForDefusal / 60;
        seconds = (int)timeForDefusal % 60;
        moduleNum.text = "" + numOfModules;
        if(minutes < 10)
        {
            minuteString = "0" + minutes;
        }
        else
        {
            minuteString = "" + minutes;
        }
        if (seconds < 10)
        {
            secondString = "0" + seconds;
        }
        else
        {
            secondString = "" + seconds;
        }
        timerNum.text = "" + minuteString + ":" + secondString;
    }


    void PlusModule()
    {
        if(numOfModules < 11)
        {
            numOfModules++;
        }
    }

    void MinusModule()
    {
        if(numOfModules > 3)
        {
            numOfModules--;
        }
    }

    void PlusTime()
    {
        if(timeForDefusal < 600)
        {
            timeForDefusal += 30;
        }
    }

    void MinusTime()
    {
        if(timeForDefusal > 30)
        {
            timeForDefusal -= 30;
        }
    }

    void StartGame()
    {
        //PLACEHOLDER FOR SCENE MANAGER SWTICHING
    }
}
