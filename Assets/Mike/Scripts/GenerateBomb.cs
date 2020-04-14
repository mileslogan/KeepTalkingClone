using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class GenerateBomb : MonoBehaviour
{
    //prefabs
    public GameObject TimerModule;
    public GameObject EmptyModule;
    
    //variables to change
    public int ModuleAmount = 1;
    public int MaxModules = 5;
    
    //Set in inspector
    public List<GameObject> ModulesToSpawnFrom = new List<GameObject>();
    [FormerlySerializedAs("LocationsToSpawn")] public int[] ModuleLocToSpawn; //choose potential index to spawn from
    [FormerlySerializedAs("Locations")] public Vector3[] ModuleLocations; //actual location relative to object
    
    
    //debugging purposes
    public List<GameObject> ModulesToSpawn = new List<GameObject>();
    public List<GameObject> SpawnedModules = new List<GameObject>();
    
    //Strikes
    public int MaxStrikes = 2;
    private int CurrentStrikes = 0;
    
    
    //EXTERNAL MODULES // EXTRA->means extra elements that go on the sides of the bomb to be used in the puzzle modules
    public GameObject BatteryPrefab;
    public GameObject SerialPrefab;
    public GameObject IndicatorPrefab;
    
    //potential locations to spawn object->could expand to be 20 sections
    public static Area TopSide = new Area(new Vector3(-1f, 1f, -0.2f),new Vector3(1f, 1f, 0.2f), 
        Quaternion.Euler(0f,0f, 90f), Quaternion.Euler(0f,0f, 0f));
    
    public static Area BotSide = new Area(new Vector3(-1f, -1f, -0.2f),new Vector3(1f, -1f, 0.2f),
        Quaternion.Euler(0f,0f, 90f),Quaternion.Euler(0f,0f, 0f));
    
    public static Area LeftSide = new Area(new Vector3(1.5f, -.65f, -0.2f),new Vector3(1.5f, .65f, 0.2f),
        Quaternion.Euler(0f,0f, 0f),Quaternion.Euler(0f,0f, 90f));
    
    public static Area RightSide = new Area(new Vector3(-1.5f, -.65f, -0.2f),new Vector3(-1.5f, .65f, 0.2f),
        Quaternion.Euler(0f,0f, 0f),Quaternion.Euler(0f,0f, 90f));
    public Area[] BombSides = new Area[] {TopSide, BotSide, LeftSide, RightSide};
    
    
    //which objects to spawn, used in the function SpawnAllExtras()
    public List<GameObject> ExtrasToSpawn = new List<GameObject>();
    
    //serial number
    List<char> Serial = new List<char>();
    public string SerialN = "";
    
    //To be used by other modules in their puzzling solving
    public bool IsEven;
    public bool HasVowel;
    
    //Batteries
    public int MaxBatteries = 2;
    public int BatteryNum;

    //indicators
    public List<string> Indicators = new List<string>();
    private string[] IndicatorsToAdd = new[] {"SND", "CLR", "CAR","IND","FRQ","SIG","NSA","MSA","TRN","BOB","FRK"};

    public int MinIndicators = 1;
    public int MaxIndicators = 5;
    private float LikelihoodToBeOn = .6f;
    public List<Indicator> AddedIndicators = new List<Indicator>();
    
    
    // Start is called before the first frame update
    void Awake()
    {
        PickModules();
        //shuffle order of where each module is spawned
        RandFuncs.Shuffle(ModuleLocToSpawn);
        
        //Construct bomb
        SpawnModules();
        CreateSerial();
        AddIndicators();
        AddBatteries();

        //spawn extras: batteries, indicators, and serial #: potentially do: weird ports
        SpawnAllExtras();
    }

    // Update is called once per frame
    void Update()
    {
        //restart for debugging
        Restart();

        //Debug.Log(SerialN);
    }

    //choose modules to spawn from 
    void PickModules()
    {
        int i = Mathf.Min(ModuleAmount, MaxModules);
        while (i > 0)
        {
            int rand = Random.Range(0, ModulesToSpawnFrom.Count);
            ModulesToSpawn.Add(ModulesToSpawnFrom[rand]);
            ModulesToSpawnFrom.Remove(ModulesToSpawnFrom[rand]);
            i--;
        }
    }
    //spawn modules
    void SpawnModules()
    {
        //go through all modules and spawn empty or module
        for(int i = 0; i < ModuleLocToSpawn.Length; i++)
        {
            int index = ModuleLocToSpawn[i];
            //Debug.Log(index);
            Vector3 location = ModuleLocations[index];
            GameObject module = null;
            if (i < ModulesToSpawn.Count)
            {
                module = ModulesToSpawn[i];
            }
            GameObject spawned;
            //check if module exists
            if (module != null)
            {
                spawned = Instantiate(module, transform.position + location, Quaternion.identity);
                //spawned.transform.parent = transform;
                SpawnedModules.Add(spawned);
            }
            //if no module, spawn empty module
            else
            {
                spawned = Instantiate(EmptyModule, transform.position + location, Quaternion.identity);
                spawned.transform.parent = transform;
            }
            
        }
        //spawn timer: always spawns at top middle of front: can be adjusted if need be
        GameObject timer = Instantiate(TimerModule, transform.position + ModuleLocations[1], Quaternion.identity);
        timer.transform.parent = transform;
    }

    
    //spawn batteries, indicators, and serial #, consider spawning ports
    void SpawnAllExtras()
    {
        int indicatorIndex = 0;
        for (int i = 0; i < ExtrasToSpawn.Count; i++)
        {
            //special spawn for indicators
            if (ExtrasToSpawn[i] == IndicatorPrefab)
            {
                SpawnIndicator(indicatorIndex);
                indicatorIndex++;
            }
            //normal spawn
            else
            {
                SpawnExtra(ExtrasToSpawn[i]);
            }
            
        }
    }

    //used by SpawnAllExtras as well as RespawnSelf to individual spawn each extra
    public GameObject SpawnExtra(GameObject prefab)
    {
        Area side = BombSides[Random.Range(0, BombSides.Length)];
        Vector3 loc = RandomVector3(side);

        GameObject extra = null;
        //GameObject
        if (prefab == SerialPrefab)
        {
            extra = Instantiate(prefab, transform.position + loc, side.SerialNumRotation);
        }
        else if (prefab == BatteryPrefab)
        {
            extra = Instantiate(prefab, transform.position + loc, side.BatteryRotation);
        }
        else if (prefab == IndicatorPrefab)
        {
            extra = Instantiate(prefab, transform.position + loc, side.SerialNumRotation);
        }
        
        if (extra != null)
        {
            extra.transform.parent = transform;
            
        }

        return extra;
    }

    //spawn indicator and set variables
    public void SpawnIndicator(int index)
    {
        GameObject indicator = SpawnExtra(IndicatorPrefab);
        IndicatorBehavior indScript = indicator.GetComponent<IndicatorBehavior>();
        
        //set variables of 3-letter string and whether it is on or off
        indScript.SetText(AddedIndicators[index].Str);
        indScript.SetLight(AddedIndicators[index].IsOn);
        indScript.IndicatorIndex = index;
    }
    

    //randomly create a location given a range
    Vector3 RandomVector3(Area range)
    {
        float x = Random.Range(range.BotLeftCorner.x, range.TopRightCorner.x);
        float y = Random.Range(range.BotLeftCorner.y, range.TopRightCorner.y);
        float z = Random.Range(range.BotLeftCorner.z, range.TopRightCorner.z);
        
        return new Vector3(x,y,z);
    }
    
    //debugging restart scene
    public void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }

    //strike occurs, used when a module fails-other prefab modules will use this function
    public void BombStrikes()
    {
        CurrentStrikes++;
        if (CurrentStrikes > MaxStrikes)
        {
            //Game Over
            GameOver();
        }
    }

    //create an explosion sfx, and black out screen, game over
    public void GameOver()
    {
        
    }
    
    
    public void CreateSerial()
    {
        //randomly select five chars [at least 1 number and 2 letters]

        //guarantee two letters and one number
        Serial.Add(RandomLetter());
        Serial.Add(RandomLetter());
        Serial.Add(RandomNum(false));
        
        //Pick 2 more nums or letters
        for (int i = 0; i < 2; i++)
        {
            
            if (Random.Range(0f, 1f) < .5f)
            {
                Serial.Add(RandomLetter());
            }
            else
            {
                Serial.Add(RandomNum(false));
            }
        }
        
        //shuffle the first 5 chars
        RandFuncs.Shuffle(Serial);
        
        //add final number to the string and check if it is even
        Serial.Add(RandomNum(true));
        foreach (char c in Serial)
        {
            SerialN += c;
        }
        //add serial number to list to be spawned in extras
        ExtrasToSpawn.Add(SerialPrefab);
        
    }

    //pick random letter to be used for serial # creation
    public char RandomLetter()
    {
        int letter = Random.Range(0, 26);
        
        //Repick a number that does not correspond with the letter Y
        while (letter == 24)
        {
            letter = Random.Range(0, 26);
        }
        //letter
        char c = (char) ('A' + letter);
        //check if is a vowel
        if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
        {
            HasVowel = true;
        }
        return c;
    }
    
    
    //pick random number to be used for serial # creation
    public char RandomNum(bool checkForEven)
    {
        int num = Random.Range(0, 10);
        if (checkForEven && num % 2 == 0)
        {
            IsEven = true;
        }
        return num.ToString()[0];
    }

    //add indicators to be spawned
    void AddIndicators()
    {
        foreach (string indicator in IndicatorsToAdd)
        {
            Indicators.Add(indicator);
        }
        
        //min range is 1 to MaxIndicators
        int indicNums = Random.Range(MinIndicators, MaxIndicators + 1);
        
        //create the number of indicators and add to list
        for(int i = 0; i < indicNums; i++)
        {
            Indicator indic = new Indicator();
            //pick one of the indicator strings
            int index = Random.Range(0, Indicators.Count);
            string str = Indicators[index];
            Indicators.Remove(str);
            indic.Str = str;
            //randomize if light is on or off
            indic.IsOn = Random.Range(0f, 1f) < LikelihoodToBeOn;
            AddedIndicators.Add(indic);
            ExtrasToSpawn.Add(IndicatorPrefab);
        }
    }
    
    //spawning batteries-0 to 2
    //future: 2 variations of batteries
    void AddBatteries()
    {
        //randomize batteries-0 to 2 batteries-currently equal likelihood
        int batteryNum = Random.Range(0, MaxBatteries + 1);
        BatteryNum = batteryNum;
        
        //add battery prefab to be spawned
        for (int i = 0; i < BatteryNum; i++)
        {
            ExtrasToSpawn.Add(BatteryPrefab);
        }
        
    }
}

//to be used for indicator spawning
public struct Indicator
{
    public string Str; //what indicator says
    public bool IsOn; //if indicator is on
}

//3d area of object to be used to randomize spawning for extras
public struct Area
{
    public Vector3 BotLeftCorner; 
    public Vector3 TopRightCorner;
    public Quaternion BatteryRotation;
    public Quaternion SerialNumRotation;
    public Area(Vector3 botLeft, Vector3 topRight, Quaternion batteryRot, Quaternion serialNumRotation )
    {
        this.BotLeftCorner = botLeft;
        this.TopRightCorner = topRight;
        this.BatteryRotation = batteryRot;
        this.SerialNumRotation = serialNumRotation;
    }
}
