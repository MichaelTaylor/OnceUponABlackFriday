using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMNG : MonoBehaviour {

    public AudioClip[] BGMComponents;

    AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGMComponents[0];
        audioSource.Play();
        Invoke("PlayMainMusic", BGMComponents[0].length);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void PlayMainMusic()
    {
        audioSource.clip = BGMComponents[1];
        audioSource.Play();
        audioSource.loop = true;
    }
}
