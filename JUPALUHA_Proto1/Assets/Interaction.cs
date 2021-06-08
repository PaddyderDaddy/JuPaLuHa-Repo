using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Detection Parameters")]
    public Transform detectionPoint;
    public float detectionRadius = 0.2f;
    public LayerMask detectionLayer;
    public GameObject detectedObject;

    public float rotZ;

   


    [Header("Others")]
    public List<GameObject> pickedItems = new List<GameObject>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPoint.position, detectionRadius);
    }

    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if (obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    public void PickUpItem(GameObject item)
    {
        pickedItems.Add(item);
    }

    public void ItemMove()
    {
        GameObject[] pipes;
        pipes = GameObject.FindGameObjectsWithTag("PipeAdjustable");
        foreach (GameObject pipe in pipes)
            pipe.transform.eulerAngles = new Vector3(0, 0, rotZ);
        
    }
    public void ItemMoveBack()
    {
        GameObject[] pipes;
        pipes = GameObject.FindGameObjectsWithTag("PipeAdjustable");
        foreach (GameObject pipe in pipes)
            pipe.transform.eulerAngles = new Vector3(0, 0, -rotZ);
        
    }
}


