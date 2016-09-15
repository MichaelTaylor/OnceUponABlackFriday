using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float speed, WheelSensitivity, WeaponThrowStrength, RageMeter, AttackRecoveryTimer;
	public LayerMask Hittable;
    public int Health, WeaponIndex;
	public GameObject[] WeaponPool;
	public GameObject [] WeaponsOnHand;
	public GameObject SpawnPoint;

	GameMNG GameManager;
	Rigidbody2D RB2D;
	Vector2 mousePos;
	public bool EmptyHanded, AttackRecover;

	// Use this for initialization
	void Start () 
	{
		GameManager = GameObject.Find ("GameManager").GetComponent<GameMNG> ();
		WeaponsOnHand = new GameObject[3];
		RB2D = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//PLAYER MOVEMENT
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

        Vector2 movement = new Vector2 (horizontal, vertical);
		RB2D.velocity = movement * speed;
		Sprinting ();
		ThrowAwayWeapon ();

		//MAKES THE PLAYER LOOK AT THE MOUSE
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

		//RAYCAST FOR WEAPON DISTANCE
		RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos, 0.3f, Hittable);
		Debug.DrawRay(transform.position, mousePos, Color.red);


		//MOVEMENT FUNCTIONS

		//WEAPON FUNCTIONS
		WeaponChecker();
      
        //INTERNAL GAME FUCTIONS
        if (hit.collider != null && Input.GetMouseButtonDown(0) == true) 
		{
			Debug.Log ("Hit Something");
		}
	}

    void Update()
    {
        SwitchWeapons();
    }

	void Sprinting()
	{
		if (Input.GetButton ("Sprint")) 
		{
			speed = 2;
		} 
		else 
		{
			speed = 1;
		}
	}

	void SwitchWeapons()
	{
        WeaponIndex += Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel"));

        //This is to make sure the array do not get out of index
        if (WeaponIndex < 0)
        {
            WeaponIndex = 2;
        }
        else if (WeaponIndex > 2)
        {
            WeaponIndex = 0;
        }
    }

	void ThrowAwayWeapon()
	{
		if (Input.GetMouseButtonDown(1) == true && WeaponsOnHand[WeaponIndex] != null) 
		{
			for (int i = 0; i < WeaponPool.Length; i++) 
			{
				if (WeaponsOnHand [WeaponIndex].GetComponent<WeaponClass>().Name == WeaponPool [i].GetComponent<WeaponClass>().Name) 
				{
					GameObject ThrownWeapon = Instantiate (WeaponsOnHand [WeaponIndex], SpawnPoint.transform.position, SpawnPoint.transform.rotation) as GameObject;
					ThrownWeapon.GetComponent<WeaponClass> ().IsLaunched = true;
					ThrownWeapon.GetComponent<BoxCollider2D>().isTrigger = false;
					ThrownWeapon.SetActive (true);
					AttackRecover = true;
					AttackRecoveryTimer = 0f;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//TODO: Add weapon pickup and functionality
		if (other.CompareTag("Weapon") || other.CompareTag("Gun"))
		{
			if (WeaponsOnHand[WeaponIndex] == null)
			{
				WeaponsOnHand [WeaponIndex] = other.gameObject;
				other.gameObject.SetActive (false);
			}

			/*else if (WeaponsOnHand [0] != null && WeaponsOnHand [1] == null) 
			{
				WeaponsOnHand [1] = other.gameObject;
				other.gameObject.SetActive (false);
			}
			else if (WeaponsOnHand [0] != null && WeaponsOnHand [1] != null && WeaponsOnHand [2] == null) 
			{
				WeaponsOnHand [2] = other.gameObject;
				other.gameObject.SetActive (false);
			}*/
		}
	}

    void WeaponChecker()
    {


        if (WeaponsOnHand[WeaponIndex] == null)
        {
            EmptyHanded = false;
        }

        //This will get rid of it without the console asking for a null reference
        if (AttackRecover = true && AttackRecoveryTimer <= 0.25f)
        {
            AttackRecoveryTimer += 1f * Time.deltaTime;
            WeaponsOnHand[WeaponIndex] = null;
            AttackRecover = false;
        }

		if (WeaponsOnHand[WeaponIndex] != null && WeaponsOnHand[WeaponIndex].gameObject.tag == "Gun")
        {
			if (Input.GetMouseButtonDown (0) == true) 
			{
				GameObject Projectile = Instantiate (WeaponsOnHand[WeaponIndex].GetComponent<WeaponClass>().Projectile, SpawnPoint.transform.position, SpawnPoint.transform.rotation) as GameObject;
				Projectile.GetComponent<Rigidbody2D> ().AddForce (transform.up * 100, ForceMode2D.Force);
				Debug.Log ("Shoot");
			}
        }
	}
}
