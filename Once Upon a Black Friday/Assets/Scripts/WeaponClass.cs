using UnityEngine;
using System.Collections;

public class WeaponClass : MonoBehaviour {

	public string Name;
	public bool IsLaunched;
	public float Force;
	public GameObject Projectile;

	// Use this for initialization
	void Start () {

		if (IsLaunched == true) 
		{
			GetComponent<Rigidbody2D> ().AddForce (transform.up * Force, ForceMode2D.Force);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GetComponent<Rigidbody2D> ().velocity.magnitude == 0f) 
		{
			//Debug.Log ("Pickup");
		}

		//Debug.Log (GetComponent<Rigidbody2D> ().velocity.magnitude);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Wall") 
		{
			//GetComponent<BoxCollider2D> ().isTrigger = true;
			Debug.Log ("Hit");
		}
	}

}
