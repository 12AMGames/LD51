using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIcon : MonoBehaviour
{
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetHeart(HeartState status)
    {
        anim.SetFloat("HealthState", (float)status);
    }
}

public enum HeartState
{
    Empty = 2,
    Half = 1,
    Full = 0
}

