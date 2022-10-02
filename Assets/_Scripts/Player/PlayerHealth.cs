using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerHealthUpdated;

    [SerializeField]
    private TextMeshProUGUI timerText;
    public float playerHealth = 6;
    public float playerMaxHealth = 6;

    private Rigidbody rb;
    private PlayerController player;

    public GameObject GameOverUI;

    private void Start()
    {
        playerHealth = playerMaxHealth;
        timerText = GetComponent<PlayerController>().handAnim.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
        StartCoroutine(DyingSlowlyHeJustLikeMeFr());
    }

    private void Update()
    {
        //if (healthText != null) healthText.text = health.ToString();
        if (transform.position.y < -10)
        {
            Die();
        }
    }

    public void HealPlayer(int amt)
    {
        playerHealth += amt;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        OnPlayerHealthUpdated?.Invoke();
        Debug.Log(playerHealth);
    }

    public void DamagePlayer(int amt)
    {
        AudioManager.PlaySound(SoundNames.PlayerHurt);

        playerHealth -= amt;
        OnPlayerHealthUpdated?.Invoke();
        if(playerHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator DyingSlowlyHeJustLikeMeFr()
    {
        int timerCount = 10;
        timerText.text = "Grace period";
        yield return new WaitForSeconds(10);
        while (true)
        {
            timerCount--;
            timerText.text = timerCount.ToString();
            if(timerCount <= 0)
            {
                ScoreManager.Instance.AddPoints(Mathf.RoundToInt(1000 / playerHealth));
                timerCount = 10;
                DamagePlayer(Mathf.RoundToInt(playerHealth / 2));
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Die()
    {
        Instantiate(GameOverUI, GameObject.FindGameObjectWithTag("UI").transform);
        GetComponent<PlayerController>().enabled = false;
        ScoreManager.Instance.SaveStats();
        yield return new WaitForSeconds(2);
        LevelLoaderManager.Instance.Restart();
    }

    private void ReEnable()
    {
        player.enabled = true;
    }
}
