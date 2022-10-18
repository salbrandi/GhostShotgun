using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunShell : MonoBehaviour
{
    public float startVelocity, falloff, timeToLive, damage, waitTime;

    public GameObject source, deathprefab;

    float timer;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeToLive;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * startVelocity;
            startVelocity = Mathf.Lerp(startVelocity, 0, falloff * Time.deltaTime);
        }
        else
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.gameObject.CompareTag("Wall"))
        {
            Die();
        }

        if (source != null)
        {
            if (source.CompareTag("Shotgun"))
            {
                if (other.collider.gameObject.CompareTag("Damageable"))
                {
                    other.collider.gameObject.GetComponentInChildren<Damageable>().TakeDamage(damage);
                    other.collider.gameObject.GetComponent<EnemyBehaviour>()?.SetLastDamageSource(this.transform.position);
                    Die();
                }
            }
        }
        else
        {
            if (other.collider.gameObject.CompareTag("Player"))
            {
                other.collider.gameObject.GetComponentInChildren<Damageable>().TakeDamage(damage);
                Die();
            }
        }



    }

    void Die()
    {
        if (deathprefab != null)
            Instantiate(deathprefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
