using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public Vector2 target;

	// Use this for initialization
	void Start ()
    {
        target = GetComponent<PatrolWaypoints>().WPoints[GetComponent<PatrolWaypoints>().currentIndex];
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation = Quaternion.LookRotation(target);
    }
}
