using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickModule : MonoBehaviour
{
    private Lerp CameraLerp;
    public GameObject Self;
    private void Start()
    {
        Self = gameObject;
        CameraLerp = FindObjectOfType<Lerp>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) //Left click
        {

            CameraLerp.LerpGivenObject(gameObject);

        }
    }
}