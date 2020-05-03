using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Lerp : MonoBehaviour

{
    private Camera Cam;
    
    //6 locations for the modules: does not zoom in IF TIMER MODULE
    public CameraSpot[] ModuleSpots;
    
    //initial pick up bomb spot
    public CameraSpot HoldBombSpot = new CameraSpot(new Vector3(0f, 3.76f, -10f), 
        23.55f, Vector3.zero);
    

    public CameraSpot DefaultSpot; //original spot of camera
    
    private CameraSpot NewSpot; //spot to lerp to when CanLerp is true
    private bool CanLerp = false; //used to determine if lerping
    private float StartingFov; //initial lerp spot the moment camera begins to lerp
    private Vector3 StartingPos;
    
    //used for speed of lerp
    private float Timer; 
    public float LerpTime = 2f;
    
    //Get scripts
    private GenerateBomb BombScript;
    private BombLerp BombLerpScript;
    private MouseControl MouseScript;
    // Start is called before the first frame update
    void Start()
    {
        MouseScript = FindObjectOfType<MouseControl>();
        BombScript = FindObjectOfType<GenerateBomb>();
        BombLerpScript = FindObjectOfType<BombLerp>();
        Cam = GetComponent<Camera>();
        StartingFov = Cam.fieldOfView;
        StartingPos = transform.position;
        DefaultSpot = new CameraSpot(transform.position, Cam.fieldOfView, transform.rotation.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        //see if can lerp
        if (CanLerp)
        {
            
            Timer += Time.deltaTime / LerpTime;
            float t = Timer;
            t = t * t * t * (t * (6f * t - 15f) + 10f); //Smoother step lerp
            
            
            transform.position = Vector3.Lerp(StartingPos, NewSpot.Loc, t);
            Cam.fieldOfView = Mathf.Lerp(StartingFov, NewSpot.FieldOfView, t);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, NewSpot.RotationAngles, t));
            //turn of lerping once done
            if (t >= 1)
            {
                CanLerp = false;
            }
        }
    }

    //lerp camera based on input CameraSpot and lerptime(speed of lerp)
    public void LerpCamera(CameraSpot cam, float lerpTime)
    {
        StartingFov = Cam.fieldOfView;
        StartingPos = transform.position;
        NewSpot = cam;
        CanLerp = true;
        Timer = 0f;
        LerpTime = lerpTime;
    }
    
    //lerp to the location based on module index
    public void LerpToModule(int index, GameObject obj)
    {
        
        if (MouseScript.CurrentState == MouseControl.BombStates.PickedUp 
            || MouseScript.CurrentState == MouseControl.BombStates.OnModule)
        {
            LerpCamera(ModuleSpots[index], .3f);
            MouseScript.CurrentState = MouseScript.CurrentState = MouseControl.BombStates.OnModule;
     
            //front side
            if (index < 6)
            {
                BombLerpScript.LerpCamera(BombLerpScript.PickUpFrontSpot, .5f); 
            }
            else//back side
            {

                BombLerpScript.LerpCamera(BombLerpScript.FlipSpot, .5f); 
            }
            
            //declare which object is currently on
            StartCoroutine(DelayObj(.05f, obj));
            
            //GenerateBomb.SelectedModule = obj;
            //BombScript.SelectMod = GenerateBomb.SelectedModule;


        }
        
    }

    public void LerpGivenObject(GameObject obj)
    {
        //go through spawned modules to figure out index of module
        for (int i = 0; i < BombScript.SpawnedModules.Count; i++)
        {
            //see which object it is
            if (BombScript.SpawnedModules[i] == obj)
            {
                //lerp to the location based on module index
                LerpToModule(BombScript.ModuleLocToSpawn[i], obj);
                
                //Turn off collider
                BombScript.TurnOnAllCols();
                //obj.GetComponent<Collider>().enabled = false;
                obj.GetComponentInChildren<ClickModule>().gameObject.GetComponent<Collider>().enabled = false;
                Debug.Log(obj);
                return;
            }
        }
        Debug.Log("Error: object clicked was not the same");
        
    }
    
    IEnumerator DelayObj(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        GenerateBomb.SelectedModule = obj;
        BombScript.SelectMod = GenerateBomb.SelectedModule;
    }
}

[System.Serializable]
public struct CameraSpot
{
    public Vector3 Loc; //move camera
    public float FieldOfView; //camera zoom

    public Vector3 RotationAngles;
    //Constructor
    public CameraSpot(Vector3 loc, float fieldOfView, Vector3 rot) {
        this.Loc = loc;
        this.FieldOfView = fieldOfView;
        this.RotationAngles = rot;
    }
}
