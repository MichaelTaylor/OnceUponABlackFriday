using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public int Health;
    public bool WillUseWayPoints;
    public float Speed, DistanceThreshold;

    GameObject Player;
    SpriteRenderer spriteRenderer;
    PolyNavAgent PolyNavagent;
    PatrolWaypoints PatrolWayPoints;


	// Use this for initialization
	void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        PolyNavagent = GetComponent<PolyNavAgent>();

        if (WillUseWayPoints == true)
        {
            PatrolWayPoints = GetComponent<PatrolWaypoints>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        float XDistance = transform.position.x - Player.transform.position.x;
        float YDistance = transform.position.y - Player.transform.position.y;

        if ((Mathf.Abs(XDistance) <= DistanceThreshold) || (Mathf.Abs(XDistance) <= DistanceThreshold))
        {
            PolyNavagent.SetDestination(Player.transform.position);
        }
        else
        {
            PolyNavagent.SetDestination(PatrolWayPoints.WPoints[PatrolWayPoints.currentIndex]);
        }
    }

    void TakeDamage(int DamageTaken)
    {

    }
}
