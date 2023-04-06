using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Speed = 1;
    private float x;
    [SerializeField] private float Acceleration;
    [SerializeField] private float Decceleration;
    [SerializeField] private float MaxSpeed;

    public float JumpForce;
    private bool j;
    private bool FacingRight = false;
    public float DashSpeed;
    private float Dashtime = 0;
    public float StartDash;
    private int Direction = 0;
    private bool CanDash = true;
    public float DashDelay = 1f;
    public float MaxDashSpeed = 200f;
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
            rb.velocity = new Vector2(rb.velocity.x, JumpForce); 
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
            rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y + (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime));
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y + (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }


    void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        float TargetSpeed = x * MaxSpeed;

        float SpeedDiff = TargetSpeed - rb.velocity.x;

        //basicly an if statement: if Mathf.Abs(TargetSpeed) > 0.01 = true then accelRate = Acceleration else accelRate = Decceleration
        float AccelRate = (Mathf.Abs(TargetSpeed) > 0.01) ? Acceleration : Decceleration;

        float Movement = Mathf.Pow(Mathf.Abs(SpeedDiff) * AccelRate, Speed) * Mathf.Sign(SpeedDiff);

        rb.AddForce(Movement * Vector2.right);

        //if (rb.velocity.x * rb.velocity.x <= (x * Speed * Time.fixedDeltaTime) * (x * Speed * Time.fixedDeltaTime))
        //    rb.velocity = new Vector2(x * Speed * Time.fixedDeltaTime, rb.velocity.y);

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
                //rb.gravityScale = 2;
                CanDash = false;
            }
            else
            {
                Dashtime -= Time.deltaTime;
                //rb.gravityScale = 0;
                rb.velocity = new Vector2(DashSpeed * Time.deltaTime * Direction, rb.velocity.y);
                if (j && IsGrounded) 
                {
                    Direction = 0;
                    Dashtime = StartDash;
                }


                //if (Direction == 1)
                //{
                //    //rb.velocity = (Vector2.right * DashSpeed * Time.deltaTime) / ((Dashtime+1) * 2f);
                //    //rb.AddForce(Vector2.right * DashSpeed * Time.deltaTime, ForceMode2D.Impulse);
                //    if (rb.velocity.x <= MaxDashSpeed)
                //    {
                //        rb.velocity += Vector2.right * DashSpeed * Time.deltaTime;
                //    }
                //    if (Dashtime <= 0.1f)
                //    {
                //        rb.velocity -= Vector2.right * DashSpeed * Time.deltaTime;
                //    }
                //    print(rb.velocity + " " + Dashtime);
                //}
                //else if (Direction == -1)
                //{
                //    //rb.velocity = (Vector2.left * DashSpeed * Time.deltaTime) / ((Dashtime+1) * 2f);
                //    //rb.AddForce(Vector2.left * DashSpeed * Time.deltaTime, ForceMode2D.Impulse);


                //    //damd sdsdfsdxf
                //    if (rb.velocity.x >= -MaxDashSpeed)
                //    {
                //        rb.velocity += Vector2.left * DashSpeed * Time.deltaTime;
                //    }
                //    if (Dashtime <= 0.1f)
                //    {
                //        rb.velocity -= Vector2.left * DashSpeed * Time.deltaTime;
                //    }
                //    print(rb.velocity + " " + Dashtime);
                //}

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
