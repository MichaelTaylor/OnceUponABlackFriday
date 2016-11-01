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
    protected float Velocity, KnockBackRecoveryTimer, InvincibilityTimer;
    public Color32 RegularColor;
    protected Color32 DamagedColor = new Color32(0, 0, 0, 255);

    //Get Component Varibles
    protected GameObject Player;
    protected BoxCollider2D EnemyCollider;
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
        EnemyCollider = GetComponent<BoxCollider2D>();
		BodyAnimator = GetComponent<Animator> ();
        LegAnimator = transform.Find("Legs").gameObject.GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PolyNavagent = GetComponent<PolyNavAgent>();
		PatrolWayPoints = GetComponent<PatrolWaypoints>();

        RegularColor = spriteRenderer.color;

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
            /*RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, AttackRange, Hittable);
            Debug.DrawRay(transform.position, transform.up * AttackRange, Color.green);

            if (hit.collider != false && XDistance <= 0.1f && YDistance <= 0.1f)
            {
                hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(AttackPower);
            }*/
        }
        
        if (IsInKnockBack == true && KnockBackRecoveryTimer <= 0.3f)
        {
            KnockBackRecoveryTimer += 1 * Time.deltaTime;
            RB2D.AddForce(transform.up * -KnockBackForce);

            InvincibilityTimer += 0.2f;

            if (InvincibilityTimer % 1 == 0)
            {
                spriteRenderer.color = DamagedColor;
                InvincibilityTimer = 0;
            }
            else
            {
                spriteRenderer.color = RegularColor;
            }
        }
        else if (IsInKnockBack == true && KnockBackRecoveryTimer > 0.3f)
        {
            IsInKnockBack = false;
            KnockBackRecoveryTimer = 0f;
            EnemyCollider.enabled = true;
            spriteRenderer.color = RegularColor;
        }

        AnimationChecker();
    }

    //Purley for taking damage
	public void TakeDamage(int DamageTaken)
    {
        Health -= DamageTaken;
        IsInKnockBack = true;
        EnemyCollider.enabled = false;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(AttackPower);
            Debug.Log("Go");
        }
    }
}
