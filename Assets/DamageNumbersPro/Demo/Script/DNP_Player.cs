using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageNumbersPro
{
    public class DNP_Player : MonoBehaviour
    {
        public static DNP_Player instance;

        [Header("Settings:")]
        public float speed = 5f;
        public float acceleration = 8f;
        public float jumpStrength = 5f;

        [Header("Sprite Sheets:")]
        public List<Sprite> idle;
        public List<Sprite> run;
        public List<Sprite> jump;
        public List<Sprite> land;
        public List<Sprite> hurt;

        //References:
        SpriteRenderer sprite;
        Rigidbody2D rig;
        CapsuleCollider2D cc;


        //Internal:
        List<Sprite> currentAnimation;
        bool isGrounded;
        float lastJumpTime;
        float lastAirTime;
        int currentIndex;
        float horizontal = 0f;
        float hurtTime;

        void Awake()
        {
            instance = this;

            sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            rig = GetComponent<Rigidbody2D>();
            cc = GetComponent<CapsuleCollider2D>();

            IncreaseIndex();
        }

        public CapsuleCollider2D GetCollider()
        {
            return cc;
        }

        void IncreaseIndex()
        {
            if (currentAnimation == idle || currentAnimation == jump)
            {
                Invoke("IncreaseIndex", 0.07f);
            }else if (currentAnimation == land || currentAnimation == hurt)
            {
                Invoke("IncreaseIndex", 0.09f);
            }
            else
            {
                Invoke("IncreaseIndex", 0.05f);
            }

            if (currentAnimation == null)
            {
                currentIndex = 0;
                return;
            }

            currentIndex++;

            if(currentIndex > currentAnimation.Count - 1)
            {
                if(currentAnimation == land)
                {
                    currentAnimation = idle;
                }

                if(currentAnimation == jump || currentAnimation == hurt)
                {
                    currentIndex = currentAnimation.Count - 1;
                }
                else
                {
                    currentIndex = 0;
                }
            }

            sprite.sprite = currentAnimation[currentIndex];
        }

        public void Hurt()
        {
            currentAnimation = hurt;
            hurtTime = Time.time + 0.3f;

            if(transform.position.x < -2f)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }else if(transform.position.x > 2)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            rig.velocity = -(Vector2)transform.right * 15f + Vector2.up * 6f;
        }

        void Update()
        {
            HandleMovement();
        }

        void FixedUpdate()
        {
            CheckGrounded();
        }

        void LateUpdate()
        {
            HandleAnimations();
        }

        void HandleMovement()
        {
            horizontal = 0f;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal -= 1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                horizontal += 1;
            }

            if(Time.time < hurtTime)
            {
                rig.velocity = Vector2.Lerp(rig.velocity, Vector2.zero, Time.deltaTime * 0.5f);
                return;
            }

            if (currentAnimation == land) horizontal = 0;

            Vector2 desiredSpeed = new Vector2(horizontal * speed, rig.velocity.y);
            rig.velocity = Vector2.Lerp(rig.velocity, desiredSpeed, Time.deltaTime * acceleration);

            if (horizontal > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (horizontal < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                if(Time.time > lastJumpTime + 0.2f && Time.time > lastAirTime + 0.1f)
                {
                    lastJumpTime = Time.time;
                    rig.velocity = new Vector2(rig.velocity.x, jumpStrength);

                    //Jump:
                    currentAnimation = jump;
                    currentIndex = 0;
                }
            }
        }

        void CheckGrounded(){
            Vector2 position = transform.position;

            gameObject.layer = 2; //Ignore Raycast
            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.down * cc.size.y * 0.49f, Vector2.down, 0.04f);
            gameObject.layer = 0;

            if(hit.collider != null)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
                lastAirTime = Time.time;
            }
        }

        void HandleAnimations()
        {
            if (Time.time < hurtTime) return;

            List<Sprite> newAnimation = null;

            if (isGrounded)
            {
                if(currentAnimation == jump && Time.time > lastJumpTime + 0.3f)
                {
                    newAnimation = land;
                }
                else if(currentAnimation == land)
                {
                    //Nothing:
                }
                else if(Time.time > lastJumpTime + 0.2f)
                {
                    if (horizontal == 0)
                    {
                        newAnimation = idle;
                    }
                    else
                    {
                        newAnimation = run;
                    }
                }
            }

            if(newAnimation != currentAnimation && newAnimation != null)
            {
                currentAnimation = newAnimation;
                currentIndex = 0;
            }
        }
    }

}