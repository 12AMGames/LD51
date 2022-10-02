using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanThow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerController>().canThrow)
                return;
            other.gameObject.GetComponent<PlayerController>().handAnim.SetBool("HasSoda", true);
            other.gameObject.GetComponent<PlayerController>().canThrow = true;
            Destroy(gameObject);
        }
    }
}
