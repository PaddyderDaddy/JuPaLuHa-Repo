using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    //Lucys Variablen
    //Rigidbody2D ChaRigidbody;
    //SpriteRenderer Renderer;

    //public float MoveSpeed = 10;

    //public float jumpDelay = 0.25f;
    //private float jumpTimer;

    //public float OverlapBoxDistance = 0;
    //public bool Grounded = false;

    ////GRAB
    //public bool TouchingObject = false;
    //public Vector2 upOffset;
    //public float collisionRadius;
    //public Color gizmoColor = Color.red;

    //public float GravityNormal = -9.81f;

    ////DOUBLE JUMP
    //int ExtraJumps;
    //public float JumpForce;

    //public bool wasGrounded = false;


    //Patricks Variablen
    public Transform grabDetect;
    public Transform boxHolder; //bzw ich halte mich an dem objekt fest
    public float rayDist;

    Rigidbody2D MyRigidbody; //So wie es auschaut kann ich hier das ganze einfügen
    GameObject PlayerObj;
    public Transform Player;
    public static Vector3 PlayerVector;


    public float MoveSpeed = 5;
    bool Jumping = false;
    float JumpTimer = 0;
    public float Jumpheight = 40;
    public bool Grounded = false;

    SpriteRenderer Texturerend; //later
    CapsuleCollider2D CollideCapsule;
    BoxCollider2D CollideBox;
    public Sprite[] Crouching;
    public Sprite[] Standing;
    public bool Crouch;

    float xVelocity;

    Rigidbody2D HookRigid;
    BoxCollider2D HookCollide;
    public Transform HookGrab;
    static bool direction = false;
    static bool GraHoTrigger = false;

    private Transform Target;
    float TargetAngle;
    bool HookDetect = false;

    public float FixeScale = 1;
    Quaternion rotationPlayer; //idfk what that is
    Quaternion rotationPLPO;
    Quaternion rotationHookGrab;
    bool IsOnSoundMill = false;

    Vector2 RotationSpeed = new Vector2();

    Vector2 savedpositiontarget;
    float savedanglefloat;
    Vector2 savedanglevec;
    float savedpositiony;

    bool hookup = false;
    public float hookupheight = 1.1f;
    public Transform PLPO;

    bool Istoben = false;
    bool istanderSeite = false;
    public Transform HookVIS;
    //Soundmill
    float Rotationdirection;
    Transform SoundmillOffset; //ist nicht zugweisene
    float SoundOffsetAngle;
    float SoundOffsetAngleSavedPosition;
    public float Force;
    Vector2 SoundmillOffsetVector;
    float dirX;
    float dirY;


    //CableCar
    bool IsOnCableCar = false;
    Transform soundmill;
    bool ImUhrzeigerSinn = false;
    float CurrentRotation;
    bool IgnoredCollision;

    //Patricks Code
    private void Awake()
    {
        rotationPLPO = PLPO.transform.rotation;
        rotationPlayer = transform.rotation; //Damit verändert sich meine Rotation (i hope) nicht
        rotationHookGrab = HookGrab.transform.rotation;
        //HookVIS.rotation = transform.rotation; FALSCH

    }
    void LateUpdate()
    {
        transform.rotation = rotationPlayer;
        PLPO.transform.rotation = rotationPLPO;
        HookGrab.transform.rotation = rotationHookGrab;
        //transform.rotation = HookVIS.rotation; FALSCH

    }

    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
        Texturerend = GetComponent<SpriteRenderer>();
        //CollideCapsule = GetComponent<CapsuleCollider2D>();
        CollideBox = GetComponent<BoxCollider2D>();
        HookCollide = GetComponent<BoxCollider2D>();
        HookRigid = GetComponent<Rigidbody2D>();
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
            MyRigidbody.gravityScale = 0;
            xVelocity = 0;
            MyRigidbody.velocity = new Vector2(0, 0);
            hookup = true;
            //Parent = Hook PLPO
            PLPO.transform.parent = null; //Nicht mehr Child des Hooks
            HookGrab.transform.parent = null; //nicht mehr Child des Players

            HookGrab.transform.parent = PLPO.transform; //Child des PLPO          
            Player.transform.parent = HookGrab.transform; //Child des Hooks         

            //Player.transform.parent = PLPO.transform; //Child des PLPO FUNKTIONIERENDE ALTERNATIVE = DENK DRAN ROTATION ETC

            //Parent = PIN
            Debug.Log("Ich erkenne etwas");
            Target = grabCheck.collider.gameObject.GetComponent<Transform>();
            PLPO.transform.parent = Target.transform;
            //Position
            PLPO.transform.localPosition = new Vector2(0, 0);
            HookGrab.transform.localPosition = new Vector3(0, -0.4f, 0);
            Player.transform.localPosition = new Vector2(-0.7f, -hookupheight);

            //SoundmillOffset = Target.GetComponentInParent<Transform>();
            //Player.transform.root = SoundmillOffset;
            //Ursprungspunkt
            SoundmillOffset = Player.transform.root.GetComponent<Transform>();
            //RotationSpeed = Player.parent.GetComponent("SoundMill");                  Später herausfinden wie das funktioiert
            SoundmillOffsetVector = SoundmillOffset.transform.position;

            //SoundOffsetAngleSavedPosition = SoundmillOffset.transform.rotation.z;
            Debug.Log(SoundmillOffsetVector);
            HookDetect = true;
            //Angle
            //savedpositiony = Target.position.y;
            // savedpositiontarget = transform.position;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "CableCar" && IsOnSoundMill == false) //Sobald auf einer Soundmill/Grounded/oder E gedrückt wird = Ignore CableCar
        {
            if (IsOnSoundMill == true || Grounded == true || Input.GetKey(KeyCode.E))
            {
                collision = GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(collision, Player.GetComponent<Collider2D>());
                Debug.Log("IgnoredCollision");
            }
            else
            {
                Player.transform.parent = collision.transform; //Child des PLPO 
                MyRigidbody.gravityScale = 0;
                MyRigidbody.velocity = new Vector2(0, 0);
                Player.transform.localPosition = new Vector2(0, 0);
                IsOnCableCar = true;
            }

            // collision.transform.SetParent(transform);
            //soundmill = gameObject.GetComponentInParent<Transform>();
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        /*
        if(collision.tag =="RichtungsTag")
            Istoben = true;
        if (collision.tag == "SeiteTag") ;
            istanderSeite = true;
        

        if(collision.tag == "CableCar")
            Debug.Log(soundmill.transform.position);*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Istoben = false;
        istanderSeite = false;


    }
    // Update is called once per frame

    void FixedUpdate() //hier wird die Physik immer geupdatet.
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        //hier kommt die Steuerung, ich bewege die X achse   
        //MyRigidbody.MovePosition(MyRigidbody.position + new Vector2(MoveSpeed, 0)); // Die Position wird teleportiert mit dem Rigidbody 
        //das Smoothing entweder über Input Manager (Sensitivity) oder Code:

        float VelocityDelta = 50;
        if (!Grounded)
            VelocityDelta /= 10;
        //if (IsOnSoundMill == false) //nicht auf den Soundmills möglich !
            xVelocity = Mathf.MoveTowards(MyRigidbody.velocity.x, xInput * MoveSpeed, VelocityDelta * Time.deltaTime); //Das macht das Smoothing (xVelocity = berechnung zwischen velocity und der xInput(+/-)*Time)

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


        MyRigidbody.velocity = new Vector2(xVelocity, MyRigidbody.velocity.y); //Das hier ist erstmal nur die Velocity im allg wird nicht auf das Objekt gemacht

        //Grounded? Kein Array wie bei Overlapboxall
        Grounded = Physics2D.OverlapBox(MyRigidbody.position + Vector2.up * -0.3f, transform.localScale * 0.95f, 0, LayerMask.GetMask("Level")/*, ~LayerMask.GetMask("LampEnv")*/);

        //transform und Rigidbody nicht unbedingt gleiche pos, deswegne nutzen wir Rig
        if (Input.GetKey(KeyCode.Space) && Jumping == false && Grounded == true)
        {
            Jumping = true;
            JumpTimer = 0;
            //Wir können auch eine "Addforceatposition" reinmachen, damit kann man zb ein Cube sich drehen lassen.
            //Der GetKeyDown muss in die Update funktion weil es in diesem Frame kein Physik update gab. Nicht in Fixed update nutzen...also nutzen wir GetKey          
        }
        if (Input.GetKey(KeyCode.Space) && Jumping == true)
        {
            JumpTimer += Time.deltaTime;
            if (JumpTimer > 0.1f) //Stop Jump after some time
                Jumping = false;
            else
                MyRigidbody.AddForce(new Vector2(0, Jumpheight));       //,ForceMode2D.Impulse); //der Impulse macht den Addforce größer, so das es länger als ein Frame ist. Sonst würde das nur ein Frame lang sein
        }

        if (Input.GetKey(KeyCode.Space) && IsOnCableCar == true)
        {
            IsOnCableCar = false;
            Player.transform.parent = null; // collision.transform.SetParent(null);
            //MyRigidbody.AddForce(new Vector2(0, Jumpheight * 1 ));     
            HookDetect = false;
            Jumping = true;
            MyRigidbody.gravityScale = 1;
            // MyRigidbody.velocity(new Vector2(0, Jumpheight*2));
            MyRigidbody.velocity = new Vector3(0, Jumpheight / 5);

        }

        if (Input.GetKey(KeyCode.Space) && IsOnSoundMill == true && !Input.GetKey(KeyCode.E)) //Soundmillsprung
        {
            SoundmillJump();
        }
        //crouch
        if (Input.GetKey(KeyCode.LeftShift))
        {
            CollideCapsule.size = new Vector2(CollideCapsule.size.x, 0.7f);
            CollideCapsule.offset = new Vector2(CollideCapsule.offset.x, -0.15f);
            Crouch = true;
        }
        else
        {
            CollideCapsule.size = new Vector2(CollideCapsule.size.x, 1f);
            CollideCapsule.offset = new Vector2(CollideCapsule.offset.x, 0f);
            Crouch = false;
        }
        if (Input.GetKey(KeyCode.E))
        {
            GrabHook();
        }


        if (IsOnSoundMill == true)
            SoundOffsetAngleSavedPosition = SoundmillOffset.transform.rotation.z;

    }

    void SoundmillJump()
    {
        //Debug.Log(CurrentRotation);
        Debug.Log(SoundOffsetAngle);

        Vector2 fromVector2 = transform.position;
        Vector2 toVector2 = savedpositiontarget;
        /*
        //var pinspeedy = Target.position.y - savedpositiontarget.y;
        //var pinspeedx = Target.position.x - savedpositiontarget.x;

        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (cross.z > 0)
           ang = 360 - ang;

        Debug.Log(ang);
        savedanglefloat = ang;
        */

        SoundOffsetAngle = SoundmillOffset.transform.rotation.z; //rotation         

        //VERSTOßEN
        Player.transform.parent = null;
        PLPO.transform.parent = null; //Nicht mehr Child des Hooks
        HookGrab.transform.parent = null; //nicht mehr Child des Players
                                          //PARENTEN
        HookGrab.transform.parent = Player.transform; //Hook = Child des Player
        PLPO.transform.parent = HookGrab.transform;   //PLPO = Child des Hooks   
        HookGrab.transform.localPosition = new Vector3(-0.7f, 0, 0);
        PLPO.transform.localPosition = new Vector3(0, 0.4f, 0);

        Jumping = true;
        //MyRigidbody.AddForce(new Vector2(0, Jumpheight * 1 ));
        hookup = false;
        HookDetect = false;
        MyRigidbody.gravityScale = 1;
        //MyRigidbody.freezeRotation = false;


        //Sprung

        //Debug.Log(SoundOffsetAngle);

        //transform.position += new Vector3(dirX, dirY) * Force;
        // MyRigidbody.velocity = new Vector3(-Mathf.Cos(SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
        if (SoundOffsetAngle < 0) //oben
        {
            Debug.Log("Oben");
            if (SoundOffsetAngle < SoundOffsetAngleSavedPosition)// Im Uhrzeigersinn
            {
                MyRigidbody.velocity = new Vector3(Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
            }
            if (SoundOffsetAngle > SoundOffsetAngleSavedPosition)//gegen UhrzeigerSinn
                MyRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
        }

        if (SoundOffsetAngle > 0) //unten
        {
            Debug.Log("Unten");

            if (SoundOffsetAngle < SoundOffsetAngleSavedPosition)// Im Uhrzeigersinn                              
                MyRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;

            if (SoundOffsetAngle > SoundOffsetAngleSavedPosition)// gegen UhrzeigerSinn
            {
                float floatabove180 = SoundOffsetAngleSavedPosition - SoundOffsetAngle;

                if (floatabove180 > 178) //Sobald der Character eine umdrehung gemacht hat ist Z von -180 plötzlich +180 und verfälscht deswegen hier die verbesserung das es funktioniert wie im uhrzeigersinn unten
                {
                    MyRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
                }
                if (floatabove180 < 180)
                {
                    MyRigidbody.velocity = new Vector3(Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
                }
            }
            //MyRigidbody.velocity = new Vector3(Mathf.Cos(SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
        }
        IsOnSoundMill = false;
    }

    //////
    ///
    ///

    //Aufgeräumt

    //Vector2 RotationSpeed = new Vector2();
    //Vector2 savedpositiontarget;
    //float savedanglefloat;
    //Vector2 savedanglevec;
    //float savedpositiony;

    //float dirX;
    //float dirY;

    //Rigidbody2D HookRigid;
    //BoxCollider2D HookCollide;
    //*/
    ///*
    ////Player
    //public Transform grabDetect;
    //public Transform boxHolder; //bzw ich halte mich an dem objekt fest
    //public float rayDist;
    //Rigidbody2D MyRigidbody; //So wie es auschaut kann ich hier das ganze einfügen
    //GameObject PlayerObj;
    //public Transform Player;
    //public static Vector3 PlayerVector;
    //public float MoveSpeed = 5;
    //float xVelocity;

    ////Jump
    //bool Jumping = false;
    //float JumpTimer = 0;
    //public float Jumpheight = 40;
    //public bool Grounded = false;

    ////Crouch
    //CapsuleCollider2D CollideCapsule;
    //public Sprite[] Crouching;
    //public Sprite[] Standing;
    //public bool Crouch;

    ////HookGrab
    //public Transform HookGrab;
    //static bool direction = false;
    //static bool GraHoTrigger = false;
    //private Transform Target;
    //bool HookDetect = false;
    //public float FixeScale = 1;
    //bool hookup = false;
    //public float hookupheight = 1.1f;
    //public Transform PLPO;
    //public Transform HookVIS;

    ////Soundmill
    //float Rotationdirection;
    //Transform SoundmillOffset; //ist nicht zugweisene
    //float SoundOffsetAngle;
    //float SoundOffsetAngleSavedPosition;
    //public float Force;
    //Vector2 SoundmillOffsetVector;
    ////Rotation
    //Quaternion rotationPlayer; //idfk what that is
    //Quaternion rotationPLPO;
    //Quaternion rotationHookGrab;
    //bool IsOnSoundMill = false;

    ////CableCar
    //bool IsOnCableCar = false;
    //Transform soundmill;
    //private void Awake()
    //{
    //    rotationPLPO = PLPO.transform.rotation;
    //    rotationPlayer = transform.rotation; //Damit verändert sich meine Rotation (i hope) nicht
    //    rotationHookGrab = HookGrab.transform.rotation;
    //    //HookVIS.rotation = transform.rotation; FALSCH

    //}
    //void LateUpdate()
    //{
    //    transform.rotation = rotationPlayer;
    //    PLPO.transform.rotation = rotationPLPO;
    //    HookGrab.transform.rotation = rotationHookGrab;
    //    //transform.rotation = HookVIS.rotation; FALSCH

    //}

    //void Start()
    //{
    //    MyRigidbody = GetComponent<Rigidbody2D>();
    //    CollideCapsule = GetComponent<CapsuleCollider2D>();
    //    // HookCollide = GetComponent<BoxCollider2D>();
    //    //  HookRigid = GetComponent<Rigidbody2D>();
    //}
    //public void GrabHook()
    //{
    //    //just checking
    //    if (direction == false && IsOnSoundMill == false)
    //    {
    //        HookGrab.transform.localPosition = new Vector2(0.7f, hookupheight); //right up
    //    }
    //    if (direction == true && IsOnSoundMill == false)
    //    {
    //        HookGrab.transform.localPosition = new Vector2(-0.7f, hookupheight); //left up   
    //    }

    //    RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.right * transform.localScale, rayDist); //kann nur nach rechts schauen


    //    if (grabCheck.collider != null && grabCheck.collider.tag == "Pin")
    //    {
    //        //standart Kram
    //        IsOnSoundMill = true;
    //        MyRigidbody.gravityScale = 0;
    //        xVelocity = 0;
    //        MyRigidbody.velocity = new Vector2(0, 0);
    //        hookup = true;
    //        //Entparent
    //        PLPO.transform.parent = null; //Nicht mehr Child des Hooks
    //        HookGrab.transform.parent = null; //nicht mehr Child des Players
    //        //Parenten
    //        HookGrab.transform.parent = PLPO.transform; //Child des PLPO          
    //        Player.transform.parent = HookGrab.transform; //Child des Hooks                  
    //        //Player.transform.parent = PLPO.transform; //Child des PLPO FUNKTIONIERENDE ALTERNATIVE = DENK DRAN ROTATION ETC

    //        //Parent = PIN
    //        Debug.Log("Ich erkenne etwas");
    //        Target = grabCheck.collider.gameObject.GetComponent<Transform>();
    //        PLPO.transform.parent = Target.transform;
    //        //Position
    //        PLPO.transform.localPosition = new Vector2(0, 0);
    //        HookGrab.transform.localPosition = new Vector3(0, -0.4f, 0);
    //        Player.transform.localPosition = new Vector2(-0.7f, -hookupheight);

    //        //Ursprungspunkt
    //        SoundmillOffset = Player.transform.root.GetComponent<Transform>();
    //        //RotationSpeed = Player.parent.GetComponent("SoundMill"); Später herausfinden wie das funktioiert
    //        SoundmillOffsetVector = SoundmillOffset.transform.position;
    //        Debug.Log(SoundmillOffsetVector);
    //        HookDetect = true;
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.tag == "CableCar" && IsOnSoundMill == false) //Sobald auf einer Soundmill/Grounded/oder E gedrückt wird = Ignore CableCar
    //    {
    //        if (IsOnSoundMill == true || Grounded == true || Input.GetKey(KeyCode.E))
    //        {
    //            collision = GetComponent<Collider2D>();
    //            Physics2D.IgnoreCollision(collision, Player.GetComponent<Collider2D>());
    //            Debug.Log("IgnoredCollision");
    //        }
    //        else
    //        {
    //            Player.transform.parent = collision.transform; //Child des PLPO 
    //            MyRigidbody.gravityScale = 0;
    //            MyRigidbody.velocity = new Vector2(0, 0);
    //            Player.transform.localPosition = new Vector2(0, 0);
    //            IsOnCableCar = true;
    //        }
    //    }
    //}
    //// Update is called once per frame

    //void FixedUpdate() //hier wird die Physik immer geupdatet.
    //{
    //    float xInput = Input.GetAxisRaw("Horizontal");
    //    //hier kommt die Steuerung, ich bewege die X achse         
    //    //das Smoothing entweder über Input Manager (Sensitivity) oder Code:
    //    float VelocityDelta = 50;
    //    if (!Grounded)
    //        VelocityDelta /= 10;
    //    if (IsOnSoundMill == false) //nicht auf den Soundmills möglich !
    //        xVelocity = Mathf.MoveTowards(MyRigidbody.velocity.x, xInput * MoveSpeed, VelocityDelta * Time.deltaTime); //Das macht das Smoothing (xVelocity = berechnung zwischen velocity und der xInput(+/-)*Time)

    //    //HookGrab
    //    if (xVelocity < 1f && Input.GetKey(KeyCode.A) && IsOnSoundMill == false)
    //    {
    //        HookGrab.transform.localPosition = new Vector2(-0.7f, 0); //left
    //        direction = true;
    //    }
    //    else if (xVelocity > 1f && IsOnSoundMill == false)
    //    {
    //        HookGrab.transform.localPosition = new Vector2(0.7f, 0); //right
    //        direction = false;
    //    }
    //    else if (xVelocity == 0 && IsOnSoundMill == false)
    //    {
    //        if (direction == false)
    //            HookGrab.transform.localPosition = new Vector2(0.7f, 0); //right
    //        if (direction == true)
    //            HookGrab.transform.localPosition = new Vector2(-0.7f, 0); //left
    //    }

    //    MyRigidbody.velocity = new Vector2(xVelocity, MyRigidbody.velocity.y); //Das hier ist erstmal nur die Velocity im allg wird nicht auf das Objekt gemacht

    //    //Grounded
    //    Grounded = Physics2D.OverlapBox(MyRigidbody.position + Vector2.up * -0.5f, transform.localScale * 0.95f, 0, LayerMask.GetMask("Level")/*, ~LayerMask.GetMask("bsp")*/);

    //    //Jumping
    //    if (Input.GetKey(KeyCode.Space) && Jumping == false && Grounded == true)
    //    {
    //        Jumping = true;
    //        JumpTimer = 0;
    //    }
    //    if (Input.GetKey(KeyCode.Space) && Jumping == true)
    //    {
    //        JumpTimer += Time.deltaTime;
    //        if (JumpTimer > 0.1f) //Stop Jump after some time
    //            Jumping = false;
    //        else
    //            MyRigidbody.AddForce(new Vector2(0, Jumpheight));
    //    }

    //    //cablecarjump
    //    if (Input.GetKey(KeyCode.Space) && IsOnCableCar == true)
    //    {
    //        IsOnCableCar = false;
    //        Player.transform.parent = null; // collision.transform.SetParent(null);               
    //        HookDetect = false;
    //        Jumping = true;
    //        MyRigidbody.gravityScale = 1;
    //        MyRigidbody.velocity = new Vector3(0, Jumpheight / 5);
    //    }

    //    //Soundmilljump
    //    if (Input.GetKey(KeyCode.Space) && IsOnSoundMill == true && !Input.GetKey(KeyCode.E))
    //        SoundmillJump();

    //    //crouch
    //    if (Input.GetKey(KeyCode.LeftShift))
    //    {
    //        CollideCapsule.size = new Vector2(CollideCapsule.size.x, 0.7f);
    //        CollideCapsule.offset = new Vector2(CollideCapsule.offset.x, -0.15f);
    //        Crouch = true;
    //    }
    //    else
    //    {
    //        CollideCapsule.size = new Vector2(CollideCapsule.size.x, 1f);
    //        CollideCapsule.offset = new Vector2(CollideCapsule.offset.x, 0f);
    //        Crouch = false;
    //    }

    //    //Soundmillgrab
    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        GrabHook();
    //    }

    //    if (IsOnSoundMill == true)
    //        SoundOffsetAngleSavedPosition = SoundmillOffset.transform.rotation.z;

    //}

    //void SoundmillJump()
    //{
    //    //Debug.Log(CurrentRotation);
    //    Debug.Log(SoundOffsetAngle);
    //    //rotation
    //    SoundOffsetAngle = SoundmillOffset.transform.rotation.z;
    //    //VERSTOßEN
    //    Player.transform.parent = null;
    //    PLPO.transform.parent = null; //Nicht mehr Child des Hooks
    //    HookGrab.transform.parent = null; //nicht mehr Child des Players
    //    //PARENTEN
    //    HookGrab.transform.parent = Player.transform; //Hook = Child des Player
    //    PLPO.transform.parent = HookGrab.transform;   //PLPO = Child des Hooks   
    //    HookGrab.transform.localPosition = new Vector3(-0.7f, 0, 0);
    //    PLPO.transform.localPosition = new Vector3(0, 0.4f, 0);

    //    Jumping = true;
    //    hookup = false;
    //    HookDetect = false;
    //    MyRigidbody.gravityScale = 1;
    //    //transform.position += new Vector3(dirX, dirY) * Force;
    //    // MyRigidbody.velocity = new Vector3(-Mathf.Cos(SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
    //    if (SoundOffsetAngle < 0) //oben
    //    {
    //        Debug.Log("Oben");
    //        if (SoundOffsetAngle < SoundOffsetAngleSavedPosition)// Im Uhrzeigersinn
    //        {
    //            MyRigidbody.velocity = new Vector3(Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
    //        }
    //        if (SoundOffsetAngle > SoundOffsetAngleSavedPosition)//gegen UhrzeigerSinn
    //            MyRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
    //    }

    //    if (SoundOffsetAngle > 0) //unten
    //    {
    //        Debug.Log("Unten");

    //        if (SoundOffsetAngle < SoundOffsetAngleSavedPosition)// Im Uhrzeigersinn                              
    //            MyRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;

    //        if (SoundOffsetAngle > SoundOffsetAngleSavedPosition)// gegen UhrzeigerSinn
    //        {
    //            float floatabove180 = SoundOffsetAngleSavedPosition - SoundOffsetAngle;

    //            if (floatabove180 > 178) //Sobald der Character eine umdrehung gemacht hat ist Z von -180 plötzlich +180 und verfälscht deswegen hier die verbesserung das es funktioniert wie im uhrzeigersinn unten
    //            {
    //                MyRigidbody.velocity = new Vector3(-Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
    //            }
    //            if (floatabove180 < 180)
    //            {
    //                MyRigidbody.velocity = new Vector3(Mathf.Cos(-SoundOffsetAngle), Mathf.Sin(SoundOffsetAngle)) * Force;
    //            }
    //        }
    //    }
    //    IsOnSoundMill = false;
    //}
}
