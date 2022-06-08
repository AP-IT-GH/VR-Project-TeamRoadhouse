using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBehaviour : MonoBehaviour
{
    public Transform spawnpoint = null;
    public GameObject prefab = null;
    
    private void Start()
    {
        spawnBall();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            spawnBall();
        }
    }

    private void spawnBall() {
        Instantiate(prefab, spawnpoint.position, spawnpoint.rotation);
    }
}
