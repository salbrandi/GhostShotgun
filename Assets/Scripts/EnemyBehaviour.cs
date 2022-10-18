using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, Damageable
{
    GameObject Player;
    public float speed, movementRadius, detectionRadius, movementInterval, firingInterval, maxHealth;
    float moveTimer, fireTimer, currentHealth;
    Vector2 targetPos;
    Vector2 prevDirection;
    Vector3 lastDamageSource;
    Animator animator;

    public GameObject bulletPrefab, pickupPrefab;

    Rigidbody2D rb;

    public void SetLastDamageSource(Vector3 v){
        lastDamageSource = v;
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        moveTimer = movementInterval + Random.Range(-movementInterval / 3, movementInterval / 3);
        fireTimer = firingInterval + +Random.Range(-firingInterval / 3, firingInterval / 3);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        prevDirection = Vector2.zero;
        currentHealth = maxHealth;
    }

    float xWeight, yWeight;

    void FixedUpdate()
    {
        if (moveTimer > movementInterval)
        {
            rb.MovePosition(rb.position + targetPos.normalized * Time.deltaTime * speed);
            if (targetPos.normalized.y > targetPos.x)
            {
                animator.SetFloat("Vertical", targetPos.normalized.y);
                animator.SetFloat("Horizontal", 0);

            }
            else if (targetPos.normalized.y < targetPos.x)
            {
                animator.SetFloat("Horizontal", targetPos.normalized.x);
                animator.SetFloat("Vertical", 0);
            }
        } else {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0){
            rb.AddForce((transform.position - lastDamageSource).normalized * 10, ForceMode2D.Impulse);
            animator.SetBool("Dying", true);
        }

        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }
        else
        {
            // Choose a random point within detection radius and move to it
            targetPos = new Vector2(Random.Range(-movementRadius, movementRadius), Random.Range(-movementRadius, movementRadius)) - prevDirection;
            prevDirection = Vector2.zero;
            moveTimer = movementInterval * 2;
            if (fireTimer <= 0)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < detectionRadius)
                {
                    Vector2 playerLine = Player.transform.position - transform.position;
                    var hit = Physics2D.Raycast(transform.position, playerLine, 100f, LayerMask.GetMask("Wall", "Player"));
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        animator.SetTrigger("Attacking");
                        var audio = GetComponent<AudioSource>();
                        audio.PlayOneShot(audio.clip, 0.1f);
                        Fire(Mathf.Atan2(playerLine.y, playerLine.x) * Mathf.Rad2Deg);
                        fireTimer = firingInterval;
                    }
                }
            }
        }

        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

    }

    void OnCollisionEnter2D(Collision2D e)
    {
        moveTimer = movementInterval;
        prevDirection = targetPos;
        
    }

    void Fire(float angle)
    {
        var obj = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle + 180f));
        obj.GetComponent<ShotGunShell>().source = gameObject;

    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
    }

    public void Die(){
        Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
