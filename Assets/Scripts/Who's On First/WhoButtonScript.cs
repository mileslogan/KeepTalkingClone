using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhoButtonScript : MonoBehaviour
{
    public int buttonNum;
    public string buttonString;
    TextMeshPro Text;
    public bool pressed;

    Vector3 startPos;

    // Start is called before the first frame update
    void Awake()
    {
        Text = GetComponentInChildren<TextMeshPro>();
        buttonNum = int.Parse(this.gameObject.name[0].ToString());
        buttonString = GetComponentInParent<WhosOnFirst>().buttonWords[Random.Range(0, 28)];
        Text.text = buttonString;
        startPos = transform.localPosition;

        GetComponentInParent<WhosOnFirst>().availableStrings[buttonNum - 1] = buttonString;
    }

    public void ResetMe()
    {
        buttonNum = int.Parse(this.gameObject.name[0].ToString());
        buttonString = GetComponentInParent<WhosOnFirst>().buttonWords[Random.Range(0, 28)];
        Text.text = buttonString;
        GetComponentInParent<WhosOnFirst>().availableStrings[buttonNum - 1] = buttonString;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, Time.deltaTime * 1);
    }
}
