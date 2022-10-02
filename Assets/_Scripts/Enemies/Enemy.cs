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

    Rigidbody rb;
    [HideInInspector]
    public  Animator spriteAnim;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public AngleToPlayer angleToPlayer;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        AudioManager.PlaySoundAtPoint(SoundNames.EnemyHurt, transform.position);

        health -= damageAmount;
        StartCoroutine(EnemyStun());
        if (deathParticalEffect)
        {
            Instantiate(deathParticalEffect, transform.position, transform.rotation);
        }
        if (health <= 0)
        {
            KillEnemy();
        }
    }

    public virtual void KillEnemy()
    {
        AudioManager.PlaySoundAtPoint(SoundNames.EnemyDie, transform.position);

        ScoreManager.Instance.AddPoints((int)pointsValue);

        PlayerHealth playerH = GameManager.Instance.playerTransform.gameObject.GetComponent<PlayerHealth>();
        playerH.HealPlayer(playerHealAmount);

        Destroy(gameObject);
    }

    IEnumerator EnemyStun()
    {
        agent.isStopped = true;
        //agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(-transform.forward * 100);
        yield return new WaitForSeconds(1f);
        Debug.Log("yuh");
        rb.isKinematic = true;
        //agent.enabled = true;
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
