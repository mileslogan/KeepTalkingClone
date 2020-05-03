using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickModule : MonoBehaviour
{
    private Lerp CameraLerp;
    public GameObject Self;
    public GenerateBomb.ModuleTypes ModuleType = GenerateBomb.ModuleTypes.Null;
    public bool IsFlipped;

    private MeshFilter MeshF;
    private MeshRenderer MeshRend;
    private MeshFilter SelectedMeshFilter;
    private void Start()
    {
        MeshF = GetComponent<MeshFilter>();
        MeshRend = GetComponent<MeshRenderer>();
        MeshRend.enabled = false;

        SelectedMeshFilter = FindObjectOfType<OutlinerScript>().thisMeshFilter;
        //TurnOnCols();
        //TurnOffCols();
        Self = gameObject;
        CameraLerp = FindObjectOfType<Lerp>();
        if (IsFlipped)
        {
            transform.parent.Rotate(0f, 180f, 0f);
        }
        
        
    }

    public void Update()
    {
        if (MeshF.mesh == SelectedMeshFilter.mesh && !MeshRend.enabled)
        {
            MeshRend.enabled = true;
        }
        else
        {
            MeshRend.enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        //AudioManager.Instance.PlayOneShotSound("Hover", false);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) //Left click
        {

            CameraLerp.LerpGivenObject(transform.parent.gameObject);
            
            
        }
    }

    public void TurnOffCols()
    {
        foreach (Collider col in transform.parent.gameObject.GetComponentsInChildren<Collider>())
        {
            if (!col.CompareTag("ModuleOutline"))
            {
                col.enabled = false;
            }
            
        }
    }
    
    public void TurnOnCols()
    {
        foreach (Collider col in transform.parent.gameObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
    }
    
    
}