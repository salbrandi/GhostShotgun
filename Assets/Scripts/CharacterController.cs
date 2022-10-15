using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour, Damageable
{
    private Vector2 moveInput, prevInput;
    private Rigidbody2D rb;

    public float maxHealth = 10f;
    public float currentHealth;
    public bool canMove = true;
    public bool canAttack = true;
    public bool canBeDamaged = true;

    private Animator animator { get; set; }

    public float baseSpeed = 5f;

    public float dashSpeed = 20f;

    public float dashCoolDown = 2f;

    public float dashLength = 0.5f;
    float dashCounter, dashCoolCounter;

    public GameObject Shotgun;

    bool isColliding;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

    }

    void FixedUpdate(){
        if(dashCounter > 0 && !isColliding){
            rb.MovePosition(rb.position + prevInput * dashSpeed * Time.deltaTime);
        }

        if (canMove)
            rb.MovePosition(rb.position + moveInput * baseSpeed * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {   
        if(dashCounter > 0){
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0){
                dashCoolCounter = dashCoolDown;
                canBeDamaged = true;
                canMove = true;
                canAttack = true;
                Physics2D.IgnoreLayerCollision(7, 10, false);
            }
        }

        if(dashCoolCounter > 0){
            dashCoolCounter -= Time.deltaTime;
        }

    }

    void OnMove(InputValue val)
    {

        moveInput = val.Get<Vector2>();
        if (moveInput.magnitude > 0 )
        {
            if(canMove)
                prevInput = val.Get<Vector2>();
                //animator.SetBool("Moving", true);
                updateFacingDirection();
        }
        else
        {
            //animator.SetBool("Moving", false);
        }
        
    }

    void OnDodge(){
        if(dashCoolCounter <= 0 && dashCounter <= 0 && canMove){
            dashCounter = dashLength;
            canMove = false;
            canAttack = false;
            canBeDamaged = false;
            // Ignore Layer collision between player and bullets
            // animator.setTriger("Dodging");
            Physics2D.IgnoreLayerCollision(7, 10);
        }

    }

    void OnFire(){
        if (canAttack){
            Shotgun.GetComponent<MouseSwivel>().Fire();
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

    public void TakeDamage(float damage){
        Debug.Log("took damage");
        currentHealth -= damage;
    }

    void OnCollisionEnter2D(Collision2D collision){
        isColliding = true;
    }

    void OnCollisionExit2D(Collision2D collision){
        isColliding = false;
    }


    
}
