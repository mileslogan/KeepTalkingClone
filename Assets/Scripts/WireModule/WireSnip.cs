using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSnip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().enabled = !transform.parent.GetComponentInParent<MeshRenderer>().enabled; //ensures the snipped wire and intact wire are never visible at the same time.
    }
}
