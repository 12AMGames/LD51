using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathParticalEffect;
    [SerializeField] float playerKnockbackAmount = 1;
    [SerializeField] float pointsValue = 100;
    [SerializeField] int playerHealAmount = 1;
    [SerializeField] int health = 1;

    [HideInInspector]
    public  Animator spriteAnim;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public AngleToPlayer angleToPlayer;

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteAnim = GetComponentInChildren<Animator>();
        angleToPlayer = GetComponent<AngleToPlayer>();
    }

    public virtual void Update()
    {
        agent.destination = GameManager.Instance.playerTransform.position;
        spriteAnim.SetFloat("spriteRot", angleToPlayer.lastIndex);
    }

    public virtual void DamageEnemy(int damageAmount)
    {
        if (deathParticalEffect)
        {
            Instantiate(deathParticalEffect, transform.position, transform.rotation);
        }
        health -= damageAmount;
        if (health <= 0)
        {
            KillEnemy();
        }
    }

    public virtual void KillEnemy()
    {
        //LevelManager.Instance.enemiesKilled++;
        ScoreManager.Instance.AddPoints(pointsValue);

        PlayerHealth playerH = GameManager.Instance.playerTransform.gameObject.GetComponent<PlayerHealth>();
        playerH.HealPlayer(playerHealAmount);

        Destroy(gameObject);
    }

    IEnumerator EnemyStun()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        Debug.Log("yuh");
        agent.isStopped = false;
        yield break;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (agent.isStopped)
                return;
            collision.gameObject.GetComponent<PlayerController>().Knockback(transform.forward * playerKnockbackAmount);
            StartCoroutine(EnemyStun());
        }
    }
}
