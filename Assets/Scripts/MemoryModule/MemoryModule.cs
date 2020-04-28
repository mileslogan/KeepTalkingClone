using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryModule : MonoBehaviour
{

    public bool Complete = false;
    public bool Failed = false;
    public GenerateBomb BombScript;

    public List<int[]> stages = new List<int[]>();
    public int[] stage01 = { 1, 2, 3, 4 }; // The numbers that appear on the buttons for stage 1 in order
    public int[] stage02 = { 1, 2, 3, 4 };// The numbers that appear on the buttons for stage 2 in order
    public int[] stage03 = { 1, 2, 3, 4 };// The numbers that appear on the buttons for stage 3 in order
    public int[] stage04 = { 1, 2, 3, 4 };// The numbers that appear on the buttons for stage 4 in order
    public int[] stage05 = { 1, 2, 3, 4 };// The numbers that appear on the buttons for stage 5 in order
    public int curstage = 1; // current stage 
    public int bigNumber; // The big number at the top
    int stage01index; // The index of button that player click on stage 1
    int stage02index;// The index of button that player click on stage 2
    int stage03index;// The index of button that player click on stage 3
    int stage04index;// The index of button that player click on stage 4
    int stage05index;// The index of button that player click on stage 5

    public GameObject text1; //text on button 0
    public GameObject text2; //text on button 1
    public GameObject text3; //text on button 2
    public GameObject text4; //text on button 3
    public GameObject bigtext; //big number on the top
    public GameObject light1; // the light that turns on after complete stage 1
    public GameObject light2; // the light that turns on after complete stage 2
    public GameObject light3; // the light that turns on after complete stage 3
    public GameObject light4; // the light that turns on after complete stage 4
    public GameObject light5; // the light that turns on after complete stage 5
    public GameObject completeLight; // the top LED light

    public Material greenlight; // material of the green light
    public Material redlight;// material of the red light
    public Material lightoff;// material of the light-off

    public Animator memoryAnimator;
    public AudioSource audioClick;

    void Start()
    {
        //add the int array for each stage to the ArrayList
        stages.Add(stage01); 
        stages.Add(stage02);
        stages.Add(stage03);
        stages.Add(stage04);
        stages.Add(stage05);
        BombScript = FindObjectOfType<GenerateBomb>();

        // start from stage one
        StartStage();
    }


    void Update()
    {
        // Raycast
        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

        if (Physics.Raycast(ray, out hit))// if moused over
        {
            if (hit.collider.tag == "MemoryModuleButton" && Input.GetKeyDown(KeyCode.Mouse0) && hit.collider.transform.parent.gameObject == GenerateBomb.SelectedModule)
            {
                audioClick.Play(0);
                int buttonIndex = hit.collider.gameObject.GetComponent<MemoryModuleButton>().buttonIndex;
                ButtonClick(buttonIndex);
            }
        }

    }

    // Randomize the order of number in each stage
    void Shuffle()
    {
        foreach (int[] stage in stages)
        {
            // switch position twice for each stage's array
            for (int i = 0; i < 2; i++)
            {
                // randomly chose two index in the array and switch their position
                int temp;
                int index1 = Random.Range(0, 4); 
                int index2 = Random.Range(0, 4);
                temp = stage[index1];
                stage[index1] = stage[index2];
                stage[index2] = temp;
            }
        }
    }

    // start & restart after failed
    void StartStage()
    {
        //shuffle all arrays in the for each stage
        Shuffle();
        curstage = 1; // reset current stage
        bigNumber = Random.Range(1, 5); //randomly set the bignumber from 1 to 4
        Refresh(); // refresh the text
    }

    //send the index of the button that has been clicked to stage check
    public void ButtonClick(int index)
    {
        // send the index of the button that is clicked to current stage for further check
        switch (curstage)
        {
            case 1:
                Stage1check(index); 
                break;
            case 2:
                Stage2check(index);
                break;
            case 3:
                Stage3check(index);
                break;
            case 4:
                Stage4check(index);
                break;
            case 5:
                Stage5check(index);
                break;

        }
    }

    // move to next stage after finish
    void NextStage()
    {
        curstage++; // move to next stage
        bigNumber = Random.Range(1, 5);//Set the big number at top
        Refresh(); // refresh the text
    }

    //check of the button click for each stage
    void Stage1check(int index)
    {
        stage01index = index;
        if (bigNumber == 1 && index == 1) //If the display is 1, press the button in the second position.
        {
            NextStage();
            return;
        }

        if (bigNumber == 2 && index == 1) //If the display is 2, press the button in the second position.
        {
            NextStage();
            return;
        }

        if (bigNumber == 3 && index == 2)//If the display is 3, press the button in the third position.
        {
            NextStage();
            return;

        }
        if (bigNumber == 4 && index == 3)//If the display is 4, press the button in the fourth position.
        {
            NextStage();
            return;
        }
        Strike();
        StartStage(); // go back the startStage if failed
    }

    void Stage2check(int index)
    {
        stage02index = index;
        if (bigNumber == 1 && stage02[index] == 4) //If the display is 1, press the button labeled "4".
        {
            NextStage();
            return;
        }

        if (bigNumber == 2 && index == stage01index)//If the display is 2, press the button in the same position as you pressed in stage 1
        {
            NextStage();
            return;
        }

        if (bigNumber == 3 && index == 0)//If the display is 3, press the button in the first position.
        {
            NextStage();
            return;

        }
        if (bigNumber == 4 && index == stage01index)//If the display is 4, press the button in the same position as you pressed in stage 1
        {
            NextStage();
            return;
        }
        Strike();
        StartStage(); // go back the startStage if failed
    }

    void Stage3check(int index)
    {
        stage03index = index;
        if (bigNumber == 1 && stage03[index] == stage02[stage02index]) // If the display is 1, press the button with the same label you pressed in stage 2
        {
            NextStage();
            return;
        }

        if (bigNumber == 2 && stage03[index] == stage01[stage01index]) // If the display is 2, press the button with the same label you pressed in stage 1
        {
            NextStage();
            return;
        }

        if (bigNumber == 3 && index == 2)//If the display is 3, press the button in the third position.
        {
            NextStage();
            return;

        }
        if (bigNumber == 4 && stage03[index] == 4)//If the display is 4, press the button labeled "4".
        {
            NextStage();
            return;
        }
        Strike();
        StartStage();// go back the startStage if failed
    }

    void Stage4check(int index)
    {
        stage04index = index;
        if (bigNumber == 1 && index == stage01index)//If the display is 1, press the button in the same position as you pressed in stage 1
        {
            NextStage();
            return;
        }

        if (bigNumber == 2 && index == 0)//If the display is 2, press the button in the first position.
        {
            NextStage();
            return;
        }

        if (bigNumber == 3 && index == stage02index)//If the display is 3, press the button in the same position as you pressed in stage 2
        {
            NextStage();
            return;

        }
        if (bigNumber == 4 && index == stage02index)//If the display is 4, press the button in the same position as you pressed in stage 2
        {
            NextStage();
            return;
        }
        Strike();
        StartStage();// go back the startStage if failed
    }

    void Stage5check(int index)
    {
        stage05index = index;
        if (bigNumber == 1 && stage05[index] == stage01[stage01index])// If the display is 1, press the button with the same label you pressed in stage 1
        {
            NextStage();
            return;
        }

        if (bigNumber == 2 && stage05[index] == stage02[stage02index])// If the display is 2, press the button with the same label you pressed in stage 2
        {
            NextStage();
            return;
        }

        if (bigNumber == 3 && stage05[index] == stage03[stage03index])// If the display is 3, press the button with the same label you pressed in stage 4
        {
            NextStage();
            return;

        }
        if (bigNumber == 4 && stage05[index] == stage04[stage04index])// If the display is 4, press the button with the same label you pressed in stage 3
        {
            NextStage();
            return;
        }
        Strike();
        StartStage();// go back the startStage if failed
    }

    // refresh the text for testing 
    void Refresh()
    {
        if (curstage == 6)
        {
            Complete = true;
            if (BombScript != null)
            {
                BombScript.Completed();// call the Bomb script when complete
            }
            ChangeLight(); // Change the LED light to green
            return;
        }
        bigtext.GetComponent<TextMeshPro>().text = "" + bigNumber;
        text1.GetComponent<TextMeshPro>().text = "" + stages[curstage - 1][0];
        text2.GetComponent<TextMeshPro>().text = "" + stages[curstage - 1][1];
        text3.GetComponent<TextMeshPro>().text = "" + stages[curstage - 1][2];
        text4.GetComponent<TextMeshPro>().text = "" + stages[curstage - 1][3];
        ChangeLight();
        memoryAnimator.SetTrigger("MemoryRefreshButton");
    }

    //change the green light that shows the curstage. Change the top LED light after complete
    void ChangeLight()
    {
        //change the light base on the stages
        switch (curstage)
        {
            case 1:
                light1.GetComponent<MeshRenderer>().material = lightoff;
                light2.GetComponent<MeshRenderer>().material = lightoff;
                light3.GetComponent<MeshRenderer>().material = lightoff;
                light4.GetComponent<MeshRenderer>().material = lightoff;
                light5.GetComponent<MeshRenderer>().material = lightoff;
                break;
            case 2:
                light1.GetComponent<MeshRenderer>().material = greenlight;
                break;
            case 3:
                light2.GetComponent<MeshRenderer>().material = greenlight;
                break;
            case 4:
                light3.GetComponent<MeshRenderer>().material = greenlight;
                break;
            case 5:
                light4.GetComponent<MeshRenderer>().material = greenlight;
                break;
            case 6:
                light5.GetComponent<MeshRenderer>().material = greenlight;
                completeLight.GetComponent<MeshRenderer>().material = greenlight;
                break;

        }


    }


    void Strike()
    {
        if (BombScript != null)
        {
            BombScript.BombStrikes(); // call the Bomb script when Strikes
        }
        
        StartCoroutine(FlashRedLight()); // call the function that flash the red light
    }

    //flash red light
    IEnumerator FlashRedLight()
    {
        completeLight.GetComponent<MeshRenderer>().material = redlight;
        yield return new WaitForSeconds(0.3f);
        completeLight.GetComponent<MeshRenderer>().material = lightoff;
    }
    
}
