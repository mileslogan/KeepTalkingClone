using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public bool Defused; // true if the bomb is defused, false if it explorded
    public string time;
    public int modulesNum;
    public int strikeNum;
    public string timeRemain;
    public string cause;

    public GameObject defusedPage;
    public GameObject explodedPage;
    public GameObject timeText;
    public GameObject modulesNumText;
    public GameObject strikeNumText;
    public GameObject timeRemainText;
    public GameObject causeText; 



    void Start()
    {
        
        if (Defused)
        {
            explodedPage.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            defusedPage.GetComponent<SpriteRenderer>().enabled = false;
        }

        TimeCalculator();//convey timeleft(float) to string representation

        timeText.GetComponent<TextMeshPro>().text = "" + time;
        modulesNumText.GetComponent<TextMeshPro>().text = ""+ modulesNum + " Modules";
        strikeNumText.GetComponent<TextMeshPro>().text =  "3 Strikes"; //
        timeRemainText.GetComponent<TextMeshPro>().text = "" + timeRemain; //
        causeText.GetComponent<TextMeshPro>().text = "" + cause;
    }

    
    void Update()
    {
        // RayCast
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))// if moused over
        {
            if (hit.collider.tag == "EndScreenButton" && Input.GetKeyDown(KeyCode.Mouse0))
            {
                int buttonIndex = hit.collider.gameObject.GetComponent<EndScreenButton>().endscreenbuttonindex;
                LoadScene(buttonIndex);
            }
        }
    }

    void TimeCalculator() //convey timeleft(float) to string representation
    {
        int minutes;
        int seconds;
        string str_minutes;
        string str_seconds;
        minutes = (int)Timer.timeleft / 60;
        seconds = (int)Timer.timeleft % 60;
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

        timeRemain = str_minutes + " : " + str_seconds;

    }

    void LoadScene(int num) //load to start screen or retry 
    {

        if (num == 0)
            SceneManager.LoadScene(""); // retry
        else
            SceneManager.LoadScene("StartingScene"); //go back to start Screen


    }

}
