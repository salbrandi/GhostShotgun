using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EliteBehaviour : MonoBehaviour, Damageable
{

    public float detectionRadius, movementInterval, firingInterval, maxHealth, cultistRadius;

    float fireTimer, currentHealth;

    public GameObject bulletPrefab, cultistPrefab;

    public List<Transform> teleportPads;

    public Animator animator;

    GameObject Player;
    Rigidbody2D rb;

    bool attacked = false;
    bool teleported = false;

    bool spawned = false;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        fireTimer = firingInterval * 4;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 playerLine = Player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerLine, 20f, LayerMask.GetMask("Wall", "Player"));
        if (hit.collider == null || (!hit.collider.gameObject.CompareTag("Player"))) return;


        if(currentHealth <= 0){
            animator.SetBool("Dying", true);
        }

        
        // We use firinginterval * 4 to represent 3 distinct phases of attack: bullet, cultist spawn, bullet,
        // and then a delay before teleporting  
        if (fireTimer > firingInterval * 3 || (fireTimer < firingInterval * 2 && fireTimer > firingInterval))
            {
                if(!attacked){
                    animator.SetTrigger("Attacking");
                    Fire();
                    attacked = true;
                }
            } else if (fireTimer > firingInterval * 2 && fireTimer < firingInterval * 3) {
                if(!spawned){
                    animator.SetTrigger("Attacking");
                    SpawnCultists();
                    spawned = true;
                    attacked = false;
                    teleported = false;
                }
            } else if (fireTimer <= 0) {
                if(!teleported){
                    animator.SetTrigger("Attacking");
                    TeleportToPad();
                    teleported = true;
                    attacked = false;
                    spawned = false;
                    fireTimer = firingInterval * 4;
                }
            }

        

        fireTimer -= Time.deltaTime;
    }

    void Fire()
    {
        var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        //obj.GetComponent<ShotGunShell>().source = gameObject;

    }

    void SpawnCultists(){
        for(int i = 0; i < 2; i++){
            Quaternion angle = Quaternion.Euler(0, 0, 120 * i -60);
            Vector3 spawnPos = transform.position + angle * (Player.transform.position - transform.position).normalized * cultistRadius;
            Instantiate(cultistPrefab, spawnPos, Quaternion.identity);

        }
    }

    void TeleportToPad(){
        // Instantiate teleport effects
        transform.position = teleportPads[((int) Random.Range(0, 4))].position;
    }

    
    public void TakeDamage(float damage){
        currentHealth -= damage;
    }

    public void Die(){
        Player.GetComponent<CharacterController>().won = true;
        Destroy(gameObject);
    }
}
