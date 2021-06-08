using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerPhysics : MonoBehaviour
{    
    // movement variables
    Rigidbody2D PlayerRigidbody;
    public float MoveSpeed = 5;
    float xVelocity;

    // jump variables
    int JumpLimiter;
    public float JumpForce;
    public bool Grounded = false;

    // dash variables
    public float DashSpeed;
    float DashTime;
    public float StartDashTime;
    int Direction;
    int DashLimiter;

    // colliding with Soundmills/Cable cars
    bool IsOnSoundmill = false;
    bool IsOnCableCar = false;

    Quaternion PlayerRotation;
    Vector3 PlayerScale;


    bool FacingRight = true;

    // Text
    public TMPro.TMP_Text PlayerText;
    public RectTransform PlayerTextRect;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerRotation = transform.rotation;
        PlayerScale = transform.localScale;
        PlayerText.text = "yeet";
        PlayerTextRect = PlayerText.GetComponent<RectTransform>();
        PlayerTextRect.transform.SetParent(PlayerText.transform);
        PlayerText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Grounded = Physics2D.OverlapBox(PlayerRigidbody.position + Vector2.up * -0.3f, transform.localScale * 0.98f, 0, ~LayerMask.GetMask("Player"));

        float xInput = Input.GetAxisRaw("Horizontal");

        float velocityDelta = 80;
        if (!Grounded)
            velocityDelta /= 10;

        xVelocity = Mathf.MoveTowards(PlayerRigidbody.velocity.x, xInput * MoveSpeed, velocityDelta * Time.deltaTime);

        PlayerRigidbody.velocity = new Vector2(xVelocity, PlayerRigidbody.velocity.y);

        if (FacingRight == false && xInput > 0)
            Flip();
        else if (FacingRight == true && xInput < 0)
            Flip();
    }

    private void Update()
    {
        // jump and double (/mulitple) jump
        if (Grounded == true)
        {
            JumpLimiter = 1;
            DashLimiter = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && JumpLimiter > 0)
        {
            PlayerRigidbody.velocity = Vector2.up * JumpForce;
            JumpLimiter--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && JumpLimiter == 0 && Grounded == true)
        {
            PlayerRigidbody.velocity = Vector2.up * JumpForce;
        }

        // dash
        if (Direction == 0)
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
                Direction = 1;
            else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
                Direction = 2;
        }

        else
        {
            if (DashTime <= 0)
            {
                Direction = 0;
                DashTime = StartDashTime;
                PlayerRigidbody.velocity = Vector2.zero;
            }
            else
            {
                DashTime -= Time.deltaTime;

                if (Direction == 1 && DashLimiter > 0)
                {
                    PlayerRigidbody.velocity = Vector2.left * DashSpeed;
                    DashLimiter--;
                }
                else if (Direction == 2 && DashLimiter > 0)
                {
                    PlayerRigidbody.velocity = Vector2.right * DashSpeed;
                    DashLimiter--;
                }
            }
        }
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Soundmill" && IsOnCableCar == false)
        {
            IsOnSoundmill = true;
            collision.collider.transform.SetParent(transform, false);

            transform.rotation = PlayerRotation;
            transform.localScale = PlayerScale;
            PlayerRigidbody.gravityScale = 0;
            xVelocity = 0;
            PlayerRigidbody.velocity = new Vector2(0, 0);
        }
        //collision.collider.transform.position = GrabPoint.transform.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //collision.collider.transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "interactive")
        {
            PlayerTextRect.anchoredPosition = new Vector2(transform.position.x, transform.position.y + 1);
            
            PlayerText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerText.gameObject.SetActive(false);
    }
}
