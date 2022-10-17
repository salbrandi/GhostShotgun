using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameObject startPoint;
    private bool deleted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        deleted = true;
        if(deleted = true)
        {
            Destroy(startPoint);
            deleted = false;
        }

    }
}
