using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator spriteAnim;
    private AngleToPlayer angleToPlayer;
    private int health = 1;

    private void Start()
    {
        spriteAnim = GetComponentInChildren<Animator>();
        angleToPlayer = GetComponent<AngleToPlayer>();
    }

    public void DamageEnemy(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        GameObject effect = Instantiate(deathParticalEffect, transform.position, Quaternion.identity);
        ParticleSystem.MainModule settings = effect.GetComponent<ParticleSystem>().main;
        settings.startColor = GetComponent<SpriteRenderer>().color;
        //LevelManager.Instance.enemiesKilled++;
        Destroy(gameObject);
    }

    private void Update()
    {
        spriteAnim.SetFloat("spriteRot", angleToPlayer.lastIndex);
    }
}
