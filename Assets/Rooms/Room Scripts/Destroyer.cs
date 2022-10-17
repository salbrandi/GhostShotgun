using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameObject startPoint;
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        Destroy(startPoint);
    }
}
