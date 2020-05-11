using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhosOnFirst : MonoBehaviour
{

    public string[] keyWords;
    public string[] buttonWords;

    public string screenText;
    public TextMeshPro Text;

    public string[] availableStrings = new string[6];
    public string correctLabel;

    public string[] correctList;

    WhosOnFirstArrays arrayStorage;

    public int correctButtonToPress;

    public WhoButtonScript[] buttons;

    public MeshRenderer LED;
    public Material[] LEDmats;

    public bool CORRECT;

    private GenerateBomb BombScript;

    public int correctCounter;
    public MeshRenderer[] stageLights;
    public Material stageOff;

    // Start is called before the first frame updates
    void Start()
    {
        BombScript = FindObjectOfType<GenerateBomb>();
        arrayStorage = GetComponent<WhosOnFirstArrays>();
        screenText = keyWords[Random.Range(0, 28)];
        Text.text = screenText;
        DetermineCorrectButton();
    }

    void DetermineCorrectButton()
    {
        for (int i = 0; i < keyWords.Length; i++)
        {
            if (screenText == keyWords[i])
            {
                if (i == 0 || i == 4 || i == 7 || i == 22)
                {
                    correctLabel = availableStrings[2];
                }
                else if (i==1 || i==2 || i==24)
                {
                    correctLabel = availableStrings[1];
                }
                else if (i == 26 || i == 27 || i == 3|| i == 8 || i == 13 || i == 15|| i == 19 || i == 23 || i == 25)
                {
                    correctLabel = availableStrings[5];
                }
                else if (i == 5 || i == 11 || i == 12 || i == 20)
                {
                    correctLabel = availableStrings[4];
                }
                else if (i == 6 || i == 9 || i == 10 || i == 14 || i == 16 || i == 17 || i == 21)
                {
                    correctLabel = availableStrings[3];
                }
                else if (i == 18)
                {
                    correctLabel = availableStrings[0];
                }
            }
        } //determines the correct button label to look for.
        SelectButtonList();
        FindButtonNumber();
    }

    void SelectButtonList()
    {
        for (int i = 0; i < buttonWords.Length; i++)
        {
            if (correctLabel == buttonWords[i])
            {
                correctList = arrayStorage.arrays[i];
            }
        }
    }

    void FindButtonNumber()
    {
        for (int i = 0; i < correctList.Length; i++)
        {
            if (correctList[i] == availableStrings[0])
            {
                correctButtonToPress = 0;
                break;
            }
            else if (correctList[i] == availableStrings[1])
            {
                correctButtonToPress = 1;
                break;
            }
            else if (correctList[i] == availableStrings[2])
            {
                correctButtonToPress = 2;
                break;
            }
            else if (correctList[i] == availableStrings[3])
            {
                correctButtonToPress = 3;
                break;
            }
            else if (correctList[i] == availableStrings[4])
            {
                correctButtonToPress = 4;
                break;
            }
            else if (correctList[i] == availableStrings[5])
            {
                correctButtonToPress = 5;
                break;
            }
        }
        correctButtonToPress += 1;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; //outs to hit so i can affect the wires
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //screen point to ray (mouse position)

        if (Physics.Raycast(ray, out hit))// if moused over
        {
            if (hit.collider.tag == "WhoButtons" && Input.GetKeyDown(KeyCode.Mouse0) && (BombScript == null || gameObject == GenerateBomb.SelectedModule))
            {
                hit.collider.transform.localPosition += transform.forward*.07f;
                hit.collider.gameObject.GetComponent<WhoButtonScript>().pressed = true;
                if (hit.collider.gameObject.GetComponent<WhoButtonScript>().buttonNum == correctButtonToPress)
                {
                    Debug.Log("Correct Choice!");
                    correctCounter++;
                    if (correctCounter >= 3)
                    {
                        CORRECT = true;
                        BombScript.Completed();
                    }
                    else
                    {
                        foreach (WhoButtonScript button in buttons)
                        {
                            button.transform.localPosition += transform.forward * .07f;
                            button.ResetMe();
                        }
                        screenText = keyWords[Random.Range(0, 28)];
                        Text.text = screenText;
                        DetermineCorrectButton();
                    }
                }
                else
                {
                    StartCoroutine("FailFlash");
                    BombScript.BombStrikes(); //trigger strike    
                    if (!CORRECT)
                    {
                        Debug.Log("WRONG!");
                        foreach (WhoButtonScript button in buttons)
                        {
                            button.transform.localPosition += transform.forward * .07f;
                            button.ResetMe();
                            correctCounter = 0;
                        }

                        screenText = keyWords[Random.Range(0, 28)];
                        Text.text = screenText;
                        DetermineCorrectButton();
                    }
                }
                BombScript.BombShake(); //Shake bomb
            }
        }

        for (int i = 0; i < stageLights.Length; i++)
        {
            if (i > correctCounter-1)
            {
                stageLights[i].material = stageOff;
            }
            else
            {
                stageLights[i].material = LEDmats[0];
            }
        }

        if (CORRECT)
        {
            LED.material = LEDmats[0];
        }
    }

    IEnumerator FailFlash()
    {
        LED.material = LEDmats[1];
        yield return new WaitForSeconds(.5f);
        LED.material = LEDmats[2];
    }
}
