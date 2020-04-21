using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireModScript : MonoBehaviour
{
    public bool COMPLETED;
    public bool FAILED;

    public Material[] lightMaterials;
    public MeshRenderer LED;

    string serialNum;
    //to be taken from the bomb casing from Mike

    public int wireCount; //3-6
    int wireCounter; //ensures there are exactly the right amount of wire when initilizing the wire positions

    public GameObject[] wireSlots; //all possible activated wires
    public List<GameObject> activeWires = new List<GameObject>();

    Color[] colorOrder; //gets the colors of the wires in order from top to bottom
    int colorCounter = 0;

    //CASES: read the manual to understand the naming.
    //three
    public bool three_one = false;
    public bool three_two = false;
    public bool three_three = false;
    public bool three_four = false;
    //four
    public bool four_one = false;
    public bool four_two = false;
    public bool four_three = false;
    public bool four_four = false;
    public bool four_five = false;
    //five
    public bool five_one = false;
    public bool five_two = false;
    public bool five_three = false;
    public bool five_four = false;
    //six
    public bool six_one = false;
    public bool six_two = false;
    public bool six_three = false;
    public bool six_four = false;
    //
    int bluecount = 0;
    int redcount = 0;
    int yellowcount = 0;
    int blackcount = 0;
    int whitecount = 0;

    //animation
    Vector3 originalRot;

    AudioSource AS;
    public AudioClip clip;

    //conditions (tells you what wire to cut. *note: there are two special cases)
    ConditionWire[] conditions = new ConditionWire[15];

    void Awake()
    {
        originalRot = transform.rotation.eulerAngles;
        //DEBUGGING ONLY UNTIL IMPLEMETED WITH MIKE
        serialNum = Random.Range(111111,999999).ToString();
        //DEBUGGING ONLY UNTIL IMPLEMETED WITH MIKE

        wireCount = Random.Range(3, 6);
        wireCounter = 0;
        colorCounter = 0;

        for (int i = 0; i < wireSlots.Length; i += 2)
        {
            float rnd = Random.Range(0, 1.0f);
            if (rnd > .5f && wireCounter < wireCount)
            {
                wireSlots[i].SetActive(true);
                wireCounter++;
            }
        }
        // i use two for loops here in an attempt to make the wire generation look more random.
        for (int i = 1; i < wireSlots.Length; i += 2)
        {
            float rnd = Random.Range(0, 1.0f);
            if (rnd > .5f && wireCounter < wireCount)
            {
                wireSlots[i].SetActive(true);
                wireCounter++;
            }
        }
        // fills in the gaps
        if (wireCounter < wireCount)
        {
            for (int i = 0; i < wireSlots.Length; i++)
            {
                if (wireCounter < wireCount && !wireSlots[i].activeInHierarchy)
                {
                    wireSlots[i].SetActive(true);
                    wireCounter++;
                }
            }
        }

    }

    void Start()
    {

        AS = GetComponent<AudioSource>();

        colorOrder = new Color[wireCount];//set it to the amount of wires
        foreach (GameObject wire in wireSlots)
        {
            if (wire.activeInHierarchy) // if the wire is active, then add this color to the array, now we have an array in order of the colors that appear on the wires
            {
                colorOrder[colorCounter] = wire.GetComponent<WireBehavior>().wireColor;
                activeWires.Add(wire);
                //Debug.Log(wire.GetComponent<WireBehavior>().wireColor);
                colorCounter++;
            }
        }

        DetermineCases();
        LoadConditions();
    }

    void LoadConditions()
    {
        conditions[0] = new ConditionWire(three_one, 1);
        conditions[1] = new ConditionWire(three_two, 2);
        conditions[2] = new ConditionWire(three_four, 2);

        conditions[3] = new ConditionWire(four_two, 0);
        conditions[4] = new ConditionWire(four_three, 0);
        conditions[5] = new ConditionWire(four_four, 3);
        conditions[6] = new ConditionWire(four_five, 1);

        conditions[7] = new ConditionWire(five_one, 3);
        conditions[8] = new ConditionWire(five_two, 0);
        conditions[9] = new ConditionWire(five_three, 1);
        conditions[10] = new ConditionWire(five_four, 0);

        conditions[11] = new ConditionWire(six_one, 2);
        conditions[12] = new ConditionWire(six_two, 3);
        conditions[13] = new ConditionWire(six_three, 5);
        conditions[14] = new ConditionWire(six_four, 3);
    }

    void DetermineCases() // sets up the cases in the manual
    {
        bluecount = 0;

        for (int i = 0; i < colorOrder.Length; i++) //more than one blue
        {
            if (colorOrder[i] == Color.blue)
            {
                bluecount++; // keeps track of blue wires
            }
        }

        redcount = 0;

        for (int i = 0; i < colorOrder.Length; i++) //more than one red
        {
            if (colorOrder[i] == Color.red)
            {
                redcount++; // keeps track of red wires
            }
        }

        yellowcount = 0;

        for (int i = 0; i < colorOrder.Length; i++) //more than one yllw
        {
            if (colorOrder[i] == Color.yellow)
            {
                yellowcount++; // keeps track of yellow wires
            }
        }

        blackcount = 0;

        for (int i = 0; i < colorOrder.Length; i++) //more than one black
        {
            if (colorOrder[i] == Color.black)
            {
                blackcount++; // keeps track of black wires
            }
        }

        whitecount = 0;

        for (int i = 0; i < colorOrder.Length; i++) //more than one white
        {
            if (colorOrder[i] == Color.white)
            {
                whitecount++; // keeps track of white wires
            }
        }

        if (wireCount == 3) // 3 wire cases
        {
            three_one = redcount==0;
            three_two = colorOrder[colorOrder.Length - 1] == Color.white; //last is white
            three_three = bluecount > 1;
            three_four = !three_one && !three_two && !three_three; //otherwise case (if none else)
        }
        else if (wireCount == 4) //four wires
        {
            four_one = redcount > 1 && int.Parse(serialNum[serialNum.Length].ToString()) % 2 != 0; // checks for red count and converts the last char of the serial string to an int to check if odd
            four_two = redcount == 0 && colorOrder[colorOrder.Length - 1] == Color.yellow;
            four_three = bluecount == 1;
            four_four = yellowcount > 1;
            four_five = !four_one && !four_two && !four_three && !four_four;
        }
        else if (wireCount == 5) //five wires
        {
            five_one = colorOrder[colorOrder.Length - 1] == Color.black && int.Parse(serialNum[serialNum.Length].ToString()) % 2 != 0;
            five_two = redcount == 1 && yellowcount > 1;
            five_three = blackcount == 0;
            five_four = !five_one && !five_two && !five_three;
        }
        else if (wireCount == 6) //six wires
        {
            six_one = yellowcount == 0 && int.Parse(serialNum[serialNum.Length].ToString()) % 2 != 0;
            six_two = yellowcount == 1 && whitecount > 1;
            six_three = redcount == 0;
            six_four = !six_one && !six_two && !six_three;
        }
    }

    void Update()
    {
        RaycastHit hit; //outs to hit so i can affect the wires
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //screen point to ray (mouse position)

        if (Physics.Raycast(ray, out hit))// if moused over
        {
            if (hit.collider.tag == "WIRE" && Input.GetKeyDown(KeyCode.Mouse0) && hit.collider.transform.parent.gameObject == GenerateBomb.SelectedModule)
            {
                AS.PlayOneShot(clip);
                hit.collider.gameObject.GetComponent<WireBehavior>().cut = true;
                hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                //transform.Rotate(-5,0,5);
                FailedOrCompleted();
                if (FAILED)
                {
                    StartCoroutine("FailFlash");
                }
            }
        }
        if (COMPLETED)
        {
            LED.material = lightMaterials[0];
        }
        //if (FAILED && !COMPLETED)
        //{
        //    LED.material = lightMaterials[1];
        //}

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(originalRot), Time.deltaTime * 20);

    }

    void FailedOrCompleted()
    {

        foreach (ConditionWire condition in conditions)
        {
            if (condition.condition == true)
            {
                if (activeWires[condition.wireToCut].gameObject.GetComponent<WireBehavior>().cut)
                {
                    COMPLETED = true;
                }
            }
        }

        if (!COMPLETED)
        {
            FAILED = true;
        }
    }

    public struct ConditionWire
    {
        public bool condition;
        public int wireToCut;

        public ConditionWire(bool _condition, int _wireToCut)
        {
            condition = _condition;
            wireToCut = _wireToCut;
        }
    }

    IEnumerator FailFlash()
    {
        LED.material = lightMaterials[1];
        yield return new WaitForSeconds(.5f);
        LED.material = lightMaterials[2];
    }
}
