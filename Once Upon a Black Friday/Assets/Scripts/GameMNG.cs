using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMNG : MonoBehaviour {

    //AUDIO
    public AudioClip[] BGMComponents;

    //UI
    public Image Health;
    public GameObject[] WeaponBoxHighlights;

    GameObject Player;
    AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGMComponents[0];
        audioSource.Play();
        Invoke("PlayMainMusic", BGMComponents[0].length);

        foreach(GameObject _obj in WeaponBoxHighlights)
        {
            _obj.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        HighlightController(Player.GetComponent<PlayerController>().WeaponIndex);
        HealthController();
	}

    void HealthController()
    {
       // Health.fillAmount = Player.GetComponent<PlayerController>().Health;
    }

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

    void PlayMainMusic()
    {
        audioSource.clip = BGMComponents[1];
        audioSource.Play();
        audioSource.loop = true;
    }
}
