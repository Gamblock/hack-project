using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.Rendering;
using System;

namespace DamageNumbersPro
{
    [CustomEditor(typeof(DamageNumber)), CanEditMultipleObjects]
    public class DamageNumberEditor : Editor
    {
        //References:
        DamageNumber dn;
        TextMeshPro textA;
        TextMeshPro textB;
        Material mat;

        //Styles:
        GUIStyle style;

        //Preview:
        float currentFadeIn;
        float currentFadeOut;

        //Tips:
        bool spawnHelp;
        bool glowHelp;
        bool overlayHelp;

        //Help
        bool numberHelp;
        bool prefixHelp;
        bool suffixHelp;
        bool fadingHelp;
        bool movementHelp;
        bool shakingHelp;
        bool startRotationHelp;
        bool followingHelp;
        bool combinationHelp;
        bool perspectiveHelp;
        bool poolingHelp;


        //Repaint:
        public bool repaintViews;

        //Current:
        public static int currentEditor;

        //External Inspectors:
        MaterialEditor matEditor;
        Editor textEditor;

        void OnEnable()
        {
            dn = (DamageNumber)target;
            try
            {
                if(!Application.isPlaying || dn.GetTextA() == false) dn.GetReferences();
            }catch
            {
                return;
            }

            if (!Application.isPlaying)
            {
                currentFadeIn = currentFadeOut = 0;
                dn.SetFadeIn(1);
            }

            textA = dn.GetTextA();
            textB = dn.GetTextB();
        }

        public static void Prepare(GameObject go)
        {
            if(go.GetComponent<SortingGroup>() == null)
            {
                go.AddComponent<SortingGroup>().sortingOrder = 1000;
            }
            
            if(go.transform.Find("TextA") == null)
            {
                NewTextMesh("TextA", go.transform);
            }

            if (go.transform.Find("TextB") == null)
            {
                NewTextMesh("TextB", go.transform);
            }

            Undo.RegisterCreatedObjectUndo(go, "Create new Damage Number");
        }
        public static GameObject NewTextMesh(string tmName, Transform parent)
        {
            GameObject newTM = new GameObject();
            newTM.name = tmName;

            TextMeshPro tmp = newTM.AddComponent<TextMeshPro>();
            tmp.fontSize = 5;
            tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
            tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
            tmp.text = "1";

            newTM.transform.SetParent(parent,true);
            newTM.transform.localPosition = Vector3.zero;
            newTM.transform.localScale = Vector3.one;
            newTM.transform.localEulerAngles = Vector3.zero;

            return newTM;
        }

        [MenuItem("GameObject/2D Object/Damage Number",priority = -1)]
        public static void CreateDamageNumber()
        {
            GameObject newDN = new GameObject("Damage Number");
            newDN.AddComponent<DamageNumber>();
            Prepare(newDN);

            if(Camera.main != null)
            {
                newDN.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10;
            }
        }

        public override void OnInspectorGUI()
        {
            style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            if (repaintViews)
            {
                repaintViews = false;
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }

            //Top:
            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("<size=15><b>Damage Numbers Pro</b></size>", style);
            GUI.color = new Color(1, 1, 1, 0.7f);
            EditorGUILayout.LabelField("Variables show a <b>tooltip</b> when you hover over them.", style);
            EditorGUILayout.LabelField("Disabled features do not affect performance.", style);
            EditorGUILayout.Space();
            DisplayHints(ref spawnHelp, "How to spawn damage numbers.",
                "Save your damage number as a <b>prefab</b>.",
                "Spawn prefabs using <b>numberPrefab.CreateNew(number,position)</b>.",
                "You can find more information in the documentation.");
            DisplayHints(ref glowHelp, "How to create glowing damage numbers.",
                "First enable <b>HDR</b> and add <b>Bloom</b> post processing.",
                "There is a detailed tutorial in the documentation.",
                "Then increase the <b>Color Intensity</b> of the material.");
            DisplayHints(ref overlayHelp, "How to render damage numbers through walls.",
                "Change the material's shader to <b>'Distance Field Overlay'</b>.",
                "For 2D games look into the <b>'Sorting Group'</b> component below.");
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            if (textA == null || textB == null)
            {
                if (GUILayout.Button("Prepare"))
                {
                    Prepare(dn.gameObject);
                    OnEnable();
                }
                EditorGUILayout.LabelField("", style);
                EditorGUILayout.LabelField("Click the button above to prepare the gameobject.", style);
                EditorGUILayout.LabelField("Or use <b>[GameObject/2D Object/Damage Number]</b> to create a number.", style);
                return;
            }

            serializedObject.Update();

            //Warnings:
            if(dn.enablePooling && Application.isPlaying)
            {
                GUI.color = new Color(1, 0.9f, 0.5f);
                EditorGUILayout.BeginVertical("Helpbox");
                EditorGUILayout.LabelField("<b>Pooling</b> makes <b>tweaking</b> settings at <b>runtime</b> impossible.", style);
                EditorGUILayout.EndVertical();
                GUI.color = new Color(1, 1, 1);
            }

            EditorGUILayout.BeginVertical();

            //Properties:
            DisplayMainSettings();
            DisplayNumber();
            DisplayPrefix();
            DisplaySuffix();
            DisplayFading();
            DisplayMovement();
            DisplayShaking();
            DisplayStartRotation();
            DisplayCombination();
            DisplayFollowing();
            DisplayPerspective();
            DisplayPooling();

            EditorGUILayout.EndVertical();

            //Fix Variables:
            FixTextSettings(ref dn.numberSettings);
            FixTextSettings(ref dn.prefixSettings);
            FixTextSettings(ref dn.suffixSettings);
            FixFadeSettings(ref dn.fadeIn);
            FixFadeSettings(ref dn.fadeOut);
            MinZero(ref dn.digitSettings.decimals);
            MinZero(ref dn.digitSettings.dotDistance);

            //Apply Properties:
            serializedObject.ApplyModifiedProperties();

            //Update Text:
            if (Selection.gameObjects != null && Selection.gameObjects.Length > 1)
            {
                foreach (GameObject gameObject in Selection.gameObjects)
                {
                    if (gameObject != dn.gameObject)
                    {
                        DamageNumber other = gameObject.GetComponent<DamageNumber>();
                        if (other != null)
                        {
                            other.UpdateText();
                        }
                    }
                }
            }
            dn.UpdateText();

            //Preview
            FadePreview();

            //External Editors:
            ExternalEditors();
            DisplayFinalInformation();
        }

        #region Properties
        void DisplayMainSettings()
        {
            //Category:
            NewCategoryHorizontal("Main Settings", true);

            //Reset:
            if (ResetButton())
            {
                dn.lifetime = 2;
                RefreshText();
            }
            EditorGUILayout.EndHorizontal();

            if (dn.lifetime < 0) dn.lifetime = 0;

            //Properties:
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lifetime"));
        }
        void DisplayNumber()
        {
            //Category:
            NewCategoryHorizontal("Number", dn.enableNumber);

            //Reset:
            if (ResetButton())
            {
                dn.numberSettings = new TextSettings(0);
                dn.number = 1;
                dn.digitSettings = new DigitSettings(0);
            }

            //Help:
            HelpToggle(ref numberHelp);

            //Toggle:
            EasyToggle(ref dn.enableNumber);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableNumber)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("number"));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("numberSettings"));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("digitSettings"));
                EditorGUILayout.EndHorizontal();
            }

            //Help:
            if (numberHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- Displays a single <b>number</b>.", style);
                EditorGUILayout.LabelField("- Can be <b>disabled</b> if you only want text.", style);
                EditorGUILayout.LabelField("- Call <b>DamageNumber.UpdateText()</b> after runtime changes.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayPrefix()
        {
            //Category:
            NewCategoryHorizontal("Prefix", dn.enablePrefix);

            //Reset:
            if (ResetButton())
            {
                dn.prefix = "";
                dn.prefixSettings = new TextSettings(0.2f);
            }

            //Help:
            HelpToggle(ref prefixHelp);

            //Toggle:
            EasyToggle(ref dn.enablePrefix);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enablePrefix)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("prefix"));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("prefixSettings"));
                EditorGUILayout.EndHorizontal();
            }

            //Help:
            if (prefixHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- Displays text to the <b>left</b> of the number.", style);
                EditorGUILayout.LabelField("- Call <b>DamageNumber.UpdateText()</b> after runtime changes.", style);

                GUI.color = Color.white;
            }
        }
        void DisplaySuffix()
        {
            //Category:
            NewCategoryHorizontal("Suffix", dn.enableSuffix);

            //Reset:
            if (ResetButton())
            {
                dn.suffix = "";
                dn.suffixSettings = new TextSettings(0.2f);
            }

            //Help:
            HelpToggle(ref suffixHelp);

            //Toggle:
            EasyToggle(ref dn.enableSuffix);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableSuffix)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("suffix"));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("suffixSettings"));
                EditorGUILayout.EndHorizontal();
            }

            //Help:
            if (suffixHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- Displays text to the <b>right</b> of the number.", style);
                EditorGUILayout.LabelField("- Call <b>DamageNumber.UpdateText()</b> after runtime changes.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayFading()
        {
            //Category:
            NewCategoryHorizontal("Fading", dn.enableFading);

            //Reset:
            if (ResetButton())
            {
                dn.fadeIn = new FadeSettings(new Vector2(2, 2));
                dn.fadeOut = new FadeSettings(new Vector2(1, 1));
            }

            //Help:
            HelpToggle(ref fadingHelp);

            //Toggle:
            EasyToggle(ref dn.enableFading);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableFading)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeIn"));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeOut"));
                EditorGUILayout.EndHorizontal();
            }

            //Help:
            if (fadingHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- <b>Fades</b> the text in and out.", style);
                EditorGUILayout.LabelField("- <b>Postion Offset</b> moves the texts into opposite directions.", style);
                EditorGUILayout.LabelField("- <b>Scale Offset</b> scales up TextA and scales down TextB.", style);
                EditorGUILayout.LabelField("- <b>Scale</b> scales both texts.", style);
                EditorGUILayout.LabelField("- You can <b>preview</b> these using the sliders below.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayStartRotation()
        {
            //Category:
            NewCategoryHorizontal("Start Rotation", dn.enableStartRotation);

            //Reset:
            if (ResetButton())
            {
                dn.minRotation = -2;
                dn.maxRotation = 2;
            }

            //Help:
            HelpToggle(ref startRotationHelp);

            //Toggle:
            EasyToggle(ref dn.enableStartRotation);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableStartRotation)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minRotation"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxRotation"));
            }

            //Help:
            if (startRotationHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- Spawns with a random <b>rotation</b> between <b>" + dn.minRotation + "</b> and <b>" + dn.maxRotation +"</b>.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayMovement()
        {
            //Category:
            NewCategoryHorizontal("Movement", dn.enableMovement);

            //Reset:
            if (ResetButton())
            {
                dn.moveType = MoveType.LERP;
                dn.lerpSettings = new LerpSettings(0);
                dn.velocitySettings = new VelocitySettings(0);
            }

            //Help:
            HelpToggle(ref movementHelp);

            //Toggle:
            EasyToggle(ref dn.enableMovement);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableMovement)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveType"));
                if (dn.moveType == MoveType.LERP)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("lerpSettings"));
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("velocitySettings"));
                    EditorGUILayout.EndHorizontal();
                }
            }

            //Help:
            if (movementHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                if(dn.moveType == MoveType.LERP)
                {
                    EditorGUILayout.LabelField("- <b>Lerps</b> towards a random offset.", style);
                    EditorGUILayout.LabelField("- <b>Speed</b> can be adjusted.", style);
                }
                else
                {
                    EditorGUILayout.LabelField("- Spawns with a random <b>velocity</b>.", style);
                    EditorGUILayout.LabelField("- Velocity can be influenced by <b>drag</b> and <b>gravity</b>.", style);
                }

                GUI.color = Color.white;
            }
        }
        void DisplayShaking()
        {
            //Category:
            NewCategoryHorizontal("Shaking", dn.enableShaking);

            //Reset:
            if (ResetButton())
            {
                dn.idleShake = new ShakeSettings(new Vector2(0.005f, 0.005f));
                dn.fadeInShake = new ShakeSettings(new Vector2(0f, 0f));
                dn.fadeOutShake = new ShakeSettings(new Vector2(0f, 0f));
            }

            //Help:
            HelpToggle(ref shakingHelp);

            //Toggle:
            EasyToggle(ref dn.enableShaking);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableShaking)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("idleShake"));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeInShake"));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeOutShake"));
                EditorGUILayout.EndHorizontal();
            }

            //Help:
            if (shakingHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- <b>Shakes</b> back and forth between <b>-offset</b> and +<b>offset</b>.", style);
                EditorGUILayout.LabelField("- Uses <b>FadeInShake</b> and <b>FadeOutShake</b> while fading.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayCombination()
        {
            //Category:
            NewCategoryHorizontal("Combination", dn.enableCombination);

            //Reset:
            if (ResetButton())
            {
                dn.combinationSettings = new CombinationSettings(0f);
            }

            //Help:
            HelpToggle(ref combinationHelp);

            //Toggle:
            EasyToggle(ref dn.enableCombination);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableCombination)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("combinationSettings"));
                EditorGUILayout.EndHorizontal();
            }

            //Tip:
            if(dn.enableCombination && dn.combinationSettings.combinationGroup == "")
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- Please fill out the <b>Combination Group</b>.", style);
                EditorGUILayout.LabelField("- Otherwise numbers won't combine.", style);

                GUI.color = Color.white;
            }

            //Help:
            if (combinationHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- <b>Combines</b> numbers of the same <b>Combination Group</b>.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayFollowing()
        {
            //Category:
            NewCategoryHorizontal("Following", dn.enableFollowing);

            //Reset:
            if (ResetButton())
            {
                dn.followedTarget = null;
                dn.followSpeed = 10;
                dn.followDrag = 0;
            }

            //Help:
            HelpToggle(ref followingHelp);

            //Toggle:
            EasyToggle(ref dn.enableFollowing);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enableFollowing)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("followedTarget"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("followSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("followDrag"));
            }

            //Help:
            if (followingHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- <b>Follows</b> the <b>target</b> around.", style);
                EditorGUILayout.LabelField("- Will try to maintain it's position <b>relative</b> to the target.", style);
                EditorGUILayout.LabelField("- <b>Drag</b> fades out the following.", style);
                EditorGUILayout.LabelField("- Can be used to make damage numbers <b>track</b> their enemy.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayPerspective()
        {
            //Category:
            NewCategoryHorizontal("3D Camera", dn.enablePerspective);

            //Reset:
            if (ResetButton())
            {
                dn.cameraOverride = null;
                dn.consistentScale = true;
                dn.ignoreOrthographic = true;
                dn.renderThroughWalls = true;
                dn.perspectiveSettings = new PerspectiveSettings(0);
            }

            //Help:
            HelpToggle(ref perspectiveHelp);

            //Toggle:
            EasyToggle(ref dn.enablePerspective);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enablePerspective)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraOverride"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreOrthographic"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("consistentScale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("renderThroughWalls"));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("  ", GUILayout.Width(9));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("perspectiveSettings"));
                EditorGUILayout.EndHorizontal();
            }

            //Help:
            if (perspectiveHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- <b>Looks at</b> the <b>Main Camera</b> or (if set) <b>Override Camera</b>.", style);
                EditorGUILayout.LabelField("- Keeps the size consistent across different camera distances.", style);
                EditorGUILayout.LabelField("- Ignore this if you are creating a 2D game or use a orthographic camera.", style);

                GUI.color = Color.white;
            }
        }
        void DisplayPooling()
        {
            //Category:
            NewCategoryHorizontal("Pooling", dn.enablePooling);

            //Reset:
            if (ResetButton())
            {
                dn.poolSize = 30;
            }

            //Help:
            HelpToggle(ref poolingHelp);

            //Toggle:
            EasyToggle(ref dn.enablePooling);

            EditorGUILayout.EndHorizontal();

            //Properties:
            if (dn.enablePooling)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("poolSize"));
            }

            //Help:
            if (poolingHelp)
            {
                Lines();
                GUI.color = new Color(1, 1, 1, 0.7f);

                EditorGUILayout.LabelField("- Increases <b>spawn</b> performance at the cost of <b>ram</b>.", style);
                EditorGUILayout.LabelField("- Damage numbers will be <b>recycled</b> instead of destroyed.", style);
                EditorGUILayout.LabelField("- Use <b>numberPrefab.PrewarmPool()</b> once to fill the pool.", style);
                EditorGUILayout.LabelField("- Each damage number prefab has it's own pool.", style);
                EditorGUILayout.LabelField("", style);
                EditorGUILayout.LabelField("- If the pool runs out, new numbers will be created.", style);
                EditorGUILayout.LabelField("- Higher pool size results in more ram usage.", style);

                GUI.color = Color.white;
            }
        }
        #endregion

        void HelpToggle(ref bool helpVariable)
        {
            helpVariable = GUILayout.Toggle(helpVariable,"?", GUI.skin.button, GUILayout.Width(20));
        }

        void EasyToggle(ref bool toggleVariable)
        {
            GUI.color = new Color(1, 1, 1, toggleVariable ? 0.7f : 1f);

            bool oldToggle = toggleVariable;
            bool newToggle = GUILayout.Toggle(toggleVariable, toggleVariable ? "Enabled" : "Disabled", GUI.skin.button, GUILayout.Width(60));

            if(oldToggle != newToggle)
            {
                Undo.RecordObject(target, newToggle ? "Enable Setting" : "Disable Setting");
                toggleVariable = newToggle;
                RefreshText();
            }

            GUI.color = Color.white;
        }

        void RefreshText()
        {
            repaintViews = true;
            GUI.FocusControl("");
        }

        void FixTextSettings(ref TextSettings ts)
        {
            if (ts.horizontal < 0)
            {
                ts.horizontal = 0;
            }
        }
        void FixFadeSettings(ref FadeSettings fs)
        {
            if (fs.fadeDuration < 0)
            {
                fs.fadeDuration = 0;
            }
        }
        void MinZero(ref int variable)
        {
            if(variable < 0)
            {
                variable = 0;
            }
        }

        void NewCategory(string title)
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("<b>"+ title + ":</b>", style);
        }

        void NewCategoryHorizontal(string title, bool isActive)
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            GUI.color = new Color(1, 1, 1, isActive ? 1 : 0.5f);
            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<b>" + title + ":</b>", style);
        }

        void ExternalEditors()
        {
            int oldEditor = currentEditor;
            EditorGUILayout.LabelField("<b>Other Inspectors:</b>", style);
            currentEditor = GUILayout.Toolbar(currentEditor, new string[] { "None", "Material", "TextMeshPro" });
            Lines();
            if (currentEditor == 1)
            {
                if(matEditor == null || oldEditor != 1)
                {
                    mat = dn.GetMaterial();
                    if (matEditor != null)
                    {
                        DestroyImmediate(matEditor);
                    }
                    matEditor = (MaterialEditor)CreateEditor(mat);
                }

                if (matEditor != null)
                {
                    matEditor.DrawHeader();
                    matEditor.OnInspectorGUI();
                    Lines();
                }
            }
            else if (currentEditor == 2)
            {
                if (textEditor == null || oldEditor != 2)
                {
                    if (textEditor != null)
                    {
                        DestroyImmediate(textEditor);
                    }
                    textEditor = CreateEditor(textA);
                }

                if (textEditor != null)
                {
                    textEditor.DrawHeader();
                    textEditor.OnInspectorGUI();
                    EditorUtility.CopySerializedIfDifferent(textA, textB);


                    if(currentFadeIn == 0 && currentFadeOut == 0)
                    {
                        dn.UpdateAlpha(1);
                    }

                    textB.GetComponent<MeshRenderer>().material = textA.GetComponent<MeshRenderer>().material = textA.font.material;

                    Lines();
                }
            }
        }

        void Lines()
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            EditorGUILayout.LabelField("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            GUI.color = new Color(1, 1, 1, 1f);
        }

        void FadePreview()
        {
            EditorGUILayout.Space();
            Lines();
            EditorGUILayout.LabelField("<b>Preview:</b>", style);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Fade In");
            float lastFadeIn = currentFadeIn;
            currentFadeIn = GUILayout.HorizontalSlider(currentFadeIn, 0, 1);
            GUILayout.Label(Mathf.RoundToInt(currentFadeIn * 100f) + "%", GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();
            if (currentFadeIn != lastFadeIn || currentFadeIn > 0)
            {
                currentFadeOut = 0;
                dn.SetFadeIn(currentFadeIn);
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Fade Out");
            float lastFadeOut = currentFadeOut;
            currentFadeOut = GUILayout.HorizontalSlider(currentFadeOut, 0, 1);
            GUILayout.Label(Mathf.RoundToInt(currentFadeOut * 100f) + "%", GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();
            if (currentFadeOut != lastFadeOut || currentFadeOut > 0)
            {
                currentFadeIn = 0;
                dn.SetFadeOut(1 - currentFadeOut);
            }
            Lines();
        }

        void OnDisable()
        {
            if (!Application.isPlaying && textA != null)
            {
                currentFadeIn = currentFadeOut = 0;
                dn.SetFadeIn(1);
            }

            if (matEditor != null)
            {
                DestroyImmediate(matEditor);
            }
            if (textEditor != null)
            {
                DestroyImmediate(textEditor);
            }
        }

        bool ResetButton()
        {
            GUIContent resetButton = new GUIContent();
            resetButton.text = "R";
            resetButton.tooltip = "Resets the property.\n[CTRL + Z] to undo.";

            if (GUILayout.Button(resetButton, GUILayout.Width(20)))
            {
                Undo.RecordObject(target, "Reset a damage number's feature.");
                RefreshText();

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DisplayFinalInformation()
        {
            EditorGUILayout.Space();
            GUI.color = new Color(0.9f, 1, 0.9f, 1);
            EditorGUILayout.BeginVertical("Helpbox");

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;
            style.alignment = TextAnchor.MiddleLeft;

            GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
            linkStyle.richText = true;
            linkStyle.alignment = TextAnchor.MiddleLeft;
            linkStyle.normal.textColor = linkStyle.focused.textColor = linkStyle.hover.textColor = EditorGUIUtility.isProSkin ? new Color(0.8f, 0.9f, 1f, 1) : new Color(0.1f, 0.2f, 0.4f, 1);
            linkStyle.active.textColor = EditorGUIUtility.isProSkin ? new Color(0.6f, 0.8f, 1f, 1) : new Color(0.15f, 0.4f, 0.6f, 1);

            GUI.color = new Color(1, 1, 1f, 0.75f);
            EditorGUILayout.LabelField("<b>Contact me if you have any issues or questions.</b>", style);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(1, 1, 1, 1);
            EditorGUILayout.LabelField("<b>Email:</b>", style, GUILayout.Width(100));
            EditorGUILayout.SelectableLabel("<b>ekincantascontact@gmail.com</b>", linkStyle, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(1, 1, 1, 1);
            EditorGUILayout.LabelField("<b>Discord:</b>", style, GUILayout.Width(100));
            if (GUILayout.Button("<b><size=11>https://discord.gg/nWbRkN8Zxr</size></b>", linkStyle))
            {
                Application.OpenURL("https://discord.gg/nWbRkN8Zxr");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<b>Documentation:</b>", style, GUILayout.Width(100));
            if (GUILayout.Button("<b><size=11>https://ekincantas.com/damage-numbers-pro/</size></b>", linkStyle))
            {
                Application.OpenURL("https://ekincantas.com/damage-numbers-pro");
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = new Color(1, 1, 1, 0.75f);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<b>Hope you like my asset.</b>", style);
            EditorGUILayout.LabelField("<b>I appreciate kind reviews ^^</b>", style);
            EditorGUILayout.EndVertical();
            GUI.color = new Color(1, 1, 1, 1);
        }

        void DisplayHints(ref bool toggleVariable, string title, params string[] lines)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            GUIStyle button = new GUIStyle(GUI.skin.button);
            button.richText = true;

            if (toggleVariable)
            {
                GUI.color = new Color(1, 1, 1, 0.6f);
            }
            else
            {
                GUI.color = new Color(1, 1, 1, 0.3f);
            }

            title = "<b>" + title + "</b>";

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.white;
            EditorGUILayout.LabelField(title, style);
            if (GUILayout.Button("<size=10>" + (toggleVariable ? "▼" : "▲") + "</size>", button, GUILayout.Width(20)))
            {
                toggleVariable = !toggleVariable;
            }
            EditorGUILayout.EndHorizontal();

            if (toggleVariable == true)
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
                for (int l = 0; l < lines.Length; l++)
                {
                    if (lines[l] == " ")
                    {
                        EditorGUILayout.LabelField(lines[l], style, GUILayout.Height(6));
                    }
                    else
                    {
                        EditorGUILayout.LabelField(lines[l], style);
                    }
                }
            }

            GUI.color = Color.white;
            EditorGUILayout.EndVertical();
        }
    }
}
