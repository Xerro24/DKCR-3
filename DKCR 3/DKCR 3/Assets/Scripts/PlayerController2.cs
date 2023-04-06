using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    // movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rollSpeed = 10f;
    public float rollDuration = 1f;
    public float rollCooldown = 1f;
    private bool isGrounded = false;
    private bool isRolling = false;
    private float rollTimer = 0f;
    private float rollCooldownTimer = 0f;

    // reference to components
    private Rigidbody2D rb2d;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle movement
        float horizontalInput = Input.GetAxis("Horizontal");
        if (isRolling)
        {
            if (isGrounded)
            {
                rb2d.velocity = new Vector2(transform.localScale.x * rollSpeed, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(transform.localScale.x * rollSpeed, rb2d.velocity.y);
                rb2d.AddForce(Vector2.right * horizontalInput * moveSpeed, ForceMode2D.Impulse);
            }
            rollTimer += Time.deltaTime;
            if (rollTimer >= rollDuration)
            {
                isRolling = false;
                rollTimer = 0f;
                rollCooldownTimer = rollCooldown;
            }
        }
        else
        {
            rb2d.velocity = new Vector2(horizontalInput * moveSpeed, rb2d.velocity.y);
            // handle rolling
            if (Input.GetKeyDown(KeyCode.J) && isGrounded && rollCooldownTimer <= 0f)
            {
                isRolling = true;
                //anim.SetTrigger("Roll");
            }
            else if (rollCooldownTimer > 0f)
            {
                rollCooldownTimer -= Time.deltaTime;
            }
        }

        // handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isRolling)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        else if (Input.GetButtonDown("Jump") && isRolling)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        // flip sprite based on direction of movement
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // update animator variables
        //anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        //anim.SetBool("Grounded", isGrounded);
        //anim.SetBool("Rolling", isRolling);
    }

    // handle collisions with ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

