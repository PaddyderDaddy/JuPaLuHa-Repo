using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipsisMovement : MonoBehaviour
{
    //public Transform rotationCenter;

    //public float rotationRadius = 2f; 
    //public float angularSpeed = 2f;

    //float posX, posY, angle = 0f;

    //// Update is called once per frame
    //void Update()
    //{
    //    posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius * 1.5f;
    //    posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius / 2;
    //    transform.position = new Vector2(posX, posY);
    //    angle = angle + Time.deltaTime * angularSpeed;

    //    if (angle >= 360f)
    //        angle = 0f;
    //}

    //void Update() //Momentum jump
    //{
    //      dirX = SoundmillOffset.position.x + Mathf.Cos(SoundOffsetAngle)* rotationRadius;
    //      dirY = SoundmillOffset.position.y + Mathf.Sin(SoundOffsetAngle)* rotationRadius;

    //      if (Input.Get(KeyCode.Space))
    //          currentPos += Vector3(-dirX, dirY) * force;
    //}


    public Transform rotationCenter;

    public float angle;
    public float speed = 5f;

    public float rotationRadius = 10;

    public float tilt = 45f;

    public bool InMotion = true;

    void Update()
    {
        StopMotion();

        if (InMotion)
        {
            transform.position = new Vector2(rotationCenter.position.x + (rotationRadius * MCos(angle) * MCos(tilt)) - ((rotationRadius / 2) * MSin(angle) * MSin(tilt)),
                                             rotationCenter.position.y + (rotationRadius * MCos(angle) * MSin(tilt)) + ((rotationRadius / 2) * MSin(angle) * MCos(tilt)));
            angle += speed * Time.deltaTime;
            if (angle >= 360)
                angle = 0;
        }
    }

    void StopMotion()
    {
        if (Input.GetKeyDown(KeyCode.P))
            InMotion = !InMotion;
    }

    float MCos(float value)
    {
        return Mathf.Cos(Mathf.Deg2Rad * value);
    }

    float MSin(float value)
    {
        return Mathf.Sin(Mathf.Deg2Rad * value);
    }
}
