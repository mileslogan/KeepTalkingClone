using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireBehavior : MonoBehaviour
{

    Color[] possibleColors;
    public Color wireColor;
    WireModScript ModuleHead;

    // Start is called before the first frame update
    void Awake()
    {
        ModuleHead = GetComponentInParent<WireModScript>();

        possibleColors = new Color[5]; 
        possibleColors[0] = Color.red;
        possibleColors[1] = Color.blue;
        possibleColors[2] = Color.white;
        possibleColors[3] = Color.yellow;
        possibleColors[4] = Color.black;

        wireColor = possibleColors[Random.Range(0,4)]; //determine color of wire
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", wireColor);
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", wireColor);
    }
 
}
