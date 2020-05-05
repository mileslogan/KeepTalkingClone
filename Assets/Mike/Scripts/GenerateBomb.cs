using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GenerateBomb : MonoBehaviour
{
    //done loading
    private bool IsDone = false;
    public bool HasRotated = false;
    public float DoneTimer = .3f;
    //prefabs
    public GameObject TimerModule;
    public GameObject EmptyModule;
    public GameObject SpawnedTimer;
    
    //Canvas dark:
    public Image Panel;
    //variables to change
    public int ModuleAmount = 1;
    private int MaxModules = 11; //max possible number of modules: 11-don't change
    
    //Set in inspector
    public List<GameObject> ModulesToSpawnFrom = new List<GameObject>();
    public List<GameObject> SecondaryModulesToSpawnFrom = new List<GameObject>();
    [FormerlySerializedAs("LocationsToSpawn")] public int[] ModuleLocToSpawn; //choose potential index to spawn from
    [FormerlySerializedAs("Locations")] public Vector3[] ModuleLocations; //actual location relative to object
    
    
    //debugging purposes
    public List<GameObject> ModulesToSpawn = new List<GameObject>();
    public List<GameObject> SpawnedModules = new List<GameObject>();
    
    
    //scripts to get
    private BombSounds SoundScript;
    private MouseControl MouseScript;
    private SceneManage ManagerScript;
    public Timer TimerScript;
    //states
    public bool IsGameOver;

    public bool IsGameWon;
    //Strikes
    public int MaxStrikes = 2;
    
    //ACCESSED BY CERTAIN MODULES-TIMER
    public int CurrentStrikes = 0;
    public static ModuleTypes LostOnThisModule = ModuleTypes.Null;

    public enum ModuleTypes
    {
        Wire,
        SimonSays,
        WhosOnFirst,
        Memory,
        Keypad,
        Button,
        Null,
    }
    
    
    //EXTERNAL MODULES // EXTRA->means extra elements that go on the sides of the bomb to be used in the puzzle modules
    public GameObject BatteryPrefab;
    public GameObject SerialPrefab;
    public GameObject IndicatorPrefab;
    
    
    //potential locations to spawn object
    
    public SpawnArea[] TopSideSpots;
    public SpawnArea[] BotSideSpots;
    public SpawnArea[] LeftSideSpots;
    public SpawnArea[] RightSideSpots;

    public List<SpawnArea> AllSpots = new List<SpawnArea>();
    
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
    
    
    //VARIABLES USED BY OTHER MODULES//
    //serial number
    List<char> Serial = new List<char>();
    public string SerialN = ""; //bomb serial number
    
    //To be used by other modules in their puzzling solving
    public bool IsEven; //has an even number in its final slot
    public bool HasVowel; //has a vowel in the serial number
    
    //Batteries
    public int MaxBatteries = 2;
    public int BatteryNum; //number of batteries on bomb

    //indicators
    public List<string> Indicators = new List<string>();
    private string[] IndicatorsToAdd = new[] {"SND", "CLR", "CAR","IND","FRQ","SIG","NSA","MSA","TRN","BOB","FRK"};

    public int MinIndicators = 1;
    public int MaxIndicators = 5;
    private float LikelihoodToBeOn = .6f;
    public List<Indicator> AddedIndicators = new List<Indicator>(); //indicators on bomb: to get see if isOn: AddedIndicators[0].IsOn
    
    
    
    //module selected
    public static GameObject SelectedModule = null; //currently selected module: which enables interaction[clicking]

    public GameObject SelectMod = null;
    
    //completed
    public int ModulesLeftToComplete;
    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        //Manager script
        ManagerScript = FindObjectOfType<SceneManage>();
        ManagerScript.fadeAnim.SetInteger("FadeState", 1);
        
        
        
        //Set components based on menu: change number of modules
        ModuleAmount = ManagerScript.numModules;
        
        
        MouseScript = GetComponent<MouseControl>();
        SoundScript = GetComponent<BombSounds>();
        PickModules();
        //shuffle order of where each module is spawned
        RandFuncs.Shuffle(ModuleLocToSpawn);
        
        //Construct bomb variables
        CreateSerial();
        AddIndicators();
        AddBatteries();
        //spawn modules
        SpawnModules();
        //spawn extras: batteries, indicators, and serial #: potentially do: weird ports
        SpawnAllExtras();
        ModulesLeftToComplete = ModuleAmount;
        
        //set time
        TimerScript = FindObjectOfType<Timer>();
        TimerScript.countdowntime = ManagerScript.defuseTime;
        
        //set up all indicator spots: shuffle
        RandFuncs.Shuffle(AllSpots);
    }
    // Update is called once per frame
    void Update()
    {
        //time ran out
        if (Timer.timeleft <= 0)
        {
            GameOver();
        }
        
        //if game is won, stop timer
        if (IsGameWon)
        {
            //StartCoroutine(TimerScript.Blink());
            //TimerScript.waittime = 0.5f;
            //Win();
            
        }
        
        //once done creating bomb, rotate it
        if (IsDone && !HasRotated)
        {
            transform.parent.Rotate(Vector3.right, 90f);
            HasRotated = true;
        }
        else
        {
            if (DoneTimer <= 0f)
            {
                
                IsDone = true;
            }
            else
            {
                DoneTimer -= Time.deltaTime;
            }
            
            
        }
        
        //restart for debugging
        Restart();

        //Debug.Log(SerialN);
    }

    //choose modules to spawn from 
    void PickModules()
    {
        int i = Mathf.Min(ModuleAmount, MaxModules);
        int j = i - 6;
        while (i > 0 && i > j)
        {
            int rand = Random.Range(0, ModulesToSpawnFrom.Count);
            ModulesToSpawn.Add(ModulesToSpawnFrom[rand]);
            ModulesToSpawnFrom.Remove(ModulesToSpawnFrom[rand]);
            i--;
        }
        
        
        //more than 6 modules, than duplicates are possible
        while (j > 0)
        {
            int rand = Random.Range(0, SecondaryModulesToSpawnFrom.Count);
            ModulesToSpawn.Add(SecondaryModulesToSpawnFrom[rand]);
            SecondaryModulesToSpawnFrom.Remove(SecondaryModulesToSpawnFrom[rand]);
            j--;
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
                spawned.transform.parent = transform.parent;
                spawned.transform.Rotate(Vector3.right, 270f);
                SpawnedModules.Add(spawned);
                
                
                //indicate which location the module on the script attached
                
                //back side of bomb
                if (index >= 6)
                {
                    spawned.transform.Rotate(180f,180f,0f);
                }
            }
            //if no module, spawn empty module
            else
            {
                spawned = Instantiate(EmptyModule, transform.position + location, Quaternion.identity);
                spawned.transform.parent = transform.parent;
                
                //back side of bomb
                if (index >= 6)
                {
                    
                    //spawned.transform.Rotate(180f,0f,0f);
                    spawned.transform.Translate(0f, 0f, -.05f);

                    if (index >= 9)
                    {
                        spawned.transform.Translate(0f, .02f, 0f);
                    }
                }
                else
                {
                    spawned.transform.Rotate(180f,0f,0f);
                    spawned.transform.Translate(0f, 0f, -.1f);
                }

                spawned.name = "Null " + index.ToString();
            }
            
            
            
        }
        //spawn timer: always spawns at top middle of front: can be adjusted if need be
        GameObject timer = Instantiate(TimerModule, transform.position + ModuleLocations[1], Quaternion.identity);
        timer.transform.parent = transform.parent;
        SpawnedTimer = timer;
    }

    
    //spawn batteries, indicators, and serial #, consider spawning ports
    void SpawnAllExtras()
    {
        int indicatorIndex = 0;
        for (int i = 0; i < ExtrasToSpawn.Count; i++)
        {
            //Spawn indicators
            Vector3 loc = AllSpots[i].SpawnLoc;
            Quaternion rot = Quaternion.Euler(AllSpots[i].Rotation);

            GameObject extra = Instantiate(ExtrasToSpawn[i], loc, rot);
            
            if (ExtrasToSpawn[i] == IndicatorPrefab)
            {
                IndicatorBehavior indScript = extra.GetComponent<IndicatorBehavior>();
        
                //set variables of 3-letter string and whether it is on or off
                indScript.SetText(AddedIndicators[indicatorIndex].Str);
                indScript.SetLight(AddedIndicators[indicatorIndex].IsOn);
                indScript.IndicatorIndex = indicatorIndex;
                indicatorIndex++;
            }
            extra.transform.parent = transform.parent;
            
            //special spawn for indicators
            if (ExtrasToSpawn[i] == IndicatorPrefab)
            {
                //SpawnIndicator(indicatorIndex);
                //indicatorIndex++;
            }
            //normal spawn
            else
            {
                //SpawnExtra(ExtrasToSpawn[i]);
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
            extra.transform.parent = transform.parent;
            
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

    public void SpawnIndicator()
    {
        
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
        if (CurrentStrikes > MaxStrikes && !IsGameOver)
        {
            //Game Over
            GameOver();
            Debug.Log("Game Over");
        }
        else
        {
            AudioManager.Instance.PlayOneShotSound("Wrong", false);
        }
    }

    public void Completed()
    {
        ModulesLeftToComplete--;
        if (ModulesLeftToComplete <= 0 && !IsGameWon)
        {
            //TimerScript.StopAllCoroutines(); //stop coroutine
            Win();
            Debug.Log("Win");
        }
        else
        {
            
        }
    }
    
    //create an explosion sfx, and black out screen, game over
    public void GameOver()
    {
        IsGameOver = true;
        string sfx = SoundScript.Explosions[Random.Range(0, SoundScript.Explosions.Length)];
        AudioManager.Instance.PlayOneShotSound(sfx, true);
        Panel.color = Color.black;
        
        SpawnedTimer.GetComponent<AudioSource>().mute = true; //turn off blinking
        AudioSource audio = FindObjectOfType<MusicPlayer>().gameObject.GetComponent<AudioSource>(); //turn off music
        if (audio != null)
        {
            audio.mute = true;
        }
        //determine cause of death
        if (Timer.timeleft <= 0f)
        {
            ManagerScript.causeOfDeath = "Time Ran out";
        }
        else
        {
            //Lost on this module
            LostOnThisModule = SelectedModule.GetComponentInChildren<ClickModule>().ModuleType;
            switch (LostOnThisModule)
            {
                case ModuleTypes.Button:
                    ManagerScript.causeOfDeath = "The Button";
                    break;
                case ModuleTypes.Keypad:
                    ManagerScript.causeOfDeath = "Keypad";
                    break;
                case ModuleTypes.Memory:
                    ManagerScript.causeOfDeath = "Memory";
                    break;
                case ModuleTypes.Wire:
                    ManagerScript.causeOfDeath = "Wires";
                    break;
                case ModuleTypes.SimonSays:
                    ManagerScript.causeOfDeath = "Simon Says";
                    break;
                case ModuleTypes.WhosOnFirst:
                    ManagerScript.causeOfDeath = "Who's on First";
                    break;
            }
        }
        
        ManagerScript.defused = false;
        ManagerScript.timeLeft = (int)Timer.timeleft;
        StartCoroutine(EndScene(2f));
        //Camera.main.enabled = false;
    }

    //win screen
    public void Win()
    {
        ManagerScript.timeLeft = (int)Timer.timeleft;
        ManagerScript.defused = true;
        ManagerScript.causeOfDeath = "Nothing. You're Alive!";
        AudioSource audio = FindObjectOfType<MusicPlayer>().gameObject.GetComponent<AudioSource>();//turn off music
        if (audio != null)
        {
            audio.mute = true;
        }
        IsGameWon = true;
        StopCoroutine(TimerScript.CountDown(TimerScript.countdowntime)); //pause time
        AudioManager.Instance.PlayOneShotSound("Correct", false);
        //AudioManager.Instance.PlayOneShotSound("Correct", false);
        AudioManager.Instance.PlaySoundDelay("Correct", false, 1.7f);
        StartCoroutine(EndScene(5f));
        ManagerScript.ToEndFunction();
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

    public void TurnOnAllCols()
    {
        foreach (GameObject obj in SpawnedModules)
        {
            obj.GetComponentInChildren<ClickModule>().gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    public void BombShake()
    {
        MouseScript.ShakeBomb();
    }


    IEnumerator EndScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        ManagerScript.ToEndFunction();
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

[Serializable]
//new spawn areas
public struct SpawnArea
{
    public Vector3 SpawnLoc;
    public Vector3 Rotation;
}



