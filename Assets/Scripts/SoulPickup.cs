using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickup : MonoBehaviour
{
    GameObject Player;
    public float radius = 3;

    public float floatSpeed = 6;

    public float rotateSpeed = 5;
    float timeAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<CharacterController>().soulCharge += 1;
        timeAngle = Random.Range(0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        timeAngle += rotateSpeed * Time.deltaTime;
        Vector3 angle = new Vector3(Mathf.Sin(timeAngle), Mathf.Cos(timeAngle)) * radius;
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + angle, floatSpeed * Time.deltaTime);
        if(Player.GetComponent<CharacterController>().soulCharge == 0){
            Die();
        }
    }

    public void Die(){
        Destroy(gameObject);
    }

}
