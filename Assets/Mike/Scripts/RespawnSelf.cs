using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class RespawnSelf : MonoBehaviour
{
    public enum PotentialObj {Battery, SerialN, Indicator}
    
    //indicate which type of object to spawn if this one is colliding with another object
    public PotentialObj ObjectToSpawn;

    //make sure does not occur twice
    public bool HasTriggered;

    public Transform Parent;

    public void Start()
    {
        Parent = transform.parent;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("EXTRA") && !HasTriggered)
        {
            GenerateBomb bombScript = FindObjectOfType<GenerateBomb>();
            GameObject objToSpawn = null;
            
            //figure out which object to spawn
            switch (ObjectToSpawn)
            {
                case PotentialObj.Battery:
                    objToSpawn = bombScript.BatteryPrefab;
                    break;
                case PotentialObj.Indicator:
                    objToSpawn = bombScript.IndicatorPrefab;
                    break;
                case PotentialObj.SerialN:
                    objToSpawn = bombScript.SerialPrefab;
                    break;
            }
            
            
            //spawn object
            if (objToSpawn != null)
            {
                //special indicator spawn
                if (objToSpawn == bombScript.IndicatorPrefab)
                {
                    IndicatorBehavior indScript = GetComponent<IndicatorBehavior>();
                    
                    //spawn exact same indicator prefab with correct indicator
                    if (indScript != null)
                    {
                        bombScript.SpawnIndicator(indScript.IndicatorIndex);
                    }
                    else
                    {
                        Debug.Log("Error, no indicator behavior script");
                    }
                }
                else
                {
                    bombScript.SpawnExtra(objToSpawn);
                    bombScript.DoneTimer = .3f;
                    //obj.transform.parent = Parent;
                }
            }
            //destroy current because it overlaps
            Destroy(gameObject);
            //make sure it only happens once
            HasTriggered = true;
        }
        
    }
}
