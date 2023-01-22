using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Speed = 1;
    private float x;
    public float JumpForce;
    private bool j;
    private bool FacingRight = false;
    public float DashSpeed;
    private float Dashtime = 0;
    public float StartDash;
    private int Direction = 0;
    private bool CanDash = true;
    public float DashDelay = 1f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    //public string CurrentRoom;


    private bool IsGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Dashtime = StartDash;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        j = Input.GetButtonDown("Jump");

        if (j && IsGrounded) //IsGrounded does work >~<
        {
            rb.velocity = Vector2.up * JumpForce;
        }

        if (FacingRight == false && x > 0)
        {
            Flip();
        }
        else if (FacingRight == true && x < 0)
        {
            Flip();
        }

        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E)) && CanDash == true)
            StartCoroutine(DashInput());

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }


    void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        rb.velocity = new Vector2(x * Speed * Time.fixedDeltaTime, rb.velocity.y);

        if (j)
        {
            //rb.velocity = Vector2.up * JumpForce;
        }





        if (Direction != 0)
        {
            if (Dashtime <= 0)
            {
                Direction = 0;
                Dashtime = StartDash;
                //rb.velocity = Vector2.zero;
                rb.gravityScale = 2;
                CanDash = false;
            }
            else
            {
                Dashtime -= Time.deltaTime;
                rb.gravityScale = 0;
                if (Direction == 1)
                {
                    //rb.velocity = (Vector2.right * DashSpeed * Time.deltaTime) / ((Dashtime+1) * 2f);
                    rb.AddForce(Vector2.right, ForceMode2D.Impulse);
                    print(rb.velocity + " " + Dashtime);
                }
                else if (Direction == -1)
                {
                    //rb.velocity = (Vector2.left * DashSpeed * Time.deltaTime) / ((Dashtime+1) * 2f);
                    rb.AddForce(new Vector2(1, 0), ForceMode2D.Impulse);
                    print(rb.velocity + " " + Dashtime);
                }

            }
        }
    }
    void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);

    }

    private IEnumerator DashInput()
    {
        if (Direction == 0)
        {
            if (FacingRight == true && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E)) && CanDash)
            {
                Direction = 1;
            }
            else if (FacingRight == false && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E)) && CanDash)
            {
                Direction = -1;
            }
        }
        yield return new WaitForSeconds(DashDelay);
        CanDash = true;
    }









}
