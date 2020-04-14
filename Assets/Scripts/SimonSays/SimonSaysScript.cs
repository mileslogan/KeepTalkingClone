using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysScript : MonoBehaviour
{
    int strikeCount;

    bool serialHasVowel;

    public bool completed;

    public GameObject buttonRed;
    public GameObject buttonBlue;
    public GameObject buttonGreen;
    public GameObject buttonYellow;

    int currentStage;

    //These are the buttons that will flash
    public GameObject firstButton;
    public GameObject secondButton;
    public GameObject thirdButton;
    public GameObject fourthButton;
    public GameObject fifthButton;

    GameObject firstPushButton;
    GameObject secondPushButton;
    GameObject thirdPushButton;
    GameObject fourthPushButton;
    GameObject fifthPushButton;

    int maxStage;

    void Start()
    {
        completed = false;
        maxStage = Random.Range(3, 6);
        SequenceGen();
        currentStage = 1;
    }

    //WORK IN PROGRESS! WILL HAVE FINISHED BY 4/16!
    void Update()
    {
        
    }


    //generate or regenerate the buttons that will flash
    void SequenceGen()
    {
        //Generates first button to flash
        int random = Random.Range(0, 4);
        if(random == 0)
        {
            firstButton = buttonRed;
        }
        else if(random == 1)
        {
            firstButton = buttonBlue;
        }
        else if(random == 2)
        {
            firstButton = buttonGreen;
        }
        else
        {
            firstButton = buttonYellow;
        }

        //Generates Second button to flash
        random = Random.Range(0, 4);
        if (random == 0)
        {
            secondButton = buttonRed;
        }
        else if (random == 1)
        {
            secondButton = buttonBlue;
        }
        else if (random == 2)
        {
            secondButton = buttonGreen;
        }
        else
        {
            secondButton = buttonYellow;
        }

        //Generates Third button to flash
        random = Random.Range(0, 4);
        if (random == 0)
        {
            thirdButton = buttonRed;
        }
        else if (random == 1)
        {
            thirdButton = buttonBlue;
        }
        else if (random == 2)
        {
            thirdButton = buttonGreen;
        }
        else
        {
            thirdButton = buttonYellow;
        }

        //If there are four stages that need to be completed, generate fourth button in sequence
        if(maxStage >= 4)
        {
            random = Random.Range(0, 4);
            if (random == 0)
            {
                fourthButton = buttonRed;
            }
            else if (random == 1)
            {
                fourthButton = buttonBlue;
            }
            else if (random == 2)
            {
                fourthButton = buttonGreen;
            }
            else
            {
                fourthButton = buttonYellow;
            }
        }

        //If there are five stages that need to be completed, generate fifth button in sequence
        if(maxStage == 5)
        {
            random = Random.Range(0, 4);
            if (random == 0)
            {
                fifthButton = buttonRed;
            }
            else if (random == 1)
            {
                fifthButton = buttonBlue;
            }
            else if (random == 2)
            {
                fifthButton = buttonGreen;
            }
            else
            {
                fifthButton = buttonYellow;
            }
        }
    }
}
