using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class MouseControl : MonoBehaviour
{
    
    public enum BombStates {OnTable, PickedUp, OnModule}

    public BombStates CurrentState = BombStates.OnTable;
    
    private GenerateBomb BombScript;
    private Lerp LerpScript;
    private Transform ParentBomb;

    private BombLerp BombLerpScript;

    private float HoldingDownTimer; //holding down right mouse click

    
    
    // Rotation //
    public float RotationSpeed;
    [FormerlySerializedAs("MouseSensitivity")] public float RotSpeed = 10f;

    public float MinXRot = -77f;

    public float MaxXRot = 109f;
    // Start is called before the first frame update
    void Start()
    {
        BombScript = GetComponent<GenerateBomb>();
        ParentBomb = BombScript.gameObject.transform.parent;
        LerpScript = FindObjectOfType<Lerp>();
        BombLerpScript = FindObjectOfType<BombLerp>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //check current bomb state to determine potential actions
        //may need adjusting if there is a menu
        //TO DO EVEN WHEN NOT HOVERING
        switch (CurrentState)
        {
            case BombStates.PickedUp:
                if (Input.GetMouseButton(1)) //Right click
                {
                   
                    HoldingDownTimer += Time.deltaTime; //time holding down
                    
                    //can rotate bomb now->get input
                    float mouseX = Input.GetAxis("Mouse X") * RotSpeed * Time.deltaTime;
                    float mouseY = Input.GetAxis("Mouse Y") * RotSpeed * Time.deltaTime;

                    
                    //Clamp upward/downward rotation
                    Vector3 newRot = ParentBomb.localRotation.eulerAngles + new Vector3(mouseY, -mouseX, 0f);
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        Debug.Log(ParentBomb.eulerAngles);
                    }
                    if (ParentBomb.eulerAngles.y >= 90f && ParentBomb.eulerAngles.y <= 270)
                    {
                        newRot = ParentBomb.localRotation.eulerAngles + new Vector3(-mouseY, -mouseX, 0f);
                    }
                    
                    newRot.x = ClampAngle(newRot.x, MinXRot, MaxXRot);
                    
                    //newRot.x = Mathf.Clamp(newRot.x, MinXRot, MaxXRot);
                    ParentBomb.eulerAngles = newRot;

                }
                else
                {
                    if (HoldingDownTimer <= .15f && HoldingDownTimer > 0f) //needs to be less than this to be considered a quick click
                    {
                        BombLerpScript.LerpCamera(BombLerpScript.DefaultSpot, .5f);
                        LerpScript.LerpCamera(LerpScript.DefaultSpot, .4f);
                        //Quaternion.Lerp()
                        CurrentState = BombStates.OnTable; //switch state
                    }
                    HoldingDownTimer = 0f;
                }
                break;
            case BombStates.OnModule:
                if (Input.GetMouseButton(1))
                {
                    HoldingDownTimer += Time.deltaTime; //time holding down
                }
                else
                {
                    if (HoldingDownTimer <= .15f && HoldingDownTimer > 0f) //needs to be less than this to be considered a quick click
                    {
                        //lerp to picking up bomb state
                        BombLerpScript.LerpCamera(BombLerpScript.PickUpSpot, .3f); //MAY NEED TO ADD ANOTHER ONE IF ON BACKSIDE
                        LerpScript.LerpCamera(LerpScript.HoldBombSpot, .2f);
                        
                        
                        
                        //Quaternion.Lerp()
                        CurrentState = BombStates.PickedUp; //switch state
                        GenerateBomb.SelectedModule = null;
                        BombScript.SelectMod = GenerateBomb.SelectedModule;
                    }
                    HoldingDownTimer = 0f;
                }
                
                break;
        }
    }

    private void OnMouseOver()
    {
        //check current bomb state to determine potential actions
        switch (CurrentState)
        {
            case BombStates.OnTable:
                if (Input.GetMouseButtonDown(0)) //Left click
                {
                    //transform.parent.Rotate(Vector3.right, -71f);
                    
                    //lerp to picking up bomb state
                    BombLerpScript.LerpCamera(BombLerpScript.PickUpSpot, .5f);
                    LerpScript.LerpCamera(LerpScript.HoldBombSpot, .4f);
                    //Quaternion.Lerp()
                    CurrentState = BombStates.PickedUp; //switch state
                }
                break;
        }
        
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f)
        {
            angle += 360f;
        }

        if (angle > 180f)
        {
            return Mathf.Max(angle, 360f + min);
        }

        return Mathf.Min(angle, max);
    }
    
    
    
}
