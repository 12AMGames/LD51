using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;

    [SerializeField] private Transform firePoint;
    [SerializeField] float fireRate = 1f;

    private void Start()
    {
        InvokeRepeating("Fire", 1f, fireRate);
    }

    void Fire()
    {
        Instantiate(projectilePrefab, firePoint.position, transform.rotation);
    }
}
