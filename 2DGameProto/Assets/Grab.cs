using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    //private bool Hold;
    ////public KeyCode mouseButton;

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.F))
    //    {
    //        Hold = true;
    //    }
    //    else
    //    {
    //        Hold = false;
    //        Destroy(GetComponent<FixedJoint2D>());
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (Hold)
    //    {
    //        Debug.Log("successful!");
    //        Rigidbody2D rb = collision.transform.gameObject.GetComponent<Rigidbody2D>();
    //        if (rb != null)
    //        {
    //            Debug.Log("grabbing successful!");
    //            FixedJoint2D fj = transform.parent.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
    //            fj.connectedBody = rb;
    //        }
    //        //else
    //        //{
    //        //    FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
    //        //}
    //    }
    //}

    public Transform GrabPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            collision.collider.transform.SetParent(transform);

        //collision.collider.transform.position = GrabPoint.transform.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //collision.collider.transform.SetParent(null);
    }
}
