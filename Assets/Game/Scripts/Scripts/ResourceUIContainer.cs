using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUIContainer : MonoBehaviour
{
    public CharacterCreationManager manager;
    public ResourceController resourceController;
    public int resourceCost = 10;

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
                resourceController.UpdateResourceValues(other.collider.gameObject.GetComponent<DraggableResource>().typeOfResource,resourceCost);
                other.collider.gameObject.GetComponent<DraggableResource>().enabled = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.CompareTag("Dragable"))
        {
            if (!Input.GetMouseButton(0))
            {
                manager.ManageObjectList(false,other.collider.gameObject.GetComponent<DraggableResource>().typeOfResource);
                inPlace = true;
               
            }
        }
    }
}
