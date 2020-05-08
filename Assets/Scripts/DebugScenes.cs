using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScenes : MonoBehaviour
{
    public int NumberOfScenes = 3;

    public bool IsDebugging;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDebugging)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int nextIndex = (SceneManager.GetActiveScene().buildIndex + 1) % NumberOfScenes;
            
            SceneManager.LoadScene(nextIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int prevIndex = (SceneManager.GetActiveScene().buildIndex + NumberOfScenes - 1) % NumberOfScenes;
            SceneManager.LoadScene(prevIndex);
        }
    }
}
