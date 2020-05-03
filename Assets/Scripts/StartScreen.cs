using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public static int numOfModules;
    public static int timeForDefusal;

    public GenerateBomb bombScript;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        bombScript = FindObjectOfType<GenerateBomb>();
    }
    // Update is called once per frame
    void Update()
    {
        if(bombScript == null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

            }
        }
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
        if(timeForDefusal < 20)
        {
            timeForDefusal++;
        }
    }

    void MinusTime()
    {
        if(timeForDefusal > 1)
        {
            timeForDefusal--;
        }
    }
}
