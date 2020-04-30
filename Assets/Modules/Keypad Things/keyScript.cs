using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScript : MonoBehaviour
{
    public ArrayList symbols = new ArrayList();

    public GenerateBomb BombScript;

    public GameObject key1;
    public GameObject key2;
    public GameObject key3;
    public GameObject key4;

    public Animator anim1;
    public Animator anim2;
    public Animator anim3;
    public Animator anim4;

    public AudioSource clicker;
    public AudioClip click;

    public Material[] keylightMaterials;     public MeshRenderer keyLED;

    public List<SpriteRenderer> keySymbols;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit keyHit;
            if (Physics.Raycast(ray, out keyHit) && (BombScript == null || gameObject == GenerateBomb.SelectedModule))
            {
                clicker.PlayOneShot(click);

                if (keyHit.transform.gameObject == key1)
                {
                   
                    anim1.SetTrigger("Push");

                    SymbolTestFunction(keySymbols[0].sprite);

                }
                if (keyHit.transform.gameObject == key2)
                {

                    anim2.SetTrigger("Push");
                    SymbolTestFunction(keySymbols[1].sprite);
                }
                if (keyHit.transform.gameObject == key3)
                {

                    anim3.SetTrigger("Push");
                    SymbolTestFunction(keySymbols[2].sprite);
                }
                if (keyHit.transform.gameObject == key4)
                {

                    anim4.SetTrigger("Push");
                    SymbolTestFunction(keySymbols[3].sprite);

                }

            }

            }
        }

    // Start is called before the first frame update
    void Start()
    {
        BombScript = FindObjectOfType<GenerateBomb>();

        int chosenList = Random.Range(0, 6);
         

        if(chosenList == 0)
        {
            for(int i = 0; i < column1.Length; i++)
            {
                sprites.Add(column1[i]);

            }
            Debug.Log("Adding Column 1");
        }
        if (chosenList == 1)
        {
            for (int i = 0; i < column2.Length; i++)
            {
                sprites.Add(column2[i]);

            }
            Debug.Log("Adding Column 2");
        }
        if (chosenList == 2)
        {
            for (int i = 0; i < column3.Length; i++)
            {
                sprites.Add(column3[i]);

            }
            Debug.Log("Adding Column 3");
        }
        if (chosenList == 3)
        {
            for (int i = 0; i < column4.Length; i++)
            {
                sprites.Add(column4[i]);

            }
            Debug.Log("Adding Column 4");
        }
        if (chosenList == 4)
        {
            for (int i = 0; i < column5.Length; i++)
            {
                sprites.Add(column5[i]);

            }
            Debug.Log("Adding Column 5");
        }
        if (chosenList == 5)
        {
            for (int i = 0; i < column6.Length; i++)
            {
                sprites.Add(column6[i]);

            }
            Debug.Log("Adding Column 6");
        }



        for (int j = 0; j < 4; j++)  //assigning sprites to the slots for each button
        {
            int chosenSymbols = Random.Range(0, 7); //selecting one of the seven chosen symbols
            while (activeSprites.Contains(sprites[chosenSymbols])) //while the active sprite list does contain the chosen symbol...
            {
                chosenSymbols = Random.Range(0, 7); //pick a new symbol
            }

            activeSprites.Add(sprites[chosenSymbols]); //add it to the list of active sprites

            keySymbols[j].sprite = activeSprites[j]; //assigning sprites to the keypad buttons
        }



        for (int k = 0; k < activeSprites.Count; k++)
        {

            correctOrder.Add(sprites.IndexOf(activeSprites[k])); //create list in the order it's in
        }

        correctOrder.Sort();
     
    }

    //Make an array for each of the six columns

    public Sprite[] column1;
    public Sprite[] column2;
    public Sprite[] column3;
    public Sprite[] column4;
    public Sprite[] column5;
    public Sprite[] column6;


    //Select a column from among the six

    public List<Sprite> sprites;
    public List<Sprite> activeSprites;
    public List<Sprite> correctSprites;
    public List<int> correctOrder;

    //Select a random combination of four symbols from the selected column, no repeats

    //Click the symbols in the correct order

    //If incorrect, debug "strike"

    //Mark "correct" for all four correct clicks

    public int counter = 0; //it will only go up if the correct symbols are clicked, will not decrease

    public void SymbolTestFunction(Sprite symbolSlot)
    {
        //take the sprite from the key that's presssed and compare its place in the order of the list
        sprites.IndexOf(symbolSlot);
        Debug.Log("Getting the position of the symbol in its original column: " + sprites.IndexOf(symbolSlot));
        //if it's the lowest order, it's correct


            if (sprites.IndexOf(symbolSlot) == correctOrder[0])
            {
                counter++;
                Debug.Log("CORRECT!");
                correctOrder.RemoveAt(0);
               if (counter == 4)

            {
                Debug.Log("YOU WIN!!");
                //Global Counter++ (?)
                keyLED.material = keylightMaterials[0];
                if (BombScript != null)
                {
                    BombScript.Completed(); //if you are completing the module
                }

            }
            return;
            }

            else
            {
                Debug.Log("incorrect!");

            //Global Strike
            StartCoroutine("FailFlash");
            if (BombScript != null)
            {
                BombScript.BombStrikes(); //if you made a mistake i.e. pressed wrong button
            }


        }
        
    }

    IEnumerator FailFlash()     { 
        keyLED.material = keylightMaterials[1];         
        yield return new WaitForSeconds(.5f);         
        keyLED.material = keylightMaterials[2];
        
    } 
}

