using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent), typeof(PatrolWaypoints))]
public class EnemyController : MonoBehaviour {

    //Public
	public float Health;
    public bool WillUseWayPoints;
    public float Speed, DistanceThreshold, AttackPower, AttackRange, KnockBackForce;
	public LayerMask Hittable;

    //Private Varibles
    protected bool IsInKnockBack;
    protected Vector2 PreviousPosition;
    protected float Velocity, KnockBackRecoveryTimer;

    //Get Component Varibles
    protected GameObject Player;
    protected Animator BodyAnimator, LegAnimator;
    protected Rigidbody2D RB2D;
    protected SpriteRenderer spriteRenderer;
    protected PolyNavAgent PolyNavagent;
    protected PatrolWaypoints PatrolWayPoints;

    //Varible to measure when the enemy gets in range
    protected float XDistance;
    protected float YDistance;

    // Use this for initialization
    void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
		BodyAnimator = GetComponent<Animator> ();
        LegAnimator = transform.Find("Legs").gameObject.GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PolyNavagent = GetComponent<PolyNavAgent>();
		PatrolWayPoints = GetComponent<PatrolWaypoints>();

        //Some enemys will have a set path and some won't
		if (WillUseWayPoints == true) 
		{
			PatrolWayPoints.enabled = true;
		} 
		else 
		{
			PatrolWayPoints.enabled = false;
		}
    }
	
	// Update is called once per frame
	public virtual void Update ()
    {
		//Records a radius to detect the player
        XDistance = transform.position.x - Player.transform.position.x;
        YDistance = transform.position.y - Player.transform.position.y;

        if (IsInKnockBack == false)
        {
            //RAYCAST FOR WEAPON DISTANCE
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, AttackRange, Hittable);
            Debug.DrawRay(transform.position, transform.up * AttackRange, Color.green);

            if (hit.collider != false && XDistance <= 0.1f && YDistance <= 0.1f)
            {
                hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(AttackPower);
            }
        }
        
        if (IsInKnockBack == true && KnockBackRecoveryTimer <= 0.5f)
        {
            KnockBackRecoveryTimer += 1 * Time.deltaTime;
            RB2D.AddForce(transform.up * -KnockBackForce);
        }
        else if (IsInKnockBack == true && KnockBackRecoveryTimer > 0.5f)
        {
            IsInKnockBack = false;
            KnockBackRecoveryTimer = 0f;
        }

        AnimationChecker();
    }

    //Purley for taking damage
	public void TakeDamage(int DamageTaken)
    {
        Health -= DamageTaken;
        IsInKnockBack = true;
		if (Health <= 0) 
		{
			Destroy (gameObject);
		}
    }
	//To check if it's moving or not
    void AnimationChecker()
    {  
         LegAnimator.SetBool("IsMoving", true);
    }
}
