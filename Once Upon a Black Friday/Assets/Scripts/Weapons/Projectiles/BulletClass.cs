using UnityEngine;
using System.Collections;

public class BulletClass : MonoBehaviour {

	public int Damage;

	void OnTriggerEnter(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			other.gameObject.GetComponent<PlayerController> ().TakeDamage (Damage);
			Destroy (gameObject);
		}
	}
}
