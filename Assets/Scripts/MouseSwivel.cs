using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
            obj.GetComponent<ShotGunShell>().source = gameObject;
        }
        Vector3 kickback = new Vector3(0.5f, 0.2f, 0);
        transform.localPosition = transform.position + kickback;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var angle = mouseWorldPos - objectToFollow.transform.position;
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithVelocity(angle.normalized);
    }
}
