using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhosOnFirst : MonoBehaviour
{

    public string[] keyWords;
    public string[] buttonWords;

    public string screenText;
    public TextMeshPro Text;

    public string[] availableStrings = new string[6];
    public string correctLable;
    
    // Start is called before the first frame update
    void Start()
    {
        screenText = keyWords[Random.Range(0, 28)];
        Text.text = screenText;
    }

    void FailedButton()
    {


    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; //outs to hit so i can affect the wires
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //screen point to ray (mouse position)

        if (Physics.Raycast(ray, out hit))// if moused over
        {
            if (hit.collider.tag == "WhoButtons" && Input.GetKeyDown(KeyCode.Mouse0))
            {
                hit.collider.transform.localPosition += transform.forward*.07f;
                hit.collider.gameObject.GetComponent<ButtonScript>().pressed = true;
            }
        }
    }
}
