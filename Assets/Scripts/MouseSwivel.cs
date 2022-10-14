using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseSwivel : MonoBehaviour
{

    public GameObject bullet;
    public float distanceFromParent, maxAngle, minSpeedMod;
    public int numProjectiles;
    // public Transform objectToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var parentPos = transform.parent.transform.position;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var angle = mouseWorldPos - parentPos;
        transform.position = parentPos + angle.normalized * distanceFromParent;

        float rotation_z = Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 180);

    }

    void OnFire(){
        for(int i = 0; i < numProjectiles; i++){
            GameObject obj = Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(-maxAngle, maxAngle)));
            obj.GetComponent<ShotGunShell>().startVelocity *= Random.Range(1, minSpeedMod);
        }
    }
}
