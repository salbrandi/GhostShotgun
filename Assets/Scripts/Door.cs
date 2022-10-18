using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    int numEnemies;

    public GameObject door1, door2;

    HashSet<GameObject> objsInCollider = new HashSet<GameObject>(); 

    bool closed = false;

    float timer = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update(){
        if(timer > 0){
            timer -= Time.deltaTime;
        } else {
            if(objsInCollider.Count == 0){
                closed = false;
            } else {
                closed = true;
            }
            objsInCollider.Clear();    
            timer = 0.5f;
            door1.SetActive(closed);
            door2.SetActive(closed);
        }
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            closed = true;
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.CompareTag("Damageable")){
            objsInCollider.Add(other.gameObject);
        }

    }
}
