using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUIContainer : MonoBehaviour
{
    public CharacterCreationManager manager;

    private bool inPlace;
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.CompareTag("Dragable"))
        {
            if (!Input.GetMouseButton(0) && !inPlace)
            {
                inPlace = true;
                other.transform.position = transform.position;
                manager.ManageObjectList(true,other.collider.gameObject.GetComponent<DraggableResource>().typeOfResource);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.CompareTag("Dragable"))
        {
            if (!Input.GetMouseButton(0))
            {
                other.collider.gameObject.GetComponent<DraggableResource>().ReturnToStartingPos();
                manager.ManageObjectList(false,other.collider.gameObject.GetComponent<DraggableResource>().typeOfResource);
                inPlace = true;
               
            }
        }
    }
}
