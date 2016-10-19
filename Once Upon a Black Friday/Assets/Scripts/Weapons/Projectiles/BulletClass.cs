using UnityEngine;
using System.Collections;

public class BulletClass : MonoBehaviour {

	public float Damage;

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			other.gameObject.GetComponent<PlayerController> ().TakeDamage (Damage);
			Destroy (gameObject);
		}
	}
}
