using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageNumbersPro
{
    public class DNP_DemoCamera : MonoBehaviour
    {
        Vector3 velocity;
        Vector2 angularVelocity;

        void Start()
        {
            velocity = Vector3.zero;
            angularVelocity = Vector2.zero;
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            //Looking:
            angularVelocity += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector2 old = angularVelocity;
            angularVelocity = Vector2.Lerp(angularVelocity, Vector2.zero, Time.deltaTime * 15f);
            Vector2 delta = old - angularVelocity;

            float xAngle = transform.eulerAngles.x;
            if (xAngle > 180) xAngle -= 360;
            xAngle -= delta.y;
            if (xAngle > 80) xAngle = 80;
            if (xAngle < -80) xAngle = -80;

            transform.eulerAngles = new Vector3(xAngle, transform.eulerAngles.y + delta.x, 0);


            //Velocity:
            Vector3 targetDirection = Vector3.zero;

            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                targetDirection += transform.forward;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                targetDirection -= transform.forward;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                targetDirection += transform.right;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                targetDirection -= transform.right;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                targetDirection += Vector3.up;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                targetDirection -= Vector3.up;
            }

            velocity = Vector3.Lerp(velocity, targetDirection * 12f, Time.deltaTime * 8f);
            transform.position += velocity * Time.deltaTime;
        }
    }
}

