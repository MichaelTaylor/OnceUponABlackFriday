using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject Player;

	Vector3 Offset;

	// Use this for initialization
	void Start () 
	{
		Offset = transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		transform.position = Player.transform.position + Offset;
	}
}
