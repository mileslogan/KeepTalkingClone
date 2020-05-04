using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    AudioSource AS;
    public AudioClip Song;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
        AS.PlayOneShot(Song);
    }

    // Update is called once per frame
    void Update()
    {
        if (!AS.isPlaying)
        {
            AS.PlayOneShot(Song);
        }
    }
}
