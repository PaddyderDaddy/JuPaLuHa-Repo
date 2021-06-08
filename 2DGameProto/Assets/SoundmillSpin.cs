using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundmillSpin : MonoBehaviour
{
    public float RotationZ;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, RotationZ);
    }

    //public Transform rotationCenter;

    //public float angle;
    //public float speed = 5f;

    //public float rotationRadius = 10;

    //public float tilt = 45f;

    //public bool InMotion = true;

    //void Update()
    //{
    //    StopMotion();

    //    if (InMotion)
    //    {
    //        transform.position = new Vector2(rotationCenter.position.x + (rotationRadius * MCos(angle) * MCos(tilt)) - ((rotationRadius / 2) * MSin(angle) * MSin(tilt)),
    //                                         rotationCenter.position.y + (rotationRadius * MCos(angle) * MSin(tilt)) + ((rotationRadius / 2) * MSin(angle) * MCos(tilt)));
    //        angle += speed * Time.deltaTime;
    //        if (angle >= 360)
    //            angle = 0;
    //    }
    //}

    //void StopMotion()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //        InMotion = !InMotion;
    //}

    //float MCos(float value)
    //{
    //    return Mathf.Cos(Mathf.Deg2Rad * value);
    //}

    //float MSin(float value)
    //{
    //    return Mathf.Sin(Mathf.Deg2Rad * value);
    //}
}
