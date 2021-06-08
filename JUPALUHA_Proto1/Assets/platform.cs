using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    Transform soundmill;
    public CharControllerPhysics ControlScript;

    public float StartPositionX;
    public float StartPositionY;
    public float Amplitude;

    private void Update()
    {
        transform.position = new Vector3((Mathf.Sin(Time.time) * Amplitude) + StartPositionX, StartPositionY, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(transform);     
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ControlScript.Grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(null);
        ControlScript.Grounded = false;
    }
}
