using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public static bool Defused; // true if the bomb is defused, false if it explorded
    public int time;
    public int modulesNum;
    public int strikeNum;
    public string timeRemain;
    public string cause; // Cause of explosions

    public GameObject defusedPage; // png of the defused page
    public GameObject explodedPage;// png of the exploded page
	public GameObject defusedStamp;
	public GameObject explodedStamp;

	// Text
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
			defusedStamp.GetComponent<Animator>().SetTrigger("stamp2");

		}
        else
        {
            defusedPage.GetComponent<SpriteRenderer>().enabled = false;
            explodedStamp.GetComponent<Animator>().SetTrigger("stamp");
        }

        TimeCalculator();//convey timeleft(float) to string representation

        //Set Time text
		time = StartScreen.timeForDefusal;
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
		timeText.GetComponent<TextMeshPro>().text = str_minutes + " : " + str_seconds;

        //Set ModulesNum
		modulesNum = StartScreen.numOfModules;
        modulesNumText.GetComponent<TextMeshPro>().text = ""+ modulesNum + " Modules"; // Need access to the start screen script 
        strikeNumText.GetComponent<TextMeshPro>().text =  "3 Strikes"; 
        timeRemainText.GetComponent<TextMeshPro>().text = "" + timeRemain;

        //cause of explosions is empty if defused
        if (Defused)
        {
            causeText.GetComponent<TextMeshPro>().text = "";
        }
        else
        {
            causeText.GetComponent<TextMeshPro>().text = "" + GenerateBomb.LostOnThisModule;
        }

        StartCoroutine(StampSound());// Play Audio Source for the stamp


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

    public IEnumerator StampSound()
    {
        yield return new WaitForSeconds(0.35f);
        GetComponent<AudioSource>().Play(0);

    }



}
