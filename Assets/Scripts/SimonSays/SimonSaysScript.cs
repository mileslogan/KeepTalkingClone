using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysScript : MonoBehaviour
{
    int strikeCount;

    bool completed;

    public GameObject buttonRed;
    public GameObject buttonBlue;
    public GameObject buttonGreen;
    public GameObject buttonYellow;

    public int currentStage;
    public int buttonsPressed;

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

    public GenerateBomb bombScript;

    public Material[] lightMaterials;
    public MeshRenderer LED;

    public Animator simonAnimator;

    bool corutineStarted;

    public AudioSource simonSound;

    public AudioClip[] buttonBeep;

    void Start()
    {
        bombScript = FindObjectOfType<GenerateBomb>();
        completed = false;
        maxStage = Random.Range(3, 6);
        SequenceGen();
        currentStage = 1;
        buttonsPressed = 0;
    }

    
    void Update()
    {
        if (bombScript != null)
        {
            if (strikeCount != bombScript.CurrentStrikes)
            {
                strikeCount = bombScript.CurrentStrikes;
                AssignPushButton();
            }
        }

        //if mouse is let go, set all animation conditions to false
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            simonAnimator.SetBool("PressingBlue", false);
            simonAnimator.SetBool("PressingRed", false);
            simonAnimator.SetBool("PressingGreen", false);
            simonAnimator.SetBool("PressingYellow", false);
            corutineStarted = false;
            simonSound.Stop();
        }

        if (currentStage <= maxStage)
        {
            if (corutineStarted == false)
            {
                StopCoroutine("SimonSaysSequence");
                StartCoroutine("SimonSaysSequence");
            }
        }

        //if (GenerateBomb.SelectedModule == gameObject)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (currentStage <= maxStage)
            {
                if (Physics.Raycast(ray, out hit))
                {

                  if(hit.collider.gameObject == buttonBlue && Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        StopCoroutine("SimonSaysSequence");
                        simonAnimator.SetBool("PressingBlue", true);
                        simonSound.clip = buttonBeep[1];
                        simonSound.Play();
                        
                    }
                    if (hit.collider.gameObject == buttonRed && Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        StopCoroutine("SimonSaysSequence");
                        simonAnimator.SetBool("PressingRed", true);
                        simonSound.clip = buttonBeep[0];
                        simonSound.Play();

                    }
                    if (hit.collider.gameObject == buttonGreen && Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        StopCoroutine("SimonSaysSequence");
                        simonAnimator.SetBool("PressingGreen", true);
                        simonSound.clip = buttonBeep[2];
                        simonSound.Play();

                    }
                    if (hit.collider.gameObject == buttonYellow && Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        StopCoroutine("SimonSaysSequence");
                        simonAnimator.SetBool("PressingYellow", true);
                        simonSound.clip = buttonBeep[3];
                        simonSound.Play();

                    }

                    //logic for adavncing game or getting strikes
                    switch (buttonsPressed)
                    {
                        case 0:
                            if (hit.collider.gameObject == firstPushButton && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                buttonsPressed++;
                            }
                            else if (hit.collider.tag == "SIMONBUTTON" && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                if (bombScript != null)
                                {
                                    bombScript.BombStrikes();
                                }
                                StartCoroutine("FailFlash");
                            }
                            break;
                        case 1:
                            if (hit.collider.gameObject == secondPushButton && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                buttonsPressed++;
                            }
                            else if (hit.collider.tag == "SIMONBUTTON" && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                if (bombScript != null)
                                {
                                    bombScript.BombStrikes();
                                }
                                StartCoroutine("FailFlash");
                                buttonsPressed = 0;
                            }
                            break;
                        case 2:
                            if (hit.collider.gameObject == thirdPushButton && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                buttonsPressed++;
                            }
                            else if (hit.collider.tag == "SIMONBUTTON" && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                if (bombScript != null)
                                {
                                    bombScript.BombStrikes();
                                }
                                StartCoroutine("FailFlash");
                                buttonsPressed = 0;
                            }
                            break;
                        case 3:
                            if (hit.collider.gameObject == fourthPushButton && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                buttonsPressed++;
                            }
                            else if (hit.collider.tag == "SIMONBUTTON" && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                if (bombScript != null)
                                {
                                    bombScript.BombStrikes();
                                }
                                StartCoroutine("FailFlash");
                                buttonsPressed = 0;
                            }
                            break;
                        case 4:
                            if (hit.collider.gameObject == fifthPushButton && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                buttonsPressed++;
                            }
                            else if (hit.collider.tag == "SIMONBUTTON" && Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                if(bombScript != null)
                                {
                                    bombScript.BombStrikes();
                                }
                                StartCoroutine("FailFlash");
                                buttonsPressed = 0;
                            }
                            break;
                    }
                }
            }
            else if (currentStage == maxStage + 1)
            {
                if (bombScript != null)
                {
                    bombScript.Completed();
                }

                LED.material = lightMaterials[0];
                currentStage++;
            }
            else
            {
                completed = true;
            }
        }

        if(buttonsPressed == currentStage)
        {
            currentStage++;
            buttonsPressed = 0;
        }

    }

    IEnumerator FailFlash()
    {
        LED.material = lightMaterials[1];
        yield return new WaitForSeconds(.5f);
        LED.material = lightMaterials[2];
    }

    IEnumerator SimonSaysSequence()
    {
        corutineStarted = true;
        yield return new WaitForSeconds(1f);
        Debug.Log("started once");
        switch (currentStage)
        {
            case 1:
                FlashButton(firstButton);
                yield return new WaitForSeconds(2f);
                break;
            case 2:
                FlashButton(firstButton);
                yield return new WaitForSeconds(1.5f);
                FlashButton(secondButton);
                yield return new WaitForSeconds(2f);
                break;
            case 3:
                FlashButton(firstButton);
                yield return new WaitForSeconds(1.5f);
                FlashButton(secondButton);
                yield return new WaitForSeconds(1.5f);
                FlashButton(thirdButton);
                yield return new WaitForSeconds(2f);
                break;
            case 4:
                if(maxStage >= 4)
                {
                    FlashButton(firstButton);
                    yield return new WaitForSeconds(1.5f);
                    FlashButton(secondButton);
                    yield return new WaitForSeconds(1.5f);
                    FlashButton(thirdButton);
                    yield return new WaitForSeconds(1.5f);
                    FlashButton(fourthButton);
                    yield return new WaitForSeconds(2f);
                }
                break;
            case 5:
                if (maxStage >= 5)
                {
                    FlashButton(firstButton);
                    yield return new WaitForSeconds(1.5f);
                    FlashButton(secondButton);
                    yield return new WaitForSeconds(1.5f);
                    FlashButton(thirdButton);
                    yield return new WaitForSeconds(1.5f);
                    FlashButton(fourthButton);
                    yield return new WaitForSeconds(1.5f);
                    FlashButton(fifthButton);
                    yield return new WaitForSeconds(2f);
                }
                break;
        }

        corutineStarted = false;
      
    }

    void FlashButton(GameObject currentButton)
    {
        if(currentButton == buttonBlue)
        {
            simonAnimator.SetTrigger("FlashBlue");
            if (bombScript != null && GenerateBomb.SelectedModule == gameObject)
            {
                simonSound.PlayOneShot(buttonBeep[1]);
            }
        }
        if (currentButton == buttonRed)
        {
            simonAnimator.SetTrigger("FlashRed");
            if (bombScript != null && GenerateBomb.SelectedModule == gameObject)
            {
                simonSound.PlayOneShot(buttonBeep[0]);
            }
        }
        if (currentButton == buttonGreen)
        {
            simonAnimator.SetTrigger("FlashGreen");
            if (bombScript != null && GenerateBomb.SelectedModule == gameObject)
            {
                simonSound.PlayOneShot(buttonBeep[2]);
            }
        }
        if (currentButton == buttonYellow)
        {
            simonAnimator.SetTrigger("FlashYellow");
            if (bombScript != null && GenerateBomb.SelectedModule == gameObject)
            {
                simonSound.PlayOneShot(buttonBeep[3]);
            }
        }
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
        if (bombScript != null && !bombScript.HasVowel)
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



