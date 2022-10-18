using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour, Damageable
{
    private Vector2 moveInput, prevInput;
    private Rigidbody2D rb;

    public float maxHealth = 10f;
    public float currentHealth, fireInterval;
    public bool canMove = true;
    public bool canAttack = true;
    public bool canBeDamaged = true;

    public AudioSource shotgunSound, hurtSound;

    private Animator animator { get; set; }

    public float baseSpeed = 5f;

    public float dashSpeed = 20f;

    public float dashCoolDown = 2f;

    public float dashLength = 0.5f;
    float dashCounter, dashCoolCounter;

    public GameObject Shotgun;

    public int soulCharge;

    bool isColliding;

    public bool won = false;

    public GameObject winScreen, lossScreen;
    float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        soulCharge = 0;
        animator = GetComponentInChildren<Animator>();
        fireTimer = fireInterval;
    }

    void FixedUpdate()
    {
        if (dashCounter > 0 && !isColliding)
        {
            rb.MovePosition(rb.position + prevInput * dashSpeed * Time.deltaTime);
        }


        if (canMove)
        {
            rb.MovePosition(rb.position + moveInput * baseSpeed * Time.deltaTime);
            if (Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) && moveInput.y > 0)
            {
                animator.SetBool("Horizontal", false);
                animator.SetBool("Up", true);
            }
            else if (Mathf.Abs(moveInput.y) < Mathf.Abs(moveInput.x))
            {
                animator.SetBool("Horizontal", true);
                animator.SetBool("Up", false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {



        if(won){
            winScreen.SetActive(true);
            animator.SetBool("Won", true);
            moveInput = Vector3.zero;
            return;
        }

        if(currentHealth <= 0 && !won){
            lossScreen.SetActive(true);
            return;
        }

        if(fireTimer > 0){
            fireTimer -= Time.deltaTime;
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                // Dash is over
                dashCoolCounter = dashCoolDown;
                canBeDamaged = true;
                canMove = true;
                canAttack = true;
                Physics2D.IgnoreLayerCollision(7, 10, false);
                Shotgun.GetComponent<MouseSwivel>().dashFire(soulCharge);
                soulCharge = 0;
                animator.SetBool("Dashing", false);


            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

    }

    void OnMove(InputValue val)
    {

        moveInput = val.Get<Vector2>();
        if (moveInput.magnitude > 0)
        {
            if (canMove)
                prevInput = val.Get<Vector2>();
            updateFacingDirection();
        }
        else
        {
            animator.SetBool("Horizontal", false);
            animator.SetBool("Up", false);

        }

    }

    void OnDodge()
    {
        if ((dashCoolCounter <= 0 && dashCounter <= 0 && canMove) || (canMove && dashCounter <= 0 && soulCharge > 0))
        {
            dashCounter = dashLength;
            canMove = false;
            canAttack = false;
            canBeDamaged = false;
            // Ignore Layer collision between player and bullets
            animator.SetBool("Dashing", true);
            Physics2D.IgnoreLayerCollision(7, 10);
        }

    }

    void OnFire()
    {
        if (canAttack && fireTimer <= 0)
        {
            shotgunSound.PlayOneShot(shotgunSound.clip, 0.5f);
            Shotgun.GetComponent<MouseSwivel>().Fire();
            fireTimer = fireInterval;
        }
    }

    void updateFacingDirection()
    {
        if (moveInput.x != 0)
        {
            if (moveInput.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(float damage)
    {
        hurtSound.PlayOneShot(hurtSound.clip, 0.5f);
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 6)
            isColliding = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }

    public void Restart(){
        winScreen.SetActive(false);
        SceneManager.LoadScene("FinalGame");
    }



}
