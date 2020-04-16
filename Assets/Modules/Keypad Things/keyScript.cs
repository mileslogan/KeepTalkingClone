using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScript : MonoBehaviour
{
    public ArrayList symbols = new ArrayList();

    public GameObject key1;
    public GameObject key2;
    public GameObject key3;
    public GameObject key4;

    public Animator anim1;
    public Animator anim2;
    public Animator anim3;
    public Animator anim4;

    public Sprite symbol1;
    public Sprite symbol2;
    public Sprite symbol3;
    public Sprite symbol4;
    public Sprite symbol5;
    public Sprite symbol6;
    public Sprite symbol7;
    public Sprite symbol8;
    public Sprite symbol9;
    public Sprite symbol10;
    public Sprite symbol11;
    public Sprite symbol12;
    public Sprite symbol13;
    public Sprite symbol14;
    public Sprite symbol15;
    public Sprite symbol16;
    public Sprite symbol17;
    public Sprite symbol18;
    public Sprite symbol19;
    public Sprite symbol20;
    public Sprite symbol21;
    public Sprite symbol22;
    public Sprite symbol23;
    public Sprite symbol24;
    public Sprite symbol25;
    public Sprite symbol26;
    public Sprite symbol27;
    public Sprite symbol28;
    public Sprite symbol29;
    public Sprite symbol30;
    public Sprite symbol31;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit keyHit;
            if (Physics.Raycast(ray, out keyHit))
            {
                if (keyHit.transform == key1)
                {
                    Debug.Log("click");
                    anim1.SetTrigger("Push");

                }

            }

            }
        }
    }

