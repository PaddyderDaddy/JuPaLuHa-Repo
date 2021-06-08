using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    public float collisionRadius;
    public float DestroyTime;
    public bool TouchingObject = false;
    public GameObject spawnStartPoint;

    public enum PipeDirection { Left, Right }
    public PipeDirection type;
    public bool pipeUp;
    public bool onStart;

    void Start()
    {
       
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1, -1) * speed;
        TouchingObject = Physics2D.OverlapCircle(rb.transform.position, collisionRadius, LayerMask.GetMask("GrabbableObject"));

    }

    private void FixedUpdate()
    {
        Round();

        if(GameObject.Find("SoundOutput").transform.position.y >= 0)
        {
            pipeUp = true;
        }
        if(GameObject.Find("SoundOutput").transform.position.y < 0)
        {
            pipeUp = false;
        }

        if(GameObject.Find("SoundOutput").transform.position == gameObject.transform.position)
        {
            onStart = true;
        }
        else
        {
            onStart = false;
        }
    }

    void Update()
    {

        //DESTROY SOUND
        if (TouchingObject == true)
        {
            Destroy(this.gameObject, DestroyTime);
        }
        else
        {
            Destroy(this.gameObject, 4f);
        }

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    KeyPressed++;
        //}

        //if (KeyPressed >= 2)
        //    KeyPressed = 0;

       


    }

    public void Round()
    {
        if (onStart == true)
        {
            switch (type)
            {
                case PipeDirection.Left:
                    if (pipeUp == true)
                    {
                        rb = GetComponent<Rigidbody2D>();
                        rb.velocity = new Vector2(-1, 1) * speed;
                    }
                    else
                    {
                        rb = GetComponent<Rigidbody2D>();
                        rb.velocity = new Vector2(-1, 1) * speed;
                    }
                    break;
                case PipeDirection.Right:
                    if (pipeUp == true)
                    {
                        rb = GetComponent<Rigidbody2D>();
                        rb.velocity = new Vector2(1, 1) * speed;
                    }
                    else
                    {
                        rb = GetComponent<Rigidbody2D>();
                        rb.velocity = new Vector2(1, -1) * speed;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
