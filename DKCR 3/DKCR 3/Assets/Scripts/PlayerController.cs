using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool IsDashing;


    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    //public string CurrentRoom;

    public bool CanJump = true;
    public int jumps = 0;

    private bool IsGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool temp;
    private int tempDir;


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

        if (j && CanJump) //IsGrounded does work >~<
        {
            //if (Dashtime > 0)
            {
                //Dashtime = 0;
                //rb.velocity = new Vector2(MaxDashSpeed, JumpForce);
            }
            //else
            {
                CanJump = false;
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                jumps--;
                if (IsDashing && Dashtime > 1)
                {
                    temp = true;
                    tempDir = Direction;
                    //print("y ");
                }
            }
                


        }

        if (FacingRight == false && x > 0)
        {
            Flip();
        }
        else if (FacingRight == true && x < 0)
        {
            Flip();
        }

        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E)) && CanDash && IsGrounded)
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //print(CanDash);

        if (IsGrounded && (rb.velocity.y * Mathf.Sign(rb.velocity.y)) <= 0.5)
            CanJump = true;
            //jumps = 1;
        //CanJump = (jumps > 0) ? true : false;

        else if (!IsGrounded && !IsDashing)
        {
            print("yeah");
            CanJump = false;
        }

        if (temp)
        {
            //direction of roll only plz
            if ((!IsGrounded || rb.velocity.y != 0) && x == tempDir)
            {
                print(!IsGrounded +" "+ (rb.velocity.y != 0) +" "+ (x == tempDir));
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * MaxDashSpeed, rb.velocity.y);
            }
            else
            {
                temp = false;
            }

        }
            

        //print(rb.velocity + " " + temp);
        //print(temp);
    }


    void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        
        

        float TargetSpeed = x * MaxSpeed;

        float SpeedDiff = TargetSpeed - rb.velocity.x;

        //basicly an if statement: if Mathf.Abs(TargetSpeed) > 0.01 = true then accelRate = Acceleration else accelRate = Decceleration
        float AccelRate = (Mathf.Abs(TargetSpeed) > 0.01) ? Acceleration : Decceleration;

        float Movement = Mathf.Pow(Mathf.Abs(SpeedDiff) * AccelRate, Speed) * Mathf.Sign(SpeedDiff);

        if(IsGrounded)
            rb.AddForce(Movement * Vector2.right);
        else
            rb.AddForce(Movement * Vector2.right / 2);

        //if (rb.velocity.x * rb.velocity.x <= (x * Speed * Time.fixedDeltaTime) * (x * Speed * Time.fixedDeltaTime))
        //    rb.velocity = new Vector2(x * Speed * Time.fixedDeltaTime, rb.velocity.y);





        if (Direction != 0)
        {
            if (Dashtime <= 0)
            {
                Direction = 0;
                Dashtime = StartDash;
                //rb.velocity = Vector2.zero;
                //rb.gravityScale = 2;
                //CanDash = false;
                jumps = 0;
            }
            else
            {
                Dashtime -= Time.deltaTime;
                //rb.gravityScale = 0;
                if (Mathf.Abs(rb.velocity.x) < MaxDashSpeed)
                    rb.velocity = new Vector2(DashSpeed * Time.deltaTime * Direction, rb.velocity.y);
                else
                    rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * MaxDashSpeed, rb.velocity.y);
                jumps = 1;
                


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
            if (FacingRight == true)
            {
                Direction = 1;
            }
            else if (FacingRight == false && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E)) && CanDash)
            {
                Direction = -1;
            }
        }

        CanDash = false;
        IsDashing = true;
        CanJump = true;
        yield return new WaitForSeconds(DashDelay);
        CanJump = false;
        IsDashing = false;
        CanDash = true;
    }









}
