using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlinerScript : MonoBehaviour
{
    //NOTES FOR MIKE
    // the way this script works is that it shoots a ray, then via a raycasthit finds the mesh in the meshfilter, and assigns it to the outliner prefab's meshfilter so it always has the right shape
    // I set it so that there is a public array of tag variable so that not everything with a collider gets outlines when hovered over, on the otherhand, if you want everything with a collider to be selected, 
    // then set the array to zero.

    public MeshFilter thisMeshFilter;

    public MeshFilter selectedMeshFilter;//object the cursor is over

    RaycastHit hit;

    public string[] validTags; //set in the ditor, if you dont want to filter any tags out, then just leave this set to zero

    void Awake()
    {
        thisMeshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            transform.parent = hit.collider.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one * 1.01f;

            if (validTags.Length > 0)
            {
                for (int i = 0; i < validTags.Length; i++)
                {
                    if (hit.collider.gameObject.tag == validTags[i])
                    {
                        Debug.Log(hit.collider.gameObject);
                        selectedMeshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();
                        thisMeshFilter.mesh = selectedMeshFilter.mesh;
                    }
                    if (i==validTags.Length-1 && hit.collider.gameObject.tag != validTags[i])
                    {
                        thisMeshFilter.mesh = null;
                    }
                }
            }
            else
            {
                selectedMeshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();
                thisMeshFilter.mesh = selectedMeshFilter.mesh;
            }
            Debug.Log(hit.collider.gameObject);
        }
        else
        {
            thisMeshFilter.mesh = null;
        }
    }
}
