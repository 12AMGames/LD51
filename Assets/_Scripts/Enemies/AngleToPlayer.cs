using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CREDIT TO SPAWN CAMP GAMES FOR ALMOST ALL THIS CODE https://www.youtube.com/watch?v=qcXEcZmZ8kA
public class AngleToPlayer : MonoBehaviour
{
    private Transform player;
    private Vector3 targetPos;
    private Vector3 targetDir;

    private SpriteRenderer spriteRenderer;

    public float playerAngle;
    public int lastIndex;

    private void Start()
    {
        player = GameManager.Instance.playerTransform;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        targetDir = targetPos - transform.position;

        playerAngle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        Vector3 tempScale = Vector3.one;
        if (playerAngle > 0) { tempScale.x *= -1f; }
        spriteRenderer.transform.localScale = tempScale;

        lastIndex = GetIndex(playerAngle);
    }

    private int GetIndex(float angle)
    {
        //front
        if (angle > -22.5f && angle < 22.6f)
            return 0;
        if (angle >= 22.5f && angle < 67.5f)
            return 7;
        if (angle >= 67.5f && angle < 112.5f)
            return 6;
        if (angle >= 112.5f && angle < 157.5f)
            return 5;
        
        
        //back
        if (angle <= -157.5 || angle >= 157.5f)
            return 4;
        if (angle >= -157.4f && angle < -112.5f)
            return 3;
        if (angle >= -112.5f && angle < -67.5f)
            return 2;
        if (angle >= -67.5f && angle <= -22.5f)
            return 1;
        
        return lastIndex;
    }

    private void OnDrawGizmosSelected()
    {
      
    }
}
