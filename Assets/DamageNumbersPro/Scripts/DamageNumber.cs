using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DamageNumbersPro
{
    public class DamageNumber : MonoBehaviour
    {
        //Lifetime:
        [Tooltip("The lifetime after which this fades out.")]
        public float lifetime = 2f;

        //Number:
        public bool enableNumber = true;
        [Tooltip("The number displayed in the text.\nCan be disabled if you only need text.")]
        public float number = 1;
        public TextSettings numberSettings = new TextSettings(0);
        public DigitSettings digitSettings = new DigitSettings(0);

        //Prefix:
        public bool enablePrefix = true;
        [Tooltip("Text displayed to the left of the number.")]
        public string prefix = "";
        public TextSettings prefixSettings = new TextSettings(0.2f);

        //Suffix:
        public bool enableSuffix = true;
        [Tooltip("Text displayed to the right of the number.")]
        public string suffix = "";
        public TextSettings suffixSettings = new TextSettings(0.2f);

        //Fading:
        public bool enableFading = true;
        public FadeSettings fadeIn = new FadeSettings(new Vector2(2,2));
        public FadeSettings fadeOut = new FadeSettings(new Vector2(1, 1));

        //Moving:
        public bool enableMovement = true;
        [Tooltip("The type of movement.")]
        public MoveType moveType = MoveType.LERP;
        public LerpSettings lerpSettings = new LerpSettings(0);
        public VelocitySettings velocitySettings = new VelocitySettings(0);

        //Start Rotation:
        public bool enableStartRotation = true;
        [Tooltip("Minimum start rotation.")]
        public float minRotation = -2f;
        [Tooltip("Maximum start rotation.")]
        public float maxRotation = 2f;

        //Shaking:
        public bool enableShaking = true;
        [Tooltip("Shake settings during idle.")]
        public ShakeSettings idleShake = new ShakeSettings(new Vector2(0.005f,0.005f));
        [Tooltip("Shake settings while fading in.\nCan be used to add motion while fading.")]
        public ShakeSettings fadeInShake = new ShakeSettings(Vector2.zero);
        [Tooltip("Shake settings while fading out.\nCan be used to add motion while fading.")]
        public ShakeSettings fadeOutShake = new ShakeSettings(Vector2.zero);

        //Combination:
        public bool enableCombination = false;
        public CombinationSettings combinationSettings = new CombinationSettings(0f);

        //Following:
        public bool enableFollowing = false;
        [Tooltip("Transform that will be followed.\nTries to maintain the position relative to the target.")]
        public Transform followedTarget;
        [Tooltip("Speed at which target is followed.")]
        public float followSpeed = 10f;
        public float followDrag = 0f;

        //Perspective Camera:
        public bool enablePerspective = false;
        [Tooltip("Ignores orthographic cameras.")]
        public bool ignoreOrthographic = true;
        [Tooltip("Keeps the numbers size consistent accross different distances.")]
        public bool consistentScale = false;
        [Tooltip("Moves the number close to the camera and scales it down.\nAlternatively you can use the 'Distance Field - Overlay' shader to render through walls.")]
        public bool renderThroughWalls = true;
        public PerspectiveSettings perspectiveSettings = new PerspectiveSettings(0);
        [Tooltip("Override the camera looked at and scaled for.\nIf this set to None the Main Camera will be used.")]
        public Transform cameraOverride;

        //Pooling:
        public bool enablePooling = false;
        [Tooltip("Maximum of damage numbers stored in pool.")]
        public int poolSize = 30;

        //References:
        TextMeshPro textA;
        TextMeshPro textB;

        //Fading:
        float currentFade;
        float startTime;
        float startLifeTime;
        float baseAlpha;
        float fadeInSpeed;
        float fadeOutSpeed;

        //Transform:
        public Vector3 position;

        //Scaling:
        Vector3 originalScale;
        Vector3 baseScale;
        Vector3 currentScale;

        //Movement:
        Vector2 remainingOffset;
        Vector2 currentVelocity;

        //Following:
        Vector3 lastTargetPosition;
        Vector3 targetOffset;
        float currentFollowSpeed;

        //Combination:
        static Dictionary<string, HashSet<DamageNumber>> combinationDictionary;
        DamageNumber myAbsorber;
        DamageNumber myTarget;
        bool removedFromDictionary;
        bool givenNumber;
        float absorbProgress;

        //Pooling:
        static Transform poolParent;
        static Dictionary<int, HashSet<DamageNumber>> pools;
        int poolingID;
        bool destroyOnStart;
        bool performRestart;

        //Original Settings:
        string originalCombinationGroup;
        string originalPrefix;
        string originalSuffix;
        Transform originalFollowedTarget;

        //Other:
        Vector3 up;
        Vector3 right;

        void Start()
        {
            //Once Only:
            GetReferences();

            //For Pool Prewarming:
            if (destroyOnStart)
            {
                destroyOnStart = false;
                DestroyNumber();
            }
            else
            {
                //Repeat for Pooling:
                Restart();
            }
        }

        void Update()
        {
            //For Pooling:
            if (performRestart)
            {
                Restart();
                performRestart = false;
            }

            if (enableMovement || enableShaking)
            {
                up = transform.up;
                right = transform.right;
            }

            if(enableFading)
            {
                HandleFading();
            }
            else
            {
                //Destroy when lifetime is over.
                if (!IsAlive())
                {
                    DestroyNumber();
                }
            }

            if (enableMovement)
            {
                HandleMovement();
            }

            if(enableCombination)
            {
                HandleCombination();
            }
        }

        void LateUpdate()
        {
            if(enableFollowing)
            {
                HandleFollowing();
            }

            if(!performRestart)
            {
                ApplyTransform();
            }
        }

        //Pool Related:
        void CopyDefaults(DamageNumber prefab)
        {
            originalCombinationGroup = prefab.combinationSettings.combinationGroup;
            originalPrefix = prefab.prefix;
            originalSuffix = prefab.suffix;
            originalFollowedTarget = prefab.followedTarget;
        }
        void PreparePooling()
        {
            //Add to Pool:
            pools[poolingID].Add(this);

            //Disable GameObject:
            gameObject.SetActive(false);

            //Queue Restart:
            performRestart = true;

            //Reset Runtime Variables:
            transform.localScale = baseScale = currentScale = originalScale;
            lastTargetPosition = targetOffset = Vector3.zero;

            //Clear Combination Targets:
            myTarget = myAbsorber = null;

            //Reset some Setting Variables:
            combinationSettings.combinationGroup = originalCombinationGroup;
            prefix = originalPrefix;
            suffix = originalSuffix;
            followedTarget = originalFollowedTarget;
           
        }
        void Restart()
        {
            //Called right after spawn:
            InitVariables();
            TryCombination();
        }

        /// <summary>
        /// Use this function on prefabs to spawn new damage numbers.
        /// Will clone this damage number.
        /// </summary>
        /// <returns></returns>
        public DamageNumber CreateNew(float newNumber, Vector3 newPosition)
        {
            DamageNumber newDN = default;
            int instanceID = GetInstanceID();

            //Check Pool:
            if (enablePooling && PoolAvailable(instanceID))
            {
                //Get from Pool:
                foreach(DamageNumber dn in pools[instanceID])
                {
                    newDN = dn;
                    break;
                }
                pools[instanceID].Remove(newDN);
            }
            else
            {
                //Create New:
                GameObject newGO = Instantiate<GameObject>(gameObject);
                newDN = newGO.GetComponent<DamageNumber>();

                if (enablePooling)
                {
                    newDN.CopyDefaults(this); //For Pooling Resets.
                }
            }

            newDN.number = newNumber; //Position
            newDN.transform.position = newPosition; //Position
            newDN.gameObject.SetActive(true); //Active Gameobject

            if (enablePooling)
            {
                newDN.SetPoolingID(instanceID);
            }

            return newDN;
        }

        /// <summary>
        /// Use this function to prewarm the pool at the start of your game.
        /// It will generate enough damage numbers to fill the pool size.
        /// </summary>
        public void PrewarmPool()
        {
            if (enablePooling)
            {
                int instanceId = GetInstanceID();

                //Initialize Dictionary:
                if(pools == null)
                {
                    pools = new Dictionary<int, HashSet<DamageNumber>>();
                }

                //Initialize Pool:
                if(!pools.ContainsKey(instanceId))
                {
                    pools.Add(instanceId, new HashSet<DamageNumber>());
                }

                //Fill Pool:
                HashSet<DamageNumber> pool = pools[instanceId];
                for(int n = 0; n < poolSize - pool.Count; n++)
                {
                    DamageNumber dn = CreateNew(123, Vector3.zero);
                    dn.destroyOnStart = true;
                    dn.GetReferences();
                    dn.SetFadeIn(0);
                }
            }
        }

        bool PoolAvailable(int id)
        {
            if(pools != null && pools.ContainsKey(id))
            {
                if(pools[id].Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetPoolingID(int id)
        {
            poolingID = id;

            //Initiate Dictionaries:
            if (pools == null)
            {
                pools = new Dictionary<int, HashSet<DamageNumber>>();
            }

            //Initiate Pool Parent:
            if(poolParent == null)
            {
                GameObject poolGameobject = new GameObject("Damage Number Pool");
                DontDestroyOnLoad(poolGameobject);

                poolParent = poolGameobject.transform;
                poolParent.localScale = Vector3.one;
                poolParent.eulerAngles = poolParent.position = Vector3.zero;
            }

            //Parent:
            transform.SetParent(poolParent, true);
        }

        void HandleFollowing()
        {
            if (followedTarget == null) {
                lastTargetPosition = Vector3.zero;
                return;
            }

            //Get Offset:
            if(lastTargetPosition != Vector3.zero)
            {
                targetOffset += followedTarget.position - lastTargetPosition;
            }
            lastTargetPosition = followedTarget.position;

            if (followDrag > 0 && currentFollowSpeed > 0)
            {
                currentFollowSpeed -= followDrag * Time.deltaTime;

                if(currentFollowSpeed < 0)
                {
                    currentFollowSpeed = 0;
                }
            }

            //Move to Target:
            Vector3 oldOffset = targetOffset;
            targetOffset = Vector3.Lerp(targetOffset, Vector3.zero, Time.deltaTime * followSpeed * currentFollowSpeed);
            position += oldOffset - targetOffset;
        }

        void HandleCombination()
        {
            if (Time.time - startTime < combinationSettings.delay) return;

            if(myAbsorber != null)
            {
                //Reset Lifetime:
                startLifeTime = Time.time;

                //Move:
                position = Vector3.Lerp(position, myAbsorber.position, Time.deltaTime * combinationSettings.targetSpeed);

                //Scale:
                baseScale = Vector3.Lerp(baseScale, Vector3.zero, Time.deltaTime * combinationSettings.targetScaleDownSpeed);

                //Fading:
                absorbProgress += Time.deltaTime;
                float normalizedProgress = absorbProgress / Mathf.Max(0.001f,combinationSettings.absorbTime);

                baseAlpha = Mathf.Min(baseAlpha, Mathf.Max(0,1 - normalizedProgress * combinationSettings.targetFadeOutSpeed));
                UpdateAlpha(currentFade);

                if (normalizedProgress >= 1)
                {
                    GiveNumber();
                    DestroyNumber();
                }
            }else if(myTarget != null)
            {
                //Move:
                position = Vector3.Lerp(position, myTarget.position, Time.deltaTime * combinationSettings.absorberSpeed);
            }
        }

        void HandleMovement()
        {
            if(moveType == MoveType.LERP)
            {
                //Lerp:
                Vector2 oldOffset = remainingOffset;
                remainingOffset = Vector2.Lerp(remainingOffset, Vector2.zero, Time.deltaTime * lerpSettings.speed);
                Vector2 deltaOffset = oldOffset - remainingOffset;

                position += up * deltaOffset.y + right * deltaOffset.x;
            }
            else
            {
                //Velocity:
                if (velocitySettings.dragX > 0)
                {
                    currentVelocity.x = Mathf.Lerp(currentVelocity.x, 0, Time.deltaTime * velocitySettings.dragX);
                }
                if (velocitySettings.dragY > 0)
                {
                    currentVelocity.y = Mathf.Lerp(currentVelocity.y, 0, Time.deltaTime * velocitySettings.dragY);
                }

                currentVelocity.y -= velocitySettings.gravity * Time.deltaTime;
                position += (up * currentVelocity.y + right * currentVelocity.x) * Time.deltaTime;
            }
        }

        void ApplyTransform()
        {
            //Position:
            Vector3 finalPosition = position;

            //Shaking:
            #region Shaking
            if (enableShaking)
            {
                Vector3 idleShakePosition = ApplyShake(finalPosition, idleShake);

                if (IsAlive())
                {
                    if(currentFade < 1 && fadeInShake.offset != Vector2.zero)
                    {
                        finalPosition = Vector3.Lerp(ApplyShake(finalPosition, fadeInShake), idleShakePosition, currentFade);
                    }
                    else
                    {
                        finalPosition = idleShakePosition;
                    }
                }
                else
                {
                    if (fadeOutShake.offset != Vector2.zero)
                    {
                        finalPosition = Vector3.Lerp(ApplyShake(finalPosition, fadeOutShake), idleShakePosition, currentFade);
                    }
                    else
                    {
                        finalPosition = idleShakePosition;
                    }
                }
            }
            #endregion

            //Scale Down from Combination:
            #region Combination
            if (enableCombination)
            {
                currentScale = Vector3.Lerp(currentScale, baseScale, Time.deltaTime * combinationSettings.absorberScaleFade);
            }
            #endregion

            Vector3 appliedScale = currentScale;

            //Perspective:
            #region Perspective
            if (enablePerspective)
            {
                Transform targetCamera = cameraOverride;
                if (targetCamera == null && Camera.main != null && (Camera.main.orthographic == false || ignoreOrthographic == false))
                {
                    targetCamera = Camera.main.transform;
                }
                if (targetCamera != null)
                {
                    transform.eulerAngles = targetCamera.eulerAngles;

                    Vector3 offset = default;
                    float distance = default;

                    if (consistentScale)
                    {
                        offset = finalPosition - targetCamera.position;
                        distance = offset.magnitude;

                        appliedScale *= distance / Mathf.Max(1, perspectiveSettings.baseDistance);

                        if (distance < perspectiveSettings.closeDistance)
                        {
                            appliedScale *= perspectiveSettings.closeScale;
                        }
                        else if (distance > perspectiveSettings.farDistance)
                        {
                            appliedScale *= perspectiveSettings.farScale;
                        }
                        else
                        {
                            appliedScale *= perspectiveSettings.farScale + (perspectiveSettings.closeScale - perspectiveSettings.farScale) * Mathf.Clamp01(1 - (distance - perspectiveSettings.closeScale) / Mathf.Max(0.01f, perspectiveSettings.farDistance - perspectiveSettings.closeScale));
                        }
                    }

                    if (renderThroughWalls)
                    {
                        float near = 0.3f;
                        if(Camera.main != null)
                        {
                            near = Camera.main.nearClipPlane;
                        }

                        //Move close to camera:
                        if (!consistentScale)
                        {
                            offset = finalPosition - targetCamera.position;
                            distance = offset.magnitude;
                        }
                        near += 0.0005f * distance + 0.02f + near * 0.02f * Vector3.Angle(offset, targetCamera.forward);

                        finalPosition = offset.normalized * near + targetCamera.position;

                        //Adjust Scale:
                        appliedScale *= near / distance;
                    }
                }
            }
            #endregion

            //Apply:
            transform.localScale = appliedScale;
            transform.position = finalPosition;
        }

        void TryCombination()
        {
            if (enableCombination == false || combinationSettings.combinationGroup == "") return; //No Combination

            myAbsorber = myTarget = null;
            removedFromDictionary = false;
            givenNumber = false;
            absorbProgress = 0;

            //Create Dictionary:
            if (combinationDictionary == null)
            {
                combinationDictionary = new Dictionary<string, HashSet<DamageNumber>>();
            }

            //Create HashSet:
            if (!combinationDictionary.ContainsKey(combinationSettings.combinationGroup))
            {
                combinationDictionary.Add(combinationSettings.combinationGroup, new HashSet<DamageNumber>());
            }

            //Add to HashSet:
            combinationDictionary[combinationSettings.combinationGroup].Add(this);

            //Combination:
            if (combinationSettings.absorberType == AbsorberType.OLDEST)
            {
                float oldestStartTime = Time.time + 0.5f;
                DamageNumber oldestNumber = null;

                foreach(DamageNumber otherNumber in combinationDictionary[combinationSettings.combinationGroup])
                {
                    if(otherNumber != this && otherNumber.myAbsorber == null && otherNumber.startTime < oldestStartTime)
                    {
                        if (Vector3.Distance(otherNumber.position, position) < combinationSettings.maxDistance)
                        {
                            oldestStartTime = otherNumber.startTime;
                            oldestNumber = otherNumber;
                        }
                    }
                }

                if (oldestNumber != null)
                {
                    GetAbsorbed(oldestNumber);
                }
            }
            else
            {
                foreach (DamageNumber otherNumber in combinationDictionary[combinationSettings.combinationGroup])
                {
                    if (otherNumber != this)
                    {
                        if (Vector3.Distance(otherNumber.position, position) < combinationSettings.maxDistance)
                        {
                            if(otherNumber.myAbsorber == null)
                            {
                                otherNumber.startTime = Time.time - 0.01f;
                            }

                            otherNumber.GetAbsorbed(this);
                        }
                    }
                }
            }
        }
        public void GetAbsorbed(DamageNumber otherNumber)
        {
            otherNumber.myTarget = this;
            myAbsorber = otherNumber;
            myAbsorber.startLifeTime = Time.time + combinationSettings.bonusLifetime;

            if (combinationSettings.instantGain)
            {
                GiveNumber();
            }
        }
        public void GiveNumber()
        {
            if (!givenNumber)
            {
                givenNumber = true;

                myAbsorber.number += number;
                myAbsorber.UpdateText();
                myAbsorber.currentScale = new Vector3(myAbsorber.baseScale.x * combinationSettings.absorberScaleGain.x, myAbsorber.baseScale.y * combinationSettings.absorberScaleGain.y, 1);
            }
        }

        Vector3 ApplyShake(Vector3 vector, ShakeSettings shakeSettings)
        {
            float currentTime = Time.time - startTime;

            if (shakeSettings.offset.y != 0)
            {
                vector += up * Mathf.Sin(shakeSettings.frequency * currentTime) * shakeSettings.offset.y;
            }

            if (shakeSettings.offset.x != 0)
            {
                vector += right * Mathf.Sin(shakeSettings.frequency * currentTime) * shakeSettings.offset.x;
            }

            return vector;
        }

        public void GetReferences()
        {
            baseAlpha = 0.9f;
            currentScale = baseScale = originalScale = transform.localScale;
            textA = transform.Find("TextA").GetComponent<TextMeshPro>();
            textB = transform.Find("TextB").GetComponent<TextMeshPro>();
        }
        public void InitVariables()
        {
            currentFollowSpeed = 1f;
            baseAlpha = 0.9f;
            startLifeTime = startTime = Time.time;
            position = transform.position;
            currentScale = baseScale = originalScale = transform.localScale;

            //Movement:
            if (enableMovement)
            {
                if (moveType == MoveType.LERP)
                {
                    remainingOffset = new Vector2(Random.Range(lerpSettings.minX, lerpSettings.maxX), Random.Range(lerpSettings.minY, lerpSettings.maxY));
                }
                else
                {
                    currentVelocity = new Vector2(Random.Range(velocitySettings.minX, velocitySettings.maxX), Random.Range(velocitySettings.minY, velocitySettings.maxY));
                }
            }

            //Start Rotation:
            if (enableStartRotation)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(minRotation, maxRotation));
            }

            //Start Faded:
            if (enableFading)
            {
                fadeInSpeed = 1f / Mathf.Max(0.0001f, fadeIn.fadeDuration);
                fadeOutSpeed = 1f / Mathf.Max(0.0001f, fadeOut.fadeDuration);

                currentFade = 0f;
                SetFadeIn(0);
            }

            lastTargetPosition = Vector3.zero;

            //Update Text:
            UpdateText();
        }
        public Material GetMaterial()
        {
            return textA.fontSharedMaterial;
        }
        public TextMeshPro GetTextA()
        {
            return textA;
        }
        public TextMeshPro GetTextB()
        {
            return textB;
        }

        #region Text
        public void UpdateText()
        {
            string numberText = "";
            if(enableNumber)
            {
                string numberString = default;
                bool shortened;

                if (digitSettings.decimals <= 0)
                {
                    numberString = ProcessIntegers(Mathf.RoundToInt(number).ToString(), out shortened);
                }
                else
                {
                    string allDigits = Mathf.RoundToInt(number * Mathf.Pow(10, digitSettings.decimals)).ToString();
                    int usedDecimals = digitSettings.decimals;

                    while (digitSettings.hideZeros && allDigits.EndsWith("0") && usedDecimals > 0)
                    {
                        allDigits = allDigits.Substring(0, allDigits.Length - 1);
                        usedDecimals--;
                    }

                    string integers = allDigits.Substring(0, Mathf.Max(0, allDigits.Length - usedDecimals));

                    integers = ProcessIntegers(integers, out shortened);

                    if(integers == "")
                    {
                        integers = "0";
                    }

                    string decimals = allDigits.Substring(allDigits.Length - usedDecimals);

                    if (usedDecimals > 0 && !shortened)
                    {
                        numberString = integers + digitSettings.decimalChar + decimals;
                    }
                    else
                    {
                        numberString = integers;
                    }
                }

                numberText = ApplyTextSettings(numberString, numberSettings);
            }

            string prefixText = "";
            if (enablePrefix)
            {
                prefixText = ApplyTextSettings(prefix, prefixSettings);
            }

            string suffixText = "";
            if (enableSuffix)
            {
                suffixText = ApplyTextSettings(suffix, suffixSettings);
            }

            if (textA == null) GetReferences();
            textA.text = textB.text = prefixText + numberText + suffixText;
        }
        string ProcessIntegers(string integers, out bool shortened)
        {
            shortened = false;

            //Short Suffix:
            if (digitSettings.suffixShorten)
            {
                int currentSuffix = -1;

                while(integers.Length > digitSettings.maxDigits && currentSuffix < digitSettings.suffixes.Count - 1 && integers.Length - digitSettings.suffixDigits[currentSuffix + 1] > 0)
                {
                    currentSuffix++;
                    integers = integers.Substring(0, integers.Length - digitSettings.suffixDigits[currentSuffix]);
                }

                if(currentSuffix >= 0)
                {
                    integers += digitSettings.suffixes[currentSuffix];
                    shortened = true;
                    return integers;
                }
            }

            //Dots:
            if (digitSettings.dotSeperation && digitSettings.dotDistance > 0)
            {
                char[] chars = integers.ToCharArray();
                integers = "";
                for (int n = chars.Length - 1; n > -1; n--)
                {
                    integers = chars[n] + integers;

                    if ((chars.Length - n) % digitSettings.dotDistance == 0 && n > 0)
                    {
                        integers = digitSettings.dotChar + integers;
                    }
                }
            }

            return integers;
        }
        string ApplyTextSettings(string text, TextSettings settings)
        {
            string newString = text;

            if(text == "")
            {
                return "";
            }

            //Formatting:
            if (settings.bold)
            {
                newString = "<b>" + newString + "</b>";
            }
            if (settings.italic)
            {
                newString = "<i>" + newString + "</i>";
            }
            if (settings.underline)
            {
                newString = "<u>" + newString + "</u>";
            }
            if (settings.strike)
            {
                newString = "<s>" + newString + "</s>";
            }

            //Custom Color:
            if (settings.customColor)
            {
                newString = "<color=#" + ColorUtility.ToHtmlStringRGBA(settings.color) + ">" + newString + "</color>";
            }

            if (settings.mark)
            {
                newString = "<mark=#" + ColorUtility.ToHtmlStringRGBA(settings.markColor) + ">" + newString + "</mark>";
            }

            if (settings.alpha < 1)
            {
                newString = "<alpha=#" + ColorUtility.ToHtmlStringRGBA(new Color(1, 1, 1, settings.alpha)).Substring(6) + ">" + newString + "<alpha=#FF>";
            }

            //Change Size:
            if (settings.size > 0)
            {
                newString = "<size=+" + settings.size.ToString().Replace(',', '.') + ">" + newString + "</size>";
            }
            else if (settings.size < 0)
            {
                newString = "<size=-" + Mathf.Abs(settings.size).ToString().Replace(',', '.') + ">" + newString + "</size>";
            }

            //Character Spacing:
            if (settings.characterSpacing != 0)
            {
                newString = "<cspace=" + settings.characterSpacing.ToString().Replace(',', '.') + ">" + newString + "</cspace>";
            }

            //Spacing:
            if (settings.horizontal > 0)
            {
                string space = "<space=" + settings.horizontal.ToString().Replace(',', '.') + "em>";
                newString = space + newString + space;
            }

            if(settings.vertical != 0)
            {
                newString = "<voffset=" + settings.vertical.ToString().Replace(',', '.') + "em>" + newString + "</voffset>";
            }

            if(settings.newLine)
            {
                newString += System.Environment.NewLine;
            }

            //Return:
            return newString;
        }
        #endregion

        public bool IsAlive()
        {
            return Time.time - startLifeTime < lifetime;
        }

        #region Fading
        void HandleFading()
        {
            if (IsAlive())
            {
                //Fading In:
                if(currentFade < 1)
                {
                    currentFade = Mathf.Min(1, currentFade + Time.deltaTime * fadeInSpeed);
                    SetFadeIn(currentFade);
                }
            }
            else
            {
                //Fading Out:
                if (currentFade > 0)
                {
                    currentFade = Mathf.Max(0, currentFade - Time.deltaTime * fadeOutSpeed);
                    SetFadeOut(currentFade);
                    RemoveFromCombination();

                    if (currentFade <= 0)
                    {
                        DestroyNumber();
                    }
                }
            }
        }
        public void SetFadeIn(float progress)
        {
            SetFade(progress, fadeIn);
        }
        public void SetFadeOut(float progress)
        {
            SetFade(progress, fadeOut);
        }
        public void SetFade(float progress, FadeSettings fadeSettings)
        {
            if (this == null) return; //Had to do this to fix a unity engine bug with editor fade preview.

            //Position Offset:
            Vector2 posOffset = fadeSettings.positionOffset * (progress - 1);
            textA.transform.localPosition = posOffset;
            textB.transform.localPosition = -posOffset;

            //Scale & Scale Offset:
            Vector2 scaleOffset = fadeSettings.scaleOffset;
            if (scaleOffset.x == 0) scaleOffset.x += 0.001f;
            if (scaleOffset.y == 0) scaleOffset.y += 0.001f;

            Vector3 scaleA = Vector2.Lerp(scaleOffset * fadeSettings.scale, Vector2.one, progress);
            scaleA.z = 1;
            Vector3 scaleB = Vector2.Lerp(new Vector3(1f / scaleOffset.x, 1f / scaleOffset.y, 1) * fadeSettings.scale, Vector2.one, progress);
            scaleB.z = 1;

            textA.transform.localScale = scaleA;
            textB.transform.localScale = scaleB;

            //Alpha:
            UpdateAlpha(progress);
        }
        public void UpdateAlpha(float progress)
        {
            textA.alpha = textB.alpha = Mathf.Clamp01(progress * progress * baseAlpha * baseAlpha);
        }
        #endregion

        public void DestroyNumber()
        {
            //Remove Absorber Reference:
            if (myAbsorber != null && myAbsorber.myTarget == this)
            {
                myAbsorber.myTarget = null;
            }

            //Pooling / Destroying:
            if (enablePooling)
            {
                if(!pools.ContainsKey(poolingID))
                {
                    pools.Add(poolingID, new HashSet<DamageNumber>());
                }

                RemoveFromCombination();

                if (pools[poolingID].Count < poolSize)
                {
                    PreparePooling();
                }
                else
                {
                    Destroy(gameObject); //Not enough pool space.
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            RemoveFromCombination();

            //Remove from Pool:
            if (enablePooling && pools != null)
            {
                if(pools.ContainsKey(poolingID))
                {
                    if(pools[poolingID].Contains(this))
                    {
                        pools[poolingID].Remove(this);
                    }
                }
            }
        }

        public void RemoveFromCombination()
        {
            if (!removedFromDictionary && enableCombination && combinationSettings.combinationGroup != "")
            {
                if (combinationDictionary != null && combinationDictionary.ContainsKey(combinationSettings.combinationGroup))
                {
                    if (combinationDictionary[combinationSettings.combinationGroup].Contains(this))
                    {
                        removedFromDictionary = true;
                        combinationDictionary[combinationSettings.combinationGroup].Remove(this);
                    }
                }
            }
        }
    }
}
