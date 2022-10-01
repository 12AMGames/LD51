using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kickable : MonoBehaviour
{
    [SerializeField] float kickForce = 0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Kick(Vector3 Dir, float Force = 1f)
    {
        if(kickForce == 0f)
        {
            kickForce = Force;
        }
        rb.AddForce(Dir * kickForce, ForceMode.Impulse);
    }
}
