using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = GameManager.Instance.playerTransform;
    }

    private void Update()
    {
        Vector3 modifiedTarget = target.position;
        modifiedTarget.y = transform.position.y;
        transform.LookAt(modifiedTarget);
    }
}
