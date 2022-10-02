using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Despawn : MonoBehaviour
{
    [SerializeField] float timer = 0f;
    public UnityEvent OnTimerEnd;

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            OnTimerEnd?.Invoke();
        }
    }

    public virtual void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Planning:
                OnTimerEnd?.Invoke();
                break;
            case GameState.Playing:
                break;
            case GameState.LevelEnd:
                break;
            default:
                Debug.LogError("Why you break");
                break;
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
    }    
}