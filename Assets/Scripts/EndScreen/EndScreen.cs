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
    public int timeRemain;
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

        timeText.GetComponent<TextMeshPro>().text = "";
        modulesNumText.GetComponent<TextMeshPro>().text = ""+ modulesNum + " Modules";
        strikeNumText.GetComponent<TextMeshPro>().text = "" + strikeNum + " Strikes";
        timeRemainText.GetComponent<TextMeshPro>().text = "" + timeRemain;
        causeText.GetComponent<TextMeshPro>().text = "" + cause;
    }

    
    void Update()
    {
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

    void LoadScene(int num)
    {

        if (num == 0)
            SceneManager.LoadScene(""); // retry
        else
            SceneManager.LoadScene("StartingScene"); //go back to start Screen


    }

}
