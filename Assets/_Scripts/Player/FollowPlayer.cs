using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerHead;
    
    void Update()
    {
        transform.position = playerHead.position;
    }
}
