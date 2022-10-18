using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRooms;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;
    public Vector3 furthestRoom;
    public GameObject startRoom;

    void Awake(){
        furthestRoom = startRoom.transform.position;
    }

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            foreach(GameObject o in rooms)
            {
                if (Vector3.Distance(o.transform.position, startRoom.transform.position) > Vector3.Distance(furthestRoom, startRoom.transform.position))
                {
                    furthestRoom = o.transform.position;
                }
                    
            }
            Instantiate(boss, furthestRoom, Quaternion.identity);
            spawnedBoss = true;

        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
