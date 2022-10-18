using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 = open bottom door
    // 2 = open top door
    // 3 = left door
    // 4 = right door
    private RoomTemplate templates;
    private int rand;
    private bool spawned = false;
    public GameObject destroyerBlock;
    public float waitTime = 4f;
    void Start()
    {
        Destroy(gameObject, waitTime);
        Destroy(destroyerBlock, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        Invoke("Spawn", 0.2f);
    }
    
    void Spawn()
    {
        if(spawned == false)
        {
            if(openingDirection == 1)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if(openingDirection == 2)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if(other.GetComponent<RoomSpawner>().spawned == false&& spawned == false)
            {
                Instantiate(templates.closedRooms, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;

        }
    }
}
