using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageNumbersPro
{
    public class DNP_Gold : MonoBehaviour
    {
        bool dead;

        Vector3 startPos;
        float speed;

        void Start()
        {
            dead = false;

            speed = 0.9f + 0.2f * Random.value;
            startPos = transform.position;
        }

        private void Update()
        {
            transform.position = startPos + Mathf.Sin(Time.time * 2f * speed) * Vector3.up * 0.5f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!dead && collision.name == "Player")
            {
                dead = true;
                DNP_DemoManager.instance.GetNumber("Gold").CreateNew(1,transform.position);

                Destroy(gameObject);
            }
        }
    }
}
