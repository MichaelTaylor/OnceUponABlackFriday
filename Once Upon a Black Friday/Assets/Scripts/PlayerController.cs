using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;

	Rigidbody2D RB2D;
	public LayerMask Hittable;

	// Use this for initialization
	void Start () 
	{
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

		//MAKES THE PLAYER LOOK AT THE MOUSE
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

		//RAYCAST FOR WEAPON DISTANCE
		RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos, 1f, Hittable);
		Debug.DrawRay(transform.position, mousePos, Color.red);

		if (hit.collider != null) 
		{
			Debug.Log ("Hit Something");
		}
	}
}
