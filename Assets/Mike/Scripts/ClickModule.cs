using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickModule : MonoBehaviour
{
    private Lerp CameraLerp;
    public GameObject Self;

    public bool IsFlipped;
    private void Start()
    {
        Self = gameObject;
        CameraLerp = FindObjectOfType<Lerp>();
        if (IsFlipped)
        {
            transform.Rotate(0f, 180f, 0f);
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

            CameraLerp.LerpGivenObject(gameObject);

        }
    }
}