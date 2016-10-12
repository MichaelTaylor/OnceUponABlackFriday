using UnityEngine;
using System.Collections;

public class WeaponClass : MonoBehaviour {

	public string Name; //Name of the weapon for the system to check
    public int AnimationLayerIndex, WeaponPower, ThrownImpact; //For the animator to know which layer to switch to
	public bool IsLaunched; //Is it going to be thrown
	public float Force; //How much force it's going to be thrown
	public GameObject Projectile; //Only for guns
    public AnimationClip AttackAnimation; //This is for the animator

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

        if (col.gameObject.tag == "Enemy" && IsLaunched == true)
        {
			col.gameObject.GetComponent<EnemyController> ().TakeDamage (ThrownImpact);
        }
	}
}
