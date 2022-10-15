using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseSwivel : MonoBehaviour
{

    public GameObject bullet;
    public float distanceFromParent, maxAngle, minSpeedMod, gunFloatSpeed;
    public int numProjectiles;
    public Transform objectToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var parentPos = objectToFollow.transform.position;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var angle = mouseWorldPos - parentPos;
        angle = new Vector3(angle.x, angle.y, 0).normalized;
        // Debug.Log(angle);
        transform.position = Vector3.MoveTowards(transform.position, parentPos + (angle * distanceFromParent), gunFloatSpeed * Time.deltaTime);

        float rotation_z = Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 180);

    }

    public void Fire(){
        for(int i = 0; i < numProjectiles; i++){
            GameObject obj = Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(-maxAngle, maxAngle)));
            obj.GetComponent<ShotGunShell>().startVelocity *= Random.Range(1, minSpeedMod);
        }
        transform.localPosition = new Vector3(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.2f, 0);
    }
}
