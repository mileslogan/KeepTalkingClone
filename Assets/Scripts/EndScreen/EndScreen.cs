using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public bool Defused; // true if the bomb is defused, false if it explorded
    public int time;
    public int modulesNum;
    public int strikeNum;
    public int timeleft;
    public string timeRemain;
    public string cause; // Cause of explosions

    //Sprite Obejct
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
    //Scene Manager
    public SceneManage managerScript;



    void Start()
    {
        
        managerScript = FindObjectOfType<SceneManage>();
        managerScript.fadeAnim.SetInteger("FadeState", 1);

        // Get value from the Scene Manager script
        Defused = managerScript.defused;
        time = managerScript.defuseTime;
        modulesNum = managerScript.numModules;
        timeleft = managerScript.timeLeft;
        cause = managerScript.causeOfDeath;
        RemainTimeCalculator();//convey timeleft(float) to string representation

        // Set the sprite and stamps
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

        
        //Set Time text
		int minutes;
		int seconds;
		string str_minutes;
		string str_seconds;
		minutes = time / 60;
		seconds = time % 60;
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

        //Set ModulesNumText
        modulesNumText.GetComponent<TextMeshPro>().text = ""+ modulesNum + " Modules";
        //Set StrikeNumText
        strikeNumText.GetComponent<TextMeshPro>().text =  "3 Strikes";
        //Set RemainningTimeText
        timeRemainText.GetComponent<TextMeshPro>().text = "" + timeRemain;

        //cause of explosions is empty if defused
        if (Defused)
        {
            causeText.GetComponent<TextMeshPro>().text = "";
        }
        else
        {
            causeText.GetComponent<TextMeshPro>().text = "" + cause;
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

    void RemainTimeCalculator() //convey timeleft(float) to string representation
    {
        int minutes;
        int seconds;
        string str_minutes;
        string str_seconds;
        minutes = timeleft / 60;
        seconds = timeleft % 60;
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
            managerScript.ToBombFunction();
        else
            managerScript.ToStartFunction();
    }

    public IEnumerator StampSound()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().Play(0);

    }



}
