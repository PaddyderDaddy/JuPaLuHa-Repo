using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCars : MonoBehaviour
{
    Transform soundmill;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(transform);
        //soundmill = gameObject.GetComponentInParent<Transform>();       
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    Debug.Log(soundmill.transform.position);
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(null);
    }
}
