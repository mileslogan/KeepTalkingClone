using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance = null;

    public Animator fadeAnim;

    public int numModules;
    public int defuseTime;

    public int timeLeft;
    public string causeOfDeath;
    public bool defused;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToStartFunction()
    {
        StartCoroutine("ToStartScene");
    }

    public void ToBombFunction()
    {
        StartCoroutine("ToBombScene");
    }

    public void ToEndFunction()
    {
        StartCoroutine("ToEndScene");
    }

    IEnumerator ToStartScene()
    {
        fadeAnim.SetInteger("FadeState", 2);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

    IEnumerator ToBombScene()
    {
        fadeAnim.SetInteger("FadeState", 2);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

    IEnumerator ToEndScene()
    {
        fadeAnim.SetInteger("FadeState", 2);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }
}
