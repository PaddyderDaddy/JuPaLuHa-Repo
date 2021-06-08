using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerPhysics : MonoBehaviour
{
    Rigidbody2D ChaRigidbody;
    SpriteRenderer Renderer;
    BoxCollider2D ChaBoxCollider;

    public float MoveSpeed = 10;
    float xVelocity;

    public float jumpDelay = 0.25f;
    private float jumpTimer;
    bool Jumping = false;

    public float OverlapBoxDistance = 0;
    public bool Grounded = false;

    //GRAB
    public bool TouchingObject = false;
    public Vector2 upOffset;
    public float collisionRadius;
    public Color gizmoColor = Color.red;

    public float GravityNormal = -9.81f;

    //LEVER
    public bool touchLever = false;

    //DOUBLE JUMP
    int ExtraJumps;
    public float JumpForce;

    public bool wasGrounded = false;

    //POWER DROP
    public float DropTimer;
    public float DropForce;

    //MOMENTUM JUMP
    public Transform grabDetect;
    //public Transform boxHolder; //bzw ich halte mich an dem objekt fest
    public float rayDist;

    GameObject PlayerObj;
    public Transform Player;
    public static Vector3 PlayerVector;

    //HOOK GRAB
    public Transform HookGrab;
    static bool direction = false;
    private Transform Target;
    bool HookDetect = false;
    public float FixeScale = 1;
    bool hookup = false;
    public float hookupheight = 1.1f;
    public Transform PLPO;
    public Transform HookVIS;

    //SOUNDMILL
    float Rotationdirection;
    Transform SoundmillOffset; //ist nicht zugweisene
    float SoundOffsetAngle;
    float SoundOffsetAngleSavedPosition;
    public float Force;
    Vector2 SoundmillOffsetVector;
    //Rotation
    Quaternion rotationPlayer; //idfk what that is
    Quaternion rotationPLPO;
    Quaternion rotationHookGrab;
    public bool IsOnSoundMill = false;

    //CABLE CAR
    public bool IsOnCableCar = false;
    Transform soundmill;

    //PARACUTE
    public Gliding Glieding;
    public Transform paracute;
    public GameObject Interaktiv;


    private void Awake()
    {
        rotationPLPO = PLPO.transform.rotation;
        rotationPlayer = transform.rotation; //Damit verändert sich meine Rotation (i hope) nicht
        rotationHookGrab = HookGrab.transform.rotation;
    }

    void Start()
    {
        ChaRigidbody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        ChaBoxCollider = GetComponent<BoxCollider2D>();

        Interaktiv.gameObject.SetActive(false);
        paracute.gameObject.SetActive(true);
    }

    public void GrabHook()
    {
        //just checking
        if (direction == false && IsOnSoundMill == false)
        {
            HookGrab.transform.localPosition = new Vector2(0.7f, hookupheight); //right up
        }
        if (direction == true && IsOnSoundMill == false)
        {
            HookGrab.transform.localPosition = new Vector2(-0.7f, hookupheight); //left up   
        }

        RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.right * transform.localScale, rayDist); //kann nur nach rechts schauen


        if (grabCheck.collider != null && grabCheck.collider.tag == "Pin")
        {
            //standart Kram
            IsOnSoundMill = true;
            ChaRigidbody.gravityScale = 0;
            xVelocity = 0;
            ChaRigidbody.velocity = new Vector2(0, 0);
            hookup = true;
            //Entparent
            PLPO.transform.parent = null; //Nicht mehr Child des Hooks
            HookGrab.transform.parent = null; //nicht mehr Child des Players
            //Parenten
            HookGrab.transform.parent = PLPO.transform; //Child des PLPO          
            Player.transform.parent = HookGrab.transform; //Child des Hooks                  

            //Parent = PIN
            Debug.Log("Ich erkenne etwas");
            Target = grabCheck.collider.gameObject.GetComponent<Transform>();
            PLPO.transform.parent = Target.transform;
            //Position
            PLPO.transform.localPosition = new Vector2(0, 0);
            HookGrab.transform.localPosition = new Vector3(0, -0.4f, 0);
            Player.transform.localPosition = new Vector2(-0.7f, -hookupheight);

            //Ursprungspunkt
            SoundmillOffset = Player.transform.root.GetComponent<Transform>();
            SoundmillOffsetVector = SoundmillOffset.transform.position;
            Debug.Log(SoundmillOffsetVector);
            HookDetect = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "interaction")
        {
            OpenInteraktableIcon();
            touchLever = true;
        }    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "CableCar" && IsOnSoundMill == false)
        {
            collision.collider.transform.SetParent(null);
        }

        if (collision.gameObject.tag == "interaction")
        {
            CloseInteraktableIcon();
            touchLever = false;
        }
    }

    void FixedUpdate()
    {
        //GROUNDED
        Grounded = Physics2D.OverlapBox(ChaRigidbody.position + Vector2.up * OverlapBoxDistance, transform.localScale * 0.98f, 0, LayerMask.GetMask("Level"));
        TouchingObject = Physics2D.OverlapCircle((Vector2)transform.position + upOffset, collisionRadius, LayerMask.GetMask("GrabbableObject"));

        float xInput = Input.GetAxisRaw("Horizontal");

        float velocityDelta = 50;

        if (IsOnSoundMill == false) //nicht auf den Soundmills möglich !
            xVelocity = Mathf.MoveTowards(ChaRigidbody.velocity.x, xInput * MoveSpeed, velocityDelta * Time.deltaTime);

        //HookGrab
        if (xVelocity < 1f && Input.GetKey(KeyCode.A) && IsOnSoundMill == false)
        {
            HookGrab.transform.localPosition = new Vector2(-0.7f, 0); //left
            direction = true;
        }
        else if (xVelocity > 1f && IsOnSoundMill == false)
        {
            HookGrab.transform.localPosition = new Vector2(0.7f, 0); //right
            direction = false;
        }
        else if (xVelocity == 0 && IsOnSoundMill == false)
        {
            if (direction == false)
                HookGrab.transform.localPosition = new Vector2(0.7f, 0); //right
            if (direction == true)
                HookGrab.transform.localPosition = new Vector2(-0.7f, 0); //left
        }

        ChaRigidbody.velocity = new Vector2(xVelocity, ChaRigidbody.velocity.y);

        //cablecarjump
        if (Input.GetKey(KeyCode.Space) && IsOnCableCar == true)
        {
            IsOnCableCar = false;
            Player.transform.parent = null; // collision.transform.SetParent(null);               
            HookDetect = false;
            //Jumping = true;
            ChaRigidbody.gravityScale = 1;
            ChaRigidbody.velocity = new Vector3(0, JumpForce / 5);
        }

        //Soundmilljump
        if (Input.GetKey(KeyCode.Space) && IsOnSoundMill == true && !Input.GetKey(KeyCode.E))
            SoundmillJump();

        //Soundmillgrab
        if (Input.GetKey(KeyCode.E))
            GrabHook();

        if (IsOnSoundMill == true)
            SoundOffsetAngleSavedPosition = SoundmillOffset.transform.rotation.z;

        if (Grounded == true)
        {
            paracute.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            Jumping = false;
            DropTimer = 0;
        }

        if (Input.GetKey(KeyCode.W) && Grounded == false)
        {
            paracute.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        if (Input.GetKey(KeyCode.W) && Glieding.IsGliding == true)
            paracute.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    void SoundmillJump()
    {
        //Debug.Log(CurrentRotation);
        Debug.Log(SoundOffsetAngle);
        //rotation
        SoundOffsetAngle = SoundmillOffset.transform.rotation.z;
        //VERSTOßEN
        Player.transform.parent = null;
        PLPO.transform.parent = null; //Nicht mehr Child des Hooks
        HookGrab.transform.parent = null; //nicht mehr Child des Players
        //PARENTEN
        HookGrab.transform.parent = Player.transform; //Hook = Child des Player
        PLPO.transform.parent = HookGrab.transform;   //PLPO = Child des Hooks   
        HookGrab.transform.localPosition = new Vector3(-0.7f, 0, 0);
        PLPO.transform.localPosition = new Vector3(0, 0.4f, 0);

        //Jumping = true;
        hookup = false;
        HookDetect = false;
        ChaRigidbody.gravityScale = 1;
        //transform.position += new Vector3(dirX, dirY) * Force;
        // MyRigidbody.velocity = new Vector3(-Mathf.Cos(SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;

        if (Input.GetKey(KeyCode.A))
        {
            ChaRigidbody.velocity = new Vector3(-Mathf.Cos(-45), Mathf.Sin(45)) * Force;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ChaRigidbody.velocity = new Vector3(Mathf.Cos(-45), Mathf.Sin(45)) * Force;
        }

        //if (SoundOffsetAngle < 0) //oben
        //{
        //Debug.Log("Oben");
        //if (SoundOffsetAngle < SoundOffsetAngleSavedPosition)// Im Uhrzeigersinn
        //{
        //    ChaRigidbody.velocity = new Vector3(Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
        //}
        //if (SoundOffsetAngle > SoundOffsetAngleSavedPosition)//gegen UhrzeigerSinn
        //    ChaRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
        //}

        //if (SoundOffsetAngle > 0) //unten
        //{
        //    Debug.Log("Unten");

        //    if (SoundOffsetAngle < SoundOffsetAngleSavedPosition)// Im Uhrzeigersinn                              
        //        ChaRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;

        //    if (SoundOffsetAngle > SoundOffsetAngleSavedPosition)// gegen UhrzeigerSinn
        //    {
        //        float floatabove180 = SoundOffsetAngleSavedPosition - SoundOffsetAngle;

        //        if (floatabove180 > 178) //Sobald der Character eine umdrehung gemacht hat ist Z von -180 plötzlich +180 und verfälscht deswegen hier die verbesserung das es funktioniert wie im uhrzeigersinn unten
        //        {
        //            ChaRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
        //        }
        //        if (floatabove180 < 180)
        //        {
        //            ChaRigidbody.velocity = new Vector3(Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
        //        }
        //    }
        //}
        IsOnSoundMill = false;
    }

    void Update()
    {
        if (Jumping == true)
            DropTimer += Time.deltaTime;

        //SIMPLE JUMP
        if (Input.GetKey(KeyCode.Space) && Grounded)
        {
            jumpTimer = Time.deltaTime + jumpDelay;
            ChaRigidbody.velocity = Vector2.up * JumpForce;
            Jumping = true;
        }

        //POWER DROP
        if (Input.GetKeyDown(KeyCode.S) && Jumping == true && DropTimer > 4)
        {
            ChaRigidbody.velocity = Vector2.down * JumpForce * 5;
        }

        /*
        //DOUBLE JUMP
        if (Grounded == true)
        {
            ExtraJumps = 1;
            //DashLimiter = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && ExtraJumps > 0)
        {
            jumpTimer = Time.time + jumpDelay;
            ChaRigidbody.velocity = Vector2.up * JumpForce;
            ExtraJumps--;
            //Height.GetComponent<Animator>().SetTrigger("stretch");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && ExtraJumps == 0 && Grounded == true)
        {
            jumpTimer = Time.time + jumpDelay;
            ChaRigidbody.velocity = Vector2.up * JumpForce;
            //Height.GetComponent<Animator>().SetTrigger("stretch");
        }
        */

        //GLIDING

        if (Input.GetKeyDown(KeyCode.W) && Grounded == false)
            FindObjectOfType<Gliding>().StartGliding();
        if (Input.GetKeyDown(KeyCode.S) && Grounded == false)
            FindObjectOfType<Gliding>().StopGliding();
        if (Grounded == true)
        {
            FindObjectOfType<Gliding>().StopGliding();
            paracute.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        //wasGrounded = Grounded;

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere((Vector2)transform.position + upOffset, collisionRadius);
        }
    }

    void LateUpdate()
    {
        transform.rotation = rotationPlayer;
        PLPO.transform.rotation = rotationPLPO;
        HookGrab.transform.rotation = rotationHookGrab;
    }

    public void OpenInteraktableIcon()
    {
        Interaktiv.gameObject.SetActive(true);
    }
    public void CloseInteraktableIcon()
    {
        Interaktiv.gameObject.SetActive(false);
    }
}
