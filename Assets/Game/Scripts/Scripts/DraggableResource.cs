using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class DraggableResource : MonoBehaviour
{
    private Vector3 mousePosition;
    public bool is3D;
    public TypeEnums.ResourceTypes typeOfResource;    

    private Vector3 startingPosition;
    private bool hoveringOverSlot;
    private bool followingCursor;
    private bool colliding;

    private void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (!Input.GetMouseButton(0) && !colliding && transform.position != startingPosition)
        {
            ReturnToStartingPos();
        }
        if (is3D)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.gameObject.CompareTag("Dragable"))
                {
                    Vector3 mousePosition = Mouse.current.position.ReadValue();
                    mousePosition.z = 10;
                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    transform.position = mousePosition;
                }
            }
        }
        else
        {
            if (GetEventSystemRaycastResults().Count > 0 &&
                GetEventSystemRaycastResults()[0].gameObject == gameObject && Input.GetMouseButton(0)  )
            {
                followingCursor = true;
            }
            else if(!Input.GetMouseButton(0))
            {
                followingCursor = false;

            }

        }
        if (followingCursor)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            transform.position = mousePosition;
        }
    }

    public void ReturnToStartingPos()
    {
        transform.position = startingPosition;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.CompareTag("UIContainer"))
        {
            colliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.CompareTag("UIContainer"))
        {
            colliding = false;
        }
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

   
}
