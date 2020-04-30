using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BombLerp : MonoBehaviour
{
    //structure for bomb variables: location and rotation
    [System.Serializable]
    public struct BombSpot
    {
        public Vector3 Loc; //Bomb location
        public Quaternion Rot; //Bomb rotation
        //Constructor
        public BombSpot(Vector3 loc, Quaternion rot) {
            this.Loc = loc;
            this.Rot = rot;
        }
    }
    
    private BombSpot StartingSpot; //current location of bomb
    public BombSpot DefaultSpot; //starting location, on the table flat
    public BombSpot DefaultBackSpot; //starting location but flipped
    public BombSpot NewSpot; //location to lerp towards

    [FormerlySerializedAs("PickUpSpot")] public BombSpot PickUpFrontSpot = new BombSpot(new Vector3(0f, 1.5f, -2.5f), Quaternion.Euler(19f, 0f, 0f) ); //initial picking up bomb spot
    
    public BombSpot PickUpBackSpot = new BombSpot(new Vector3(0f, 1.5f, -2.5f), Quaternion.Euler(-19f, 0f, 0f) ); //initial picking up bomb spot
    public BombSpot FlipSpot = new BombSpot(new Vector3(0f, 1.5f, -2.5f), Quaternion.Euler(-11f, 180f, 0f) ); //initial picking up bomb spot

    public bool IsFrontSide = true;
    
    // LERPING //
    private bool CanLerp = false; 
    private float Timer; //used to lerp
    public float LerpTime = .5f; //speed of lerp to completion

    private GenerateBomb BombScript;

    private bool HasDefault; //Get Default spot
    // Start is called before the first frame update
    void Start()
    {
        BombScript = FindObjectOfType<GenerateBomb>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //do nothing until bomb is done rotating
        if (!BombScript.HasRotated)
        {
            return;
        }
        else
        {
            if (!HasDefault)
            {
                DefaultSpot.Loc = transform.position;
                DefaultSpot.Rot = transform.rotation;
                //set backside
                DefaultBackSpot = DefaultSpot;
                Vector3 newRot = DefaultSpot.Rot.eulerAngles;
                DefaultBackSpot.Rot = Quaternion.Euler(-newRot.x, newRot.y + 180f, newRot.z);
                
                HasDefault = true;
            }
        }

        if (CanLerp)
        {
            Timer += Time.deltaTime / LerpTime;
            float t = Timer;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.position = Vector3.Lerp(StartingSpot.Loc, NewSpot.Loc, t);
            transform.rotation = Quaternion.Lerp(StartingSpot.Rot, NewSpot.Rot, t);
            //Cam.fieldOfView = Mathf.Lerp(InitialFov, NewSpot.FieldOfView, t);
            if (t >= 1)
            {
                CanLerp = false;
            }
        }

        
    }
    
    
    
    public void LerpCamera(BombSpot cam, float lerpTime)
    {
        StartingSpot.Loc = transform.position;
        StartingSpot.Rot = transform.rotation;
        NewSpot = cam;
        CanLerp = true;
        Timer = 0f;
        LerpTime = lerpTime;
    }

    public void PutDown()
    {
        if (IsFrontSide)
        {
            LerpCamera(DefaultSpot, .5f);
        }
        else
        {
            LerpCamera(DefaultBackSpot, .5f);
        }
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlaySoundDelay("Bump",false,.3f));
    }

    public void PickUp()
    {
        if (IsFrontSide)
        {
            LerpCamera(PickUpFrontSpot, .5f);
        }
        else
        {
            LerpCamera(PickUpBackSpot, .5f);
        }
    }
    
    
}


