using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DamageNumbersPro
{
    public class DNP_DemoManager : MonoBehaviour
    {
        public static DNP_DemoManager instance;

        List<DamageNumber> numberPrefabs;
        Text currentText;
        int currentIndex;
        float lastSpawnTime;

        void Awake()
        {
            instance = this;

            numberPrefabs = new List<DamageNumber>();
            Transform numbers = transform.Find("Numbers");
            for (int n = 0; n < numbers.childCount; n++)
            {
                numberPrefabs.Add(numbers.GetChild(n).GetComponent<DamageNumber>());
            }

            numbers.gameObject.SetActive(false);

            currentText = transform.Find("Canvas/Current").GetComponent<Text>();

            currentIndex = 0;
            UpdateCurrent();
        }

        private void Update()
        {
            //Spawning:
            if (Input.GetMouseButtonDown(0))
            {
                SpawnNumber();
            }
            if (Input.GetMouseButton(1) && Time.time > lastSpawnTime + 0.07f)
            {
                SpawnNumber();
                lastSpawnTime = Time.time;
            }

            //Scrolling:
            if (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.E))
            {
                currentIndex++;
                if (currentIndex > numberPrefabs.Count - 1)
                {
                    currentIndex = 0;
                }
            }
            if (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.Q))
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = numberPrefabs.Count - 1;
                }
            }
        }

        private void FixedUpdate()
        {
            UpdateCurrent();
        }

        void SpawnNumber()
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            position.z = 0;

            RaycastHit hit3D;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit3D, 500);

            if (hit3D.collider != null)
            {
                position = hit3D.point;
            }
            else
            {
                if (Camera.main.GetComponent<DNP_DemoCamera>() != null) return; //Only spawn numbers on ray hits.
            }

            float number = 1 + Mathf.Pow(Random.value, 2.5f) * 200;

            if (numberPrefabs[currentIndex].name == "Gold")
            {
                number = 1;
            }

            if (numberPrefabs[currentIndex].name == "Big Numbers")
            {
                number *= 800000 * Random.value;
            }

            DamageNumber dn = numberPrefabs[currentIndex].CreateNew(Mathf.RoundToInt(number), position); //Creating a new Damage Number from Prefab.

            if (numberPrefabs[currentIndex].name == "Text")
            {
                float random = Random.value;
                if(random < 0.33f)
                {
                    dn.prefix = "Wow";
                }else if(random < 0.66f)
                {
                    dn.prefix = "Nice";
                }else
                {
                    dn.prefix = "Great";
                }
            }

            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 0.1f);
            if (hit.collider != null || hit3D.collider != null)
            {
                DNP_Player target = null;
                Transform targetTransform = null;

                if(hit.collider != null)
                {
                    target = hit.collider.GetComponent<DNP_Player>();

                    if(target != null)
                    {
                        targetTransform = hit.collider.transform;
                    }
                }
                else
                {
                    if(hit3D.collider != null && hit3D.collider.GetComponent<DNP_SineMover>() != null)
                    {
                        targetTransform = hit3D.collider.transform;
                        dn.followDrag = 0;
                        dn.followSpeed = 10;
                    }
                }
                
                if(targetTransform != null)
                {
                    if (target != null && Input.GetMouseButtonDown(0) && numberPrefabs[currentIndex].name != "Text" && numberPrefabs[currentIndex].name != "Gold" && numberPrefabs[currentIndex].name != "Health" && numberPrefabs[currentIndex].name != "Experience")
                    {
                        target.Hurt();
                    }

                    dn.followedTarget = targetTransform;
                    dn.combinationSettings.combinationGroup += targetTransform.GetInstanceID();
                }
            }

            if (numberPrefabs[currentIndex].name == "Outline")
            {
                dn.GetReferences();
                dn.GetTextA().color = dn.GetTextB().color = Color.HSVToRGB(Random.value, 1f, 1f);
            }

            if (numberPrefabs[currentIndex].name == "Shadow")
            {
                dn.GetReferences();
                dn.GetTextA().color = dn.GetTextB().color = Color.HSVToRGB(Random.value, 0.5f, 0.9f);
            }
        }

        public DamageNumber GetNumber(string damageNumber)
        {
            foreach(DamageNumber dn in numberPrefabs)
            {
                if(dn.name == damageNumber)
                {
                    return dn;
                }
            }

            return null;
        }

        void UpdateCurrent()
        {
            string inputColor = "<color=#" + ColorUtility.ToHtmlStringRGBA(Color.Lerp(Color.white, Color.red, Mathf.Sin(Time.time * 4f) * 0.05f + 0.1f)) + ">";

            currentText.text = "<color=#FFFFFF99>Current:</color> " + numberPrefabs[currentIndex].name + System.Environment.NewLine + System.Environment.NewLine + inputColor + "Scroll</color> <color=#FFFFFFaa>to change the current number.</color>" + System.Environment.NewLine + inputColor + "Mouse Click</color> <color=#FFFFFFaa>to spawn numbers.</color>";
        }
    }

}