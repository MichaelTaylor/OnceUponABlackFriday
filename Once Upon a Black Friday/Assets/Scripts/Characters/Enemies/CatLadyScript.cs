using UnityEngine;
using System.Collections;

public class CatLadyScript : EnemyController {

    public Transform SpawnPoint;
	public bool CanShoot;
    public GameObject Projectile;
	public float RateOfFire;

    public override void Update()
    {
        //If the player is in range, then the enemy will go towards them
        if (((Mathf.Abs(XDistance) <= DistanceThreshold) || (Mathf.Abs(YDistance) <= DistanceThreshold)))
        {
            PolyNavagent.SetDestination(Player.transform.position);
            PolyNavagent.stoppingDistance = 0.25f;
            PatrolWayPoints.enabled = false;
            BodyAnimator.SetBool("IsAttacking", true);
        }
        else //If not will continue to do their thing
        {
            PolyNavagent.SetDestination(PatrolWayPoints.WPoints[PatrolWayPoints.currentIndex]);
            PolyNavagent.stoppingDistance = 0.001f;
            PatrolWayPoints.enabled = true;
            BodyAnimator.SetBool("IsAttacking", false);
        }

		if ((Mathf.Abs(XDistance) <= 0.75f) && (Mathf.Abs(YDistance) <= 0.75f) && CanShoot == true)
        {
			Debug.Log("Found You");
			GameObject Cat = Instantiate(Projectile, SpawnPoint.transform.position, SpawnPoint.transform.rotation) as GameObject;
			Cat.GetComponent<Rigidbody2D>().AddForce(transform.up * 100, ForceMode2D.Force);
			Destroy (Cat, 4f);
			CanShoot = false;
			Invoke ("CanShootRecover", RateOfFire);
        }

        base.Update();
    }

	void CanShootRecover()
	{
		CanShoot = true;
	}
}
