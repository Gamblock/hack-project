using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNP_SineMover : MonoBehaviour
{
    public Vector3 posA;
    public Vector3 posB;
    public float speed = 0.35f;

    void Update()
    {
        transform.position = Vector3.Lerp(posA, posB, Mathf.Sin(speed * Time.time) * 0.5f + 0.5f);
    }
}
