using UnityEngine;
using System.Collections;

public class CutomerEnemyScript : EnemyController {

    public override void Update()
    {
        //If the player is in range, then the enemy will go towards them
        if ((Mathf.Abs(XDistance) <= DistanceThreshold) || (Mathf.Abs(YDistance) <= DistanceThreshold))
        {
            PolyNavagent.SetDestination(Player.transform.position);
            PatrolWayPoints.enabled = false;
            BodyAnimator.SetBool("IsAttacking", true);

        }
        else //If not will continue to do their thing
        {
            PolyNavagent.SetDestination(PatrolWayPoints.WPoints[PatrolWayPoints.currentIndex]);
            PatrolWayPoints.enabled = true;
            BodyAnimator.SetBool("IsAttacking", false);
        }

        base.Update();
    }
}
