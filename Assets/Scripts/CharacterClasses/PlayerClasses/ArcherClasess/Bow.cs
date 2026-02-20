using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform spawnPoint;
    public float launchForce = 30f;
    public bool isPulledBack = false;

    void Update()
    {
        if (Input.GetButtonUp("Fire1")) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject newArrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();

        rb.AddForce(spawnPoint.forward * launchForce, ForceMode.Impulse);
    }
}
