using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAway : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Spawned") || other.CompareTag("endblock"))
        {
            GameObject o = GameObject.FindWithTag("Spawned");
            Destroy(o);
        }

    }
}