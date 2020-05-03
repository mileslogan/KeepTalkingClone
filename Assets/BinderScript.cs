using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BinderScript : MonoBehaviour
{
    public GameObject binder;

    public Animator anim;

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
            RaycastHit binderHit;
            if (Physics.Raycast(ray, out binderHit)){
                if (binderHit.transform.gameObject == binder)
                {

                    anim.SetTrigger("Open");
                }

                }

            }
    }
    }
