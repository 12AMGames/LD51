using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CORN : Enemy
{
    [SerializeField] float enemySearchRad = 1;

    public override void Update()
    {
        spriteAnim.SetFloat("spriteRot", angleToPlayer.lastIndex);
        if(Vector3.Distance(transform.position, GameManager.Instance.playerTransform.position) <= enemySearchRad)
        {
            agent.stoppingDistance = 0;
            agent.destination = GameManager.Instance.playerTransform.position;
            Debug.Log("workin");
        }
        else
        {
            agent.stoppingDistance = 100;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, enemySearchRad);
    }
}
