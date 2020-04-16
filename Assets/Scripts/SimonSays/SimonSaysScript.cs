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

    public GameObject firstPushButton;
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

        //Tell what buttons need to be pushed
        AssignPushButton();
    }

    void AssignPushButton()
    {
        if (serialHasVowel)
        {
            switch (strikeCount)
            {
                case 0:
                    //assign first button to be pushed
                    firstPushButton = NoStrikeVowel(firstButton);

                    //assign second button to be pushed
                    secondPushButton = NoStrikeVowel(secondButton);

                    //assign third button to be pushed
                    thirdPushButton = NoStrikeVowel(thirdButton);

                    //if there are four stages, assign fourth button
                    if (maxStage >= 4)
                    {
                        fourthPushButton = NoStrikeVowel(fourthButton);
                    }

                    //if there are five stages, assign fifth button
                    if(maxStage == 5)
                    {
                        fifthPushButton = NoStrikeVowel(fifthButton);
                    }
                    break;
                case 1:
                    firstPushButton = OneStrikeVowel(firstButton);

                    secondPushButton = OneStrikeVowel(secondButton);

                    thirdPushButton = OneStrikeVowel(thirdButton);

                    if (maxStage >= 4)
                    {
                        fourthPushButton = OneStrikeVowel(fourthButton);
                    }

                    if (maxStage == 5)
                    {
                        fifthPushButton = OneStrikeVowel(fifthButton);
                    }
                    break;
                case 2:
                    firstPushButton = TwoStrikeVowel(firstButton);

                    secondPushButton = TwoStrikeVowel(secondButton);

                    thirdPushButton = TwoStrikeVowel(thirdButton);

                    if (maxStage >= 4)
                    {
                        fourthPushButton = TwoStrikeVowel(fourthButton);
                    }

                    if (maxStage == 5)
                    {
                        fifthPushButton = TwoStrikeVowel(fifthButton);
                    }
                    break;
            }
        }
        else
        {
            switch (strikeCount)
            {
                case 0:
                    //assign first button to be pushed
                    firstPushButton = NoStrikeNoVowel(firstButton);

                    //assign second button to be pushed
                    secondPushButton = NoStrikeNoVowel(secondButton);

                    //assign third button to be pushed
                    thirdPushButton = NoStrikeNoVowel(thirdButton);

                    //if there are four stages, assign fourth button
                    if (maxStage >= 4)
                    {
                        fourthPushButton = NoStrikeNoVowel(fourthButton);
                    }

                    //if there are five stages, assign fifth button
                    if (maxStage == 5)
                    {
                        fifthPushButton = NoStrikeNoVowel(fifthButton);
                    }
                    break;
                case 1:
                    firstPushButton = OneStrikeNoVowel(firstButton);

                    secondPushButton = OneStrikeNoVowel(secondButton);

                    thirdPushButton = OneStrikeNoVowel(thirdButton);

                    if (maxStage >= 4)
                    {
                        fourthPushButton = OneStrikeNoVowel(fourthButton);
                    }

                    if (maxStage == 5)
                    {
                        fifthPushButton = OneStrikeNoVowel(fifthButton);
                    }
                    break;
                case 2:
                    firstPushButton = TwoStrikeNoVowel(firstButton);

                    secondPushButton = TwoStrikeNoVowel(secondButton);

                    thirdPushButton = TwoStrikeNoVowel(thirdButton);

                    if (maxStage >= 4)
                    {
                        fourthPushButton = TwoStrikeNoVowel(fourthButton);
                    }

                    if (maxStage == 5)
                    {
                        fifthPushButton = TwoStrikeNoVowel(fifthButton);
                    }
                    break;
            }
        }
    }

    //Deem What Buttons Need to be pushed based on the amount of strikes and if the serial code has a vowel
    GameObject NoStrikeVowel(GameObject flashing)
    {
        GameObject push;
        if (flashing == buttonRed)
        {
            push = buttonBlue;
        }
        else if (flashing == buttonBlue)
        {
            push = buttonRed;
        }
        else if (flashing == buttonGreen)
        {
            push = buttonYellow;
        }
        else
        {
            push = buttonGreen;
        }
        return push;
    }

    GameObject OneStrikeVowel(GameObject flashing)
    {
        GameObject push;
        if (flashing == buttonRed)
        {
            push = buttonYellow;
        }
        else if (flashing == buttonBlue)
        {
            push = buttonGreen;
        }
        else if (flashing == buttonGreen)
        {
            push = buttonBlue;
        }
        else
        {
            push = buttonRed;
        }
        return push;
    }

    GameObject TwoStrikeVowel(GameObject flashing)
    {
        GameObject push;
        if (flashing == buttonRed)
        {
            push = buttonGreen;
        }
        else if (flashing == buttonBlue)
        {
            push = buttonRed;
        }
        else if (flashing == buttonGreen)
        {
            push = buttonYellow;
        }
        else
        {
            push = buttonBlue;
        }
        return push;
    }

    GameObject NoStrikeNoVowel(GameObject flashing)
    {
        GameObject push;
        if (flashing == buttonRed)
        {
            push = buttonBlue;
        }
        else if (flashing == buttonBlue)
        {
            push = buttonYellow;
        }
        else if (flashing == buttonGreen)
        {
            push = buttonGreen;
        }
        else
        {
            push = buttonRed;
        }
        return push;
    }

    GameObject OneStrikeNoVowel(GameObject flashing)
    {
        GameObject push;
        if (flashing == buttonRed)
        {
            push = buttonRed;
        }
        else if (flashing == buttonBlue)
        {
            push = buttonBlue;
        }
        else if (flashing == buttonGreen)
        {
            push = buttonYellow;
        }
        else
        {
            push = buttonGreen;
        }
        return push;
    }

    GameObject TwoStrikeNoVowel(GameObject flashing)
    {
        GameObject push;
        if (flashing == buttonRed)
        {
            push = buttonYellow;
        }
        else if (flashing == buttonBlue)
        {
            push = buttonGreen;
        }
        else if (flashing == buttonGreen)
        {
            push = buttonBlue;
        }
        else
        {
            push = buttonRed;
        }
        return push;
    }
}



