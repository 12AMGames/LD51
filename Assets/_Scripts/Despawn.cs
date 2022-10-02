using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Despawn : MonoBehaviour
{
    public float timer = 0f;
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
            DestroyMe();
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}