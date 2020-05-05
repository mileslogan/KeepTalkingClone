using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    public Color currentColor;

    public Material stripColor0;
    public Material stripColor1;
    public Material stripColor2;

    public Material[] stripMats;

    string detonate = "DETONATE";
    string abort = "ABORT";
    string hold = "HOLD";
    string press = "PRESS";

    public string currentString;

    int randomRoller;

    public bool completed;
    public bool pressing;

    public float miniTimer;

    bool frkOn;
    bool carOn;

    public bool needHold;

    public TextMeshPro buttonWords;
    public MeshRenderer buttonStrip;

    public GenerateBomb bombScript;

    public MeshRenderer buttonMesh;

    public Animator buttonAnim;

    public Material[] lightMaterials;
    public MeshRenderer LED;

    public Timer timeScript;

    public AudioSource buttonAudioSource;
    public AudioClip[] buttonClicks;

    // Start is called before the first frame update
    void Start()
    {
        
        bombScript = FindObjectOfType<GenerateBomb>();
        timeScript = FindObjectOfType<Timer>();

        if (bombScript != null)
        {
            foreach (Indicator ind in bombScript.AddedIndicators)
            {

                if (ind.Str == "CAR")
                {

                    if (ind.IsOn)
                    {
                        carOn = true;
                    }
                }

                if (ind.Str == "FRK")
                {
                    if (ind.IsOn)
                    {
                        frkOn = true;
                    }
                }
            }
        }
        

        completed = false;
        pressing = false;

        //Generate Color & Word on Bomb, and Strip Color for each strike
        currentColor = ButtonColorGen();

        currentString = ButtonTextGen();

        stripColor0 = StripColorGen();
        stripColor1 = StripColorGen();
        stripColor2 = StripColorGen();

        //Set Button Color and Text
        buttonMesh.material.SetColor("_BaseColor", currentColor);
        buttonWords.text = currentString;

        //Check if generated button needs to be pressed or held and released
        HoldOrPress();
    }

    // Update is called once per frame
    void Update()
    {
        if (bombScript == null || GenerateBomb.SelectedModule == gameObject)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "ButtonButton" && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    buttonAudioSource.PlayOneShot(buttonClicks[0]);
                    pressing = true;
                    buttonAnim.SetBool("isPressing", true);
                    bombScript.BombShake();
                }
            }
        }
        

        if(pressing)
        {
                if (bombScript != null && bombScript.CurrentStrikes == 0)
                {
                    buttonStrip.material = stripColor0;
                }
                else if (bombScript != null && bombScript.CurrentStrikes == 1)
                {
                    buttonStrip.material = stripColor1;
                }
                else
                {
                    buttonStrip.material = stripColor2;
                }

                if (!needHold)
                {
                    miniTimer += Time.deltaTime;
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        buttonAnim.SetBool("isPressing", false);
                        
                        buttonStrip.material = stripMats[4];
                        if (miniTimer <= 0.8f)
                        {
                            miniTimer = 0f;
                            Completed();
                        }
                        else
                        {
                            miniTimer = 0f;
                            Failure();
                        }
                        buttonAudioSource.PlayOneShot(buttonClicks[1]);
                        pressing = false;
                        bombScript.BombShake();
                    }
                }

                if (needHold)
                {
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        if (buttonStrip.material == stripMats[1])
                        {
                            if (timeScript.str_minutes.ToLower().IndexOf('4') != -1 || timeScript.str_seconds.ToLower().IndexOf('4') != -1)
                            {
                                Completed();
                            }
                            else
                            {
                                Failure();
                            }
                        }
                        else if (buttonStrip.material == stripMats[3])
                        {
                            if (timeScript.str_minutes.ToLower().IndexOf('5') != -1 || timeScript.str_seconds.ToLower().IndexOf('5') != -1)
                            {
                                Completed();
                            }
                            else
                            {
                                Failure();
                            }
                        }
                        else
                        {
                                if (timeScript.str_minutes.ToLower().IndexOf('1') != -1 || timeScript.str_seconds.ToLower().IndexOf('1') != -1)
                                {
                                    Completed();
                                }
                                else
                                {
                                    Failure();
                                }
                        }
                    bombScript.BombShake();
                        buttonAudioSource.PlayOneShot(buttonClicks[1]);
                        buttonAnim.SetBool("isPressing", false);
                        buttonStrip.material = stripMats[4];
                        pressing = false;
                    }
                }
        }
    }

    //Pick a color: Red, White, Yellow, Blue, or Black; Set Text Color to fit
    Color ButtonColorGen()
    {
        Color buttonColor;
        randomRoller = Random.Range(1, 6);
        if (randomRoller == 1)
        {
            buttonColor = Color.red;
            buttonWords.color = Color.white;
        }
        else if (randomRoller == 2)
        {
            buttonColor = Color.white;
            buttonWords.color = Color.black;
        }
        else if (randomRoller == 3)
        {
            buttonColor = Color.yellow;
            buttonWords.color = Color.black;
        }
        else if (randomRoller == 4)
        {
            buttonColor = Color.blue;
            buttonWords.color = Color.white;
        }
        else
        {
            buttonColor = Color.black;
            buttonWords.color = Color.white;
        }
        return buttonColor;
    }

    //Pick a color: Red, White, Yellow, or Blue
    Material StripColorGen()
    {
        Material genColor;
        randomRoller = Random.Range(1, 5);
        if (randomRoller == 1)
        {
            genColor = stripMats[0];
        }
        else if (randomRoller == 2)
        {
            genColor = stripMats[2];
        }
        else if (randomRoller == 3)
        {
            genColor = stripMats[3];
        }
        else
        {
            genColor = stripMats[1];
        }
        return genColor;
    }

    //Pick one of the four text options
    string ButtonTextGen()
    {
        string buttonText;
        randomRoller = Random.Range(1, 5);
        if (randomRoller == 1)
        {
            buttonText = detonate;
        }
        else if (randomRoller == 2)
        {
            buttonText = abort;
        }
        else if (randomRoller == 3)
        {
            buttonText = hold;
        }
        else
        {
            buttonText = press;
        }
        return buttonText;
    }

    //Figure out whether the bomb needs to be held or pressed; WORK IN PROGESS
    void HoldOrPress()
    {
        //Functionaltiy based on what is on bomb and what color and word there is
        if (currentColor == Color.blue && currentString == abort)
        {
            needHold = true;
        }
        else if (currentString == detonate && bombScript != null && bombScript.BatteryNum > 1)
        {
            needHold = false;
        }
        else if (currentColor == Color.white && carOn)
        {
            needHold = true;
        }
        else if (frkOn && bombScript != null && bombScript.BatteryNum > 2)
        {
            needHold = false;
        }
        else if (currentColor == Color.yellow)
        {
            needHold = true;
        }
        else if (currentColor == Color.red && currentString == hold)
        {
            needHold = false;
        }
        else
        {
            needHold = true;
        }
    }

    void Failure()
    {
        if (bombScript != null)
        {
            bombScript.BombStrikes();
        }
        if (!completed)
        {
            StartCoroutine("FailFlash");
        }
    }

    void Completed()
    {
        if (!completed)
        {
            if (bombScript != null)
            {
                bombScript.Completed();
            }
            LED.material = lightMaterials[0];
            completed = true;
        }
    }

    IEnumerator FailFlash()
    {
        LED.material = lightMaterials[1];
        yield return new WaitForSeconds(.5f);
        LED.material = lightMaterials[2];
    }
}
