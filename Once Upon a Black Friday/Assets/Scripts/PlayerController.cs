using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed, WeaponThrowStrength, RageMeter;
	public LayerMask Hittable;
	public GameObject [] WeaponPool;
	public GameObject [] WeaponsOnHand;
	public GameObject SpawnPoint;

	Rigidbody2D RB2D;
	Vector2 mousePos;

	// Use this for initialization
	void Start () 
	{
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
		//ThrowAwayWeapon ();

		//MAKES THE PLAYER LOOK AT THE MOUSE
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

		//RAYCAST FOR WEAPON DISTANCE
		RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos, 0.3f, Hittable);
		Debug.DrawRay(transform.position, mousePos, Color.red);

		if (hit.collider != null && Input.GetMouseButtonDown(0) == true) 
		{
			Debug.Log ("Hit Something");
		}
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
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) 
		{
			Debug.Log ("Switch");
		}
	}

	void ThrowAwayWeapon()
	{
		//TODO: add force to weapons
		if (Input.GetMouseButtonUp(0) == true && WeaponsOnHand[0] != null) 
		{
			if (System.Array.IndexOf(WeaponPool, WeaponsOnHand[0]) != -1)
				{
					Debug.Log("Found");
				}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//TODO: Add weapon pickup and functionality
		if (other.CompareTag("Weapon"))
		{
			if (WeaponsOnHand [0] == null) 
			{
				WeaponsOnHand [0] = other.gameObject;
				other.gameObject.SetActive (false);
			} 
			else if (WeaponsOnHand [0] != null && WeaponsOnHand [1] == null) 
			{
				WeaponsOnHand [1] = other.gameObject;
				other.gameObject.SetActive (false);
			}
			else if (WeaponsOnHand [0] != null && WeaponsOnHand [1] != null && WeaponsOnHand [2] == null) 
			{
				WeaponsOnHand [2] = other.gameObject;
				other.gameObject.SetActive (false);
			}
		}
	}
}
