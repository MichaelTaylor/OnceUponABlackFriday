using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMNG : MonoBehaviour {

    //AUDIO
    public AudioClip[] BGMComponents; //In order to make a seamless loop

    //UI
    public Image Health; //Carrys the health image
    public Image[] WeaponImages; //Just blank weapon images
    public GameObject[] WeaponBoxHighlights; //

	//PRIVATE VARIABLES

	//GAMEPLAY
	PlayerController Player;
    
	//AUDIO
	AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGMComponents[0];
        audioSource.Play();
        Invoke("PlayMainMusic", BGMComponents[0].length);

        //This will make the highlights start as false
        foreach(GameObject _obj in WeaponBoxHighlights)
        {
            _obj.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	   HighlightController(Player.WeaponIndex);
       HealthController();
	}

    //Basically will control the health
    void HealthController()
    {
		Health.fillAmount = Mathf.Lerp(Health.fillAmount,Player.CurrentHealth,(3 * Time.deltaTime));
    }

    //The Black highlighter for the weapon selector
    void HighlightController(int Index)
    {
        
        if (Index == 0)
        {
            WeaponBoxHighlights[0].SetActive(true);
        }
        else
        {
            WeaponBoxHighlights[0].SetActive(false);
        }

        if (Index == 1)
        {
            WeaponBoxHighlights[1].SetActive(true);
        }
        else
        {
            WeaponBoxHighlights[1].SetActive(false);
        }

        if (Index == 2)
        {
            WeaponBoxHighlights[2].SetActive(true);
        }
        else
        {
            WeaponBoxHighlights[2].SetActive(false);
        }
    }

    //Main Music function
    void PlayMainMusic()
    {
        audioSource.clip = BGMComponents[1];
        audioSource.Play();
        audioSource.loop = true;
    }
}
