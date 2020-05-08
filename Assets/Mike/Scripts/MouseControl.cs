using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class MouseControl : MonoBehaviour
{
    
    public enum BombStates {OnTable, PickedUp, OnModule}

    public BombStates CurrentState = BombStates.OnTable;
    public BombStates OutlineState = BombStates.OnTable;
    private GenerateBomb BombScript;
    private Lerp LerpScript;
    private Transform ParentBomb;
    private Collider BombCollider;
    private BombLerp BombLerpScript;

    private float HoldingDownTimer; //holding down right mouse click


    private OutlinerScript OutlineScript;
    
    
    // Rotation //
    [FormerlySerializedAs("MouseSensitivity")] public float RotSpeed = 10f;

    public float MinXRot = -77f;

    public float MaxXRot = 109f;
    // Start is called before the first frame update
    void Start()
    {
        OutlineScript = FindObjectOfType<OutlinerScript>();
        BombCollider = GetComponent<Collider>();
        BombScript = GetComponent<GenerateBomb>();
        ParentBomb = BombScript.gameObject.transform.parent;
        LerpScript = FindObjectOfType<Lerp>();
        BombLerpScript = FindObjectOfType<BombLerp>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeOutlineState();

        //prevent bomb interaction
        if (BombScript.IsGameOver || BombScript.IsGameWon)
        {
            return;
        }
        
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
                    
                    if (ParentBomb.eulerAngles.y >= 90f && ParentBomb.eulerAngles.y <= 270)
                    {
                        newRot = ParentBomb.localRotation.eulerAngles + new Vector3(-mouseY, -mouseX, 0f);
                        BombLerpScript.IsFrontSide = false;
                    }
                    else
                    {
                        BombLerpScript.IsFrontSide = true;
                    }
                    
                    newRot.x = ClampAngle(newRot.x, MinXRot, MaxXRot);
                    
                    //newRot.x = Mathf.Clamp(newRot.x, MinXRot, MaxXRot);
                    ParentBomb.eulerAngles = newRot;

                }
                else
                {
                    if (HoldingDownTimer <= .20f && HoldingDownTimer > 0f) //needs to be less than this to be considered a quick click
                    {
                        BombLerpScript.PutDown();
                        LerpScript.LerpCamera(LerpScript.DefaultSpot, .5f);
                        //Quaternion.Lerp()
                        CurrentState = BombStates.OnTable; //switch state
                        //turn on collider
                        BombCollider.enabled = true;
                    }
                    HoldingDownTimer = 0f;
                }
                break;
            case BombStates.OnModule:
                
                if (Input.GetMouseButton(1))
                {
                    HoldingDownTimer += Time.deltaTime; //time holding down
                    
                    //can rotate bomb now->get input
                    float mouseX = Input.GetAxis("Mouse X") * RotSpeed * Time.deltaTime;
                    float mouseY = Input.GetAxis("Mouse Y") * RotSpeed * Time.deltaTime;

                    
                    //Clamp upward/downward rotation
                    Vector3 newRot = ParentBomb.localRotation.eulerAngles + new Vector3(mouseY, -mouseX, 0f);
                   
                    if (ParentBomb.eulerAngles.y >= 90f && ParentBomb.eulerAngles.y <= 270)
                    {
                        newRot = ParentBomb.localRotation.eulerAngles + new Vector3(-mouseY, -mouseX, 0f);
                        BombLerpScript.IsFrontSide = false;
                    }
                    else
                    {
                        BombLerpScript.IsFrontSide = true;
                    }
                    
                    newRot.x = ClampAngle(newRot.x, MinXRot, MaxXRot);
                    
                    //newRot.x = Mathf.Clamp(newRot.x, MinXRot, MaxXRot);
                    ParentBomb.eulerAngles = newRot;


                    
                    
                    //void Shake()
                }
                else
                {
                    if (HoldingDownTimer <= .20f && HoldingDownTimer > 0f) //needs to be less than this to be considered a quick click
                    {
                        //lerp to picking up bomb state
                        BombLerpScript.PickUp(); //MAY NEED TO ADD ANOTHER ONE IF ON BACKSIDE
                        LerpScript.LerpCamera(LerpScript.HoldBombSpot, .5f);
                        
                        
                        
                        //Quaternion.Lerp()
                        CurrentState = BombStates.PickedUp; //switch state
                        BombScript.TurnOffAllCols();
                        GenerateBomb.SelectedModule = null;
                        BombScript.SelectMod = GenerateBomb.SelectedModule;
                    }
                    HoldingDownTimer = 0f;
                }
                
                break;
        }
    }

    private void OnMouseEnter()
    {
        //AudioManager.Instance.PlayOneShotSound("Hover", false);
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
                    BombScript.TurnOnAllCols();
                    //lerp to picking up bomb state
                    
                    BombLerpScript.PickUp();
                    LerpScript.LerpCamera(LerpScript.HoldBombSpot, .4f);
                    //Quaternion.Lerp()
                    CurrentState = BombStates.PickedUp; //switch state
                    //turn off collider
                    BombCollider.enabled = false;
                }
                else
                {
                    
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

    public void ShakeBomb()
    {
        StartCoroutine(ShakeBomb(.4f));
    }

    public AnimationCurve TweenCurve;
    public IEnumerator ShakeBomb(float duration)
    {
        
        float timer = 0f;
        
        //positions of bomb
        Vector3 startRot = ParentBomb.rotation.eulerAngles;
        Vector3 endRot = startRot;
        //endRot.y += 10f;

        endRot.y += 2f;
        
        while (timer < duration)
        {

            timer += Time.deltaTime;
            
            ParentBomb.rotation =  Quaternion.Euler(Vector3.LerpUnclamped(startRot, endRot, TweenCurve.Evaluate(timer/duration)));
            yield return null;
        }
    }

    void ChangeOutlineState()
    {
        if (OutlineState != CurrentState)
        {
            OutlineState = CurrentState;

            if (OutlineState == BombStates.OnTable)
            {
                OutlineScript.validTags = new string[1];
                OutlineScript.validTags[0] = "BombItself";
                
            }
            else if (OutlineState == BombStates.PickedUp)
            {
                OutlineScript.validTags = new string[1];
                OutlineScript.validTags[0] = "ModuleOutline";
                
            }
            else if (OutlineState == BombStates.PickedUp)
            {
                OutlineScript.validTags = new string[1];
                OutlineScript.validTags[0] = "ModuleParts";
            }
        }
    }
    
    
}
