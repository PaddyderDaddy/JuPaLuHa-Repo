using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp, Examine, Grab, OpenVent }
    public InteractionType type;
    public int KeyPressed = 0;



    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 6;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            KeyPressed++;
        }

        if (KeyPressed >= 2)
            KeyPressed = 0;
    }

    public void Interact()
    {
        switch (type)
        {
            case InteractionType.PickUp:
                FindObjectOfType<Interaction>().PickUpItem(gameObject);
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                if (KeyPressed <= 0)
                {
                    FindObjectOfType<Interaction>().ItemMove();
                }
                else
                {
                    FindObjectOfType<Interaction>().ItemMoveBack();
                }
                break;
            case InteractionType.OpenVent:
                if (KeyPressed <= 0)
                {
                    FindObjectOfType<Vent>().MoveVent();
                }
                else
                {
                    FindObjectOfType<Vent>().CloseVent();
                }
                break;
            default:
                Debug.Log("NONE");
                break;

        }
    }
}
