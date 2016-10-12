using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float speed, WheelSensitivity, RageMeter, AttackRecoveryTimer;
	public LayerMask Hittable;
    public int Health, WeaponIndex, AttackPower;
	public GameObject[] WeaponPool;
	public GameObject [] WeaponsOnHand;
	public GameObject SpawnPoint;

	GameMNG GameManager;
    SpriteRenderer spriteRenderer;
	Rigidbody2D RB2D;
	Vector2 mousePos;
    Animator PlayerAnimator, LegAnimator;
    public bool CanAttack;
	public bool EmptyHanded, AttackRecover;

	// Use this for initialization
	void Start () 
	{
        AttackPower = 1;

        GameManager = GameObject.Find ("GameManager").GetComponent<GameMNG> ();
        spriteRenderer = GetComponent<SpriteRenderer>();
		WeaponsOnHand = new GameObject[3]; //instantly makes 3 slots
		RB2D = GetComponent<Rigidbody2D> ();
        PlayerAnimator = GetComponent<Animator>();
        LegAnimator = transform.Find("Legs").gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//PLAYER MOVEMENT
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

        Vector2 movement = new Vector2 (horizontal, vertical);
		RB2D.velocity = movement * speed;
		Sprinting ();
		
		//MAKES THE PLAYER LOOK AT THE MOUSE
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 RayDestination = mousePos - transform.position; //In order to keep the ray cast destination accurate
		transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        //RAYCAST FOR WEAPON DISTANCE
        RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDestination, 0.6f, Hittable);
        Debug.DrawRay(transform.position, new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y), Color.red);
        
        //INTERNAL GAME FUCTIONS
        if (hit != false && hit.collider != false && hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy") && Input.GetMouseButton(0) == true)
        {
            //CanAttack = true;
            Debug.Log("HitSomething");
            hit.collider.gameObject.GetComponent<EnemyController>().TakeDamage(AttackPower);
        }
        else
        {
           //CanAttack = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) == true)
        {
            PlayerAnimator.SetBool("IsAttacking", true);
        }
        else
        {
            PlayerAnimator.SetBool("IsAttacking", false);
        }

        AnimationChecker();
        
        //WEAPON FUNCTIONS
        WeaponChecker();
        AttackPowerChecker();
        SwitchWeapons();
        ThrowAwayWeapon();

		if (WeaponsOnHand [WeaponIndex] != null) 
		{
			PlayerAnimator.SetLayerWeight (WeaponsOnHand [WeaponIndex].GetComponent<WeaponClass> ().AnimationLayerIndex, 1f);
		} 
		else 
		{
			for (int i = 1; i < WeaponPool.Length; i++) 
			{
				PlayerAnimator.SetLayerWeight (i, 0f);
			}
		}
	}

	void Sprinting()
	{
		if (Input.GetButton ("Sprint")) 
		{
			speed = 2;
		} 
		else 
		{
			speed = 1;
		}
	}

	void SwitchWeapons()
	{
        WeaponIndex += Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel"));

        //This is to make sure the array do not get out of index
        if (WeaponIndex < 0)
        {
            WeaponIndex = 2;
        }
        else if (WeaponIndex > 2)
        {
            WeaponIndex = 0;
        }
    }

	void ThrowAwayWeapon()
	{
		if (Input.GetMouseButtonDown(1) == true && WeaponsOnHand[WeaponIndex] != null) 
		{
			for (int i = 0; i < WeaponPool.Length; i++) 
			{
				if (WeaponsOnHand [WeaponIndex].GetComponent<WeaponClass>().Name == WeaponPool [i].GetComponent<WeaponClass>().Name) 
				{
					GameObject ThrownWeapon = Instantiate (WeaponsOnHand [WeaponIndex], SpawnPoint.transform.position, SpawnPoint.transform.rotation) as GameObject;
					ThrownWeapon.GetComponent<WeaponClass> ().IsLaunched = true;
                    PlayerAnimator.SetLayerWeight(ThrownWeapon.GetComponent<WeaponClass>().AnimationLayerIndex, 0f);
                    ThrownWeapon.GetComponent<BoxCollider2D>().isTrigger = false;
					ThrownWeapon.SetActive (true);
					AttackRecover = true;
					AttackRecoveryTimer = 0f;
                    GameManager.WeaponImages[WeaponIndex].sprite = null;
                }
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		//Adds weapon to arsenal
		if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Gun")
		{   
			if (WeaponsOnHand[WeaponIndex] == null) //This will check if the slot is empty or not
			{
                //PlayerAnimator.SetLayerWeight(other.gameObject.GetComponent<WeaponClass>().AnimationLayerIndex, 1f);   
                WeaponsOnHand [WeaponIndex] = other.gameObject; //Picks up the weapon
                GameManager.WeaponImages[WeaponIndex].sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite; //shows the weapon in the UI
                other.gameObject.SetActive (false); // will make the pick up dissapear
            }
		}
	}

    void WeaponChecker()
    {   //To check if the player is empty handed
        if (WeaponsOnHand[WeaponIndex] == null)
        {
            EmptyHanded = false;
        }

        //This will get rid of it without the console asking for a null reference
        if (AttackRecover = true && AttackRecoveryTimer <= 0.25f)
        {
            AttackRecoveryTimer += 1f * Time.deltaTime;
            WeaponsOnHand[WeaponIndex] = null;
            AttackRecover = false;
        }
        //If this is true then it will shoot projectiles
		if (WeaponsOnHand[WeaponIndex] != null && WeaponsOnHand[WeaponIndex].gameObject.tag == "Gun")
        {
			if (Input.GetMouseButtonDown (0) == true) 
			{
				GameObject Projectile = Instantiate (WeaponsOnHand[WeaponIndex].GetComponent<WeaponClass>().Projectile, SpawnPoint.transform.position, SpawnPoint.transform.rotation) as GameObject;
				Projectile.GetComponent<Rigidbody2D> ().AddForce (transform.up * 100, ForceMode2D.Force);
				Debug.Log ("Shoot");
			}
        }
	}

    void AttackPowerChecker()
    {
        if (WeaponsOnHand[WeaponIndex] != null)
        {
            AttackPower = WeaponsOnHand[WeaponIndex].GetComponent<WeaponClass>().WeaponPower;
        }
        else
        {
            AttackPower = 1;
        }
            

    }

    void AnimationChecker()
    {
        //LEGS
        if (RB2D.velocity.x != 0f || RB2D.velocity.y != 0f)
        {
            LegAnimator.SetBool("IsMoving", true);
        }
        else
        {
            LegAnimator.SetBool("IsMoving", false);
        }
    }
}
