using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SerialNumber : MonoBehaviour
{
    //beginning text of string on object
    public string DisplayString = "Serial#\n";
    public TextMeshPro TopSideText;

    public TextMeshPro FlipSideText;
    // Start is called before the first frame update
    void Start()
    {
        TopSideText.text = FindObjectOfType<GenerateBomb>().SerialN;
        //FlipSideText.text = DisplayString + FindObjectOfType<GenerateBomb>().SerialN;
    }
    
}
