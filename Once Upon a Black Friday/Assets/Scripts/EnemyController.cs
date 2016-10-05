using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent), typeof(PatrolWaypoints))]
public class EnemyController : MonoBehaviour {

    //Public
    public int Health;
    public bool WillUseWayPoints;
    public float Speed, DistanceThreshold;

    //Private Varibles
    Vector2 PreviousPosition;
    float Velocity;
    
    //Get Component Varibles
    GameObject Player;
    Animator BodyAnimator, LegAnimator;
    Rigidbody2D RB2D;
    SpriteRenderer spriteRenderer;
    PolyNavAgent PolyNavagent;
    PatrolWaypoints PatrolWayPoints;

	// Use this for initialization
	void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
		BodyAnimator = GetComponent<Animator> ();
        LegAnimator = transform.Find("Legs").gameObject.GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PolyNavagent = GetComponent<PolyNavAgent>();

        //Some enemys will have a set path and some won't
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

        //If the player is in range, then the enemy will go towards them
        if ((Mathf.Abs(XDistance) <= DistanceThreshold) || (Mathf.Abs(YDistance) <= DistanceThreshold))
        {
            PolyNavagent.SetDestination(Player.transform.position);
            PatrolWayPoints.enabled = false;
			BodyAnimator.SetBool ("IsAttacking", true);

        }
        else //If not will continue to do their thing
        {
            PolyNavagent.SetDestination(PatrolWayPoints.WPoints[PatrolWayPoints.currentIndex]);
            PatrolWayPoints.enabled = true;
			BodyAnimator.SetBool ("IsAttacking", false);
        }

        AnimationChecker();
    }

    //Purley for taking damage
    public void TakeDamage(int DamageTaken)
    {
        Health -= DamageTaken;

		if (Health >= 0) 
		{
			Destroy (gameObject);
			Debug.Log ("Dead");
		}
			
    }

    void AnimationChecker()
    {
        
            LegAnimator.SetBool("IsMoving", true);

    }
}
