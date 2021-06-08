using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    public GameObject SoundKillZone;
    public bool isOpen;

    public DeploySound GameManagerScript;

   
    public void MoveVent()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 1, 0);
        isOpen = true;
    }

    public void CloseVent()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
        isOpen = false;
       
    }

}
