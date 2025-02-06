using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static VariableConditionEvent;

[CanEditMultipleObjects]
public class GameEventDataEditor : EditorWindow
{
    private static GameEventDataEditor window;

    private GameEventData gameEventdata = null;
    private ReorderableList dataList;
    private SerializedObject serializedObject;
    private Vector2 scrollBar = Vector2.zero;

    // function
    private Action updateCallBack;
    private Func<bool> ApplyModifiedCallBack;
    private Action doyoulist;

    [MenuItem("Window/Data/GameEventData")]
    private static void Init()
    {
        window = GetWindow<GameEventDataEditor>();

        window.titleContent = new GUIContent("Game Event Editor");
        window.minSize = new(600, 600);
        window.maxSize = new(600, 600);
    }

    private void Initialize(GameEventData gameEventdata)
    {
        serializedObject = new SerializedObject(gameEventdata);
        updateCallBack += serializedObject.Update;
        ApplyModifiedCallBack += serializedObject.ApplyModifiedProperties;

        dataList = new ReorderableList(serializedObject, serializedObject.FindProperty("gameEvent"), true, true, true, true);
        drawUI(dataList);        
    }

    private void drawUI(ReorderableList reorderableList)
    {
        if (reorderableList == null) return;

        doyoulist += reorderableList.DoLayoutList;
        
        reorderableList.drawElementCallback = (rect, index, active, focused) =>
        {
            var baseHeight = EditorGUIUtility.singleLineHeight;
            var baseHeightSpace = EditorGUIUtility.singleLineHeight + 4;

            float height = baseHeight;
            rect.y += 1;
            var obj = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.LabelField(new Rect(rect.x, rect.y, 45, baseHeight), "Event : ");
            EditorGUI.PropertyField(new Rect(rect.x + 45, rect.y, 160, baseHeight),
                obj.FindPropertyRelative("type"), GUIContent.none);
            switch ((GameEvent.Type)obj.FindPropertyRelative("type").intValue)
            {
                case GameEvent.Type.ScreenDialogueEvent:
                    rect.y += baseHeightSpace;
                    height += baseHeightSpace;

                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, 450, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("screenDialogueData"));
                    break;

                case GameEvent.Type.TalkDialogueEvent:
                    rect.y += baseHeightSpace;
                    height += baseHeightSpace;

                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, 450, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("talkDialogueData"));
                    break;

                case GameEvent.Type.ChoiceEvent:
                    SerializedProperty choiceObj = obj.FindPropertyRelative("choiceData");
                    if (GUI.Button(new Rect(rect.x + 210, rect.y, 40, baseHeight), "+"))
                    {
                        choiceObj.arraySize++;
                    }
                    if (GUI.Button(new Rect(rect.x + 250, rect.y, 40, baseHeight), "-"))
                    {
                        choiceObj.arraySize--;
                    }
                    rect.y += baseHeightSpace;

                    int i = 0;
                    foreach (SerializedProperty result in choiceObj)
                    {
                        height += baseHeightSpace;
                        EditorGUI.LabelField(new Rect(rect.x + 20, rect.y + i * baseHeightSpace, 100, baseHeight), "Choice Name");
                        result.FindPropertyRelative("choiceName").stringValue =
                                EditorGUI.TextField(new Rect(rect.x + 120, rect.y + i * baseHeightSpace, 80, baseHeight), result.FindPropertyRelative("choiceName").stringValue);
                        EditorGUI.ObjectField(new Rect(rect.x + 210, rect.y + i * baseHeightSpace, 350, baseHeight), result.FindPropertyRelative("gameEventData"));
                        i++;
                    }

                    
                    break;

                case GameEvent.Type.MovingControllerEvent:
                    height += baseHeightSpace;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Is Control?");
                    obj.FindPropertyRelative("IsControl").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 105, rect.y, 100, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsControl").boolValue);
                    break;

                case GameEvent.Type.MovingOrderEvent:
                    height += baseHeightSpace * 2;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Object Name");
                    obj.FindPropertyRelative("targetObject").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 100, rect.y, 250, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("targetObject").stringValue);

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Is Global?");
                    obj.FindPropertyRelative("IsGlobal").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 100, rect.y, 100, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsGlobal").boolValue);
                    obj.FindPropertyRelative("movingPos").vector2Value =
                        EditorGUI.Vector2Field(new Rect(rect.x + 210, rect.y, 150, EditorGUIUtility.singleLineHeight), "", obj.FindPropertyRelative("movingPos").vector2Value);
                    break;

                case GameEvent.Type.MovingTeloportEvent:
                    height += baseHeightSpace * 2;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Object Name");
                    obj.FindPropertyRelative("targetObject").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 100, rect.y, 250, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("targetObject").stringValue);

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Is Global?");
                    obj.FindPropertyRelative("IsGlobal").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 100, rect.y, 100, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsGlobal").boolValue);
                    obj.FindPropertyRelative("movingPos").vector2Value =
                        EditorGUI.Vector2Field(new Rect(rect.x + 210, rect.y, 150, EditorGUIUtility.singleLineHeight), "", obj.FindPropertyRelative("movingPos").vector2Value);
                    break;

                case GameEvent.Type.CameraLimit:
                    height += baseHeightSpace * 1;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 90, EditorGUIUtility.singleLineHeight), "Object Name");
                    obj.FindPropertyRelative("targetObject").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 95, rect.y, 180, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("targetObject").stringValue);
                    EditorGUI.LabelField(new Rect(rect.x + 280, rect.y, 90, EditorGUIUtility.singleLineHeight), "Is Limited?");
                    obj.FindPropertyRelative("IsLimited").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 370, rect.y, 20, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsLimited").boolValue);
                    break;

                case GameEvent.Type.CameraMoving:
                    height += baseHeightSpace * 2;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Object Name");
                    obj.FindPropertyRelative("targetObject").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 100, rect.y, 250, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("targetObject").stringValue);

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight), "Is Global?");
                    obj.FindPropertyRelative("IsGlobal").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 80, rect.y, 20, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsGlobal").boolValue);
                    obj.FindPropertyRelative("movingPos").vector2Value =
                        EditorGUI.Vector2Field(new Rect(rect.x + 120, rect.y, 150, EditorGUIUtility.singleLineHeight), "", obj.FindPropertyRelative("movingPos").vector2Value);
                    EditorGUI.LabelField(new Rect(rect.x + 285, rect.y, 80, EditorGUIUtility.singleLineHeight), "moveTime");
                    obj.FindPropertyRelative("delayTime").floatValue =
                        EditorGUI.FloatField(new Rect(rect.x + 355, rect.y, 80, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("delayTime").floatValue);
                    break;

                case GameEvent.Type.FadeIn:
                    rect.y += baseHeightSpace;
                    height += baseHeightSpace;

                    obj.FindPropertyRelative("delayTime").floatValue =
                        EditorGUI.FloatField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("delayTime").floatValue);
                    break;

                case GameEvent.Type.FadeOut:
                    rect.y += baseHeightSpace;
                    height += baseHeightSpace;

                    obj.FindPropertyRelative("delayTime").floatValue =
                        EditorGUI.FloatField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("delayTime").floatValue);
                    break;

                case GameEvent.Type.ScreenShake:
                    height += baseHeightSpace * 2;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Power Value");
                    obj.FindPropertyRelative("power").floatValue =
                        EditorGUI.FloatField(new Rect(rect.x + 100, rect.y, 150, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("power").floatValue);

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "delay Time");
                    obj.FindPropertyRelative("delayTime").floatValue =
                        EditorGUI.FloatField(new Rect(rect.x + 100, rect.y, 150, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("delayTime").floatValue);
                    break;

                case GameEvent.Type.BGM:
                    height += baseHeightSpace * 1;

                    rect.y += baseHeightSpace;
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, 450, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("audioClip"));
                    break;

                case GameEvent.Type.SceneLoading:
                    height += baseHeightSpace;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Scene Name");
                    obj.FindPropertyRelative("sceneName").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 100, rect.y, 250, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("sceneName").stringValue);
                    break;

                case GameEvent.Type.DoorEvent:
                    height += baseHeightSpace;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Object Name");
                    obj.FindPropertyRelative("targetObject").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 100, rect.y, 250, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("targetObject").stringValue);

                    EditorGUI.LabelField(new Rect(rect.x + 355, rect.y, 100, EditorGUIUtility.singleLineHeight), "Is Opened?");
                    obj.FindPropertyRelative("IsOpened").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 455, rect.y, 100, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsOpened").boolValue);
                    break;

                case GameEvent.Type.ObjectSpawnEvent:
                    height += baseHeightSpace;

                    rect.y += baseHeightSpace;
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("spawnObject"));
                    EditorGUI.LabelField(new Rect(rect.x + 270, rect.y, 80, EditorGUIUtility.singleLineHeight), "Is Global?");
                    obj.FindPropertyRelative("IsGlobal").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 351, rect.y, 20, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsGlobal").boolValue);
                    obj.FindPropertyRelative("movingPos").vector2Value = 
                        EditorGUI.Vector2Field(new Rect(rect.x + 375, rect.y, 200, EditorGUIUtility.singleLineHeight), "", obj.FindPropertyRelative("movingPos").vector2Value);
                    break;

                case GameEvent.Type.SetActiveEvent:
                    height += baseHeightSpace;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Object Name");
                    obj.FindPropertyRelative("targetObject").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 100, rect.y, 250, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("targetObject").stringValue);

                    EditorGUI.LabelField(new Rect(rect.x + 355, rect.y, 100, EditorGUIUtility.singleLineHeight), "Is Actived?");
                    obj.FindPropertyRelative("IsActive").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 455, rect.y, 100, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsActive").boolValue);
                    break;

                case GameEvent.Type.Wait:
                    rect.y += baseHeightSpace;
                    height += baseHeightSpace;

                    obj.FindPropertyRelative("waitTime").floatValue =
                        EditorGUI.FloatField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("waitTime").floatValue);
                    break;

                case GameEvent.Type.UIVisiableEvent:
                    height += baseHeightSpace;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Is Actived?");
                    obj.FindPropertyRelative("IsActive").boolValue =
                        EditorGUI.Toggle(new Rect(rect.x + 105, rect.y, 100, EditorGUIUtility.singleLineHeight), obj.FindPropertyRelative("IsActive").boolValue);
                    break;

                case GameEvent.Type.VariableEvent:
                    height += baseHeightSpace * 1;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x + 15, rect.y, 60, baseHeight), "Condition : ");
                    EditorGUI.PropertyField(new Rect(rect.x + 80, rect.y, 140, baseHeight),
                        obj.FindPropertyRelative("condition"), GUIContent.none);

                    float VariableX = 220 + 15;
                    switch ((VariableConditionEvent.Condition)obj.FindPropertyRelative("condition").intValue)
                    {
                        case VariableConditionEvent.Condition.VariableInt:
                            EditorGUI.LabelField(new Rect(rect.x + VariableX + 10, rect.y, 90, baseHeight), "Variable Key : ");
                            obj.FindPropertyRelative("targetKey").stringValue =
                                EditorGUI.TextField(new Rect(rect.x + VariableX + 90, rect.y, 80, baseHeight), obj.FindPropertyRelative("targetKey").stringValue);
                            EditorGUI.PropertyField(new Rect(rect.x + VariableX + 175, rect.y, 50, baseHeight), obj.FindPropertyRelative("operators"), GUIContent.none);
                            obj.FindPropertyRelative("valueInt").intValue =
                                EditorGUI.IntField(new Rect(rect.x + VariableX + 230, rect.y, 40, baseHeight), obj.FindPropertyRelative("valueInt").intValue);
                            break;

                        case VariableConditionEvent.Condition.VariableBoolean:
                            EditorGUI.LabelField(new Rect(rect.x + VariableX + 10, rect.y, 90, baseHeight), "Variable Key : ");
                            obj.FindPropertyRelative("targetKey").stringValue =
                                EditorGUI.TextField(new Rect(rect.x + VariableX + 90, rect.y, 80, baseHeight), obj.FindPropertyRelative("targetKey").stringValue);
                            obj.FindPropertyRelative("valueBoolean").boolValue =
                                EditorGUI.Toggle(new Rect(rect.x + VariableX + 180, rect.y, 20, baseHeight), obj.FindPropertyRelative("valueBoolean").boolValue);
                            break;

                        case VariableConditionEvent.Condition.ObjectSensor:
                            EditorGUI.LabelField(new Rect(rect.x + VariableX + 10, rect.y, 115, baseHeight), "Not Use This enum");
                            break;
                    }
                    break;

                case GameEvent.Type.VariableRandomEvent:
                    height += baseHeightSpace * 1;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x + 10, rect.y, 90, baseHeight), "Variable Key : ");
                    obj.FindPropertyRelative("targetKey").stringValue =
                        EditorGUI.TextField(new Rect(rect.x + 90, rect.y, 80, baseHeight), obj.FindPropertyRelative("targetKey").stringValue);
                    EditorGUI.LabelField(new Rect(rect.x + 180, rect.y, 70, baseHeight), "Min value : ");
                    obj.FindPropertyRelative("minValue").intValue =
                        EditorGUI.IntField(new Rect(rect.x + 250, rect.y, 40, baseHeight), obj.FindPropertyRelative("minValue").intValue);
                    EditorGUI.LabelField(new Rect(rect.x + 300, rect.y, 70, baseHeight), "Max value : ");
                    obj.FindPropertyRelative("maxValue").intValue =
                        EditorGUI.IntField(new Rect(rect.x + 370, rect.y, 40, baseHeight), obj.FindPropertyRelative("maxValue").intValue);
                    break;

                case GameEvent.Type.VariableConditionEvent:
                    height += baseHeightSpace * 2;

                    rect.y += baseHeightSpace;
                    EditorGUI.LabelField(new Rect(rect.x + 15, rect.y, 60, baseHeight), "Condition : ");
                    EditorGUI.PropertyField(new Rect(rect.x + 80, rect.y, 140, baseHeight),
                        obj.FindPropertyRelative("condition"), GUIContent.none);

                    float VariableConditionX = 220 + 15;
                    switch ((Condition)obj.FindPropertyRelative("condition").intValue)
                    {
                        case Condition.VariableInt:
                            EditorGUI.LabelField(new Rect(rect.x + VariableConditionX + 10, rect.y, 90, baseHeight), "Variable Key : ");
                            obj.FindPropertyRelative("targetKey").stringValue =
                                EditorGUI.TextField(new Rect(rect.x + VariableConditionX + 90, rect.y, 80, baseHeight), obj.FindPropertyRelative("targetKey").stringValue);
                            EditorGUI.PropertyField(new Rect(rect.x + VariableConditionX + 175, rect.y, 50, baseHeight), obj.FindPropertyRelative("substitution"), GUIContent.none);
                            obj.FindPropertyRelative("valueInt").intValue =
                                EditorGUI.IntField(new Rect(rect.x + VariableConditionX + 230, rect.y, 40, baseHeight), obj.FindPropertyRelative("valueInt").intValue);
                            break;

                        case Condition.VariableBoolean:
                            Substitution substitution = (Substitution)obj.FindPropertyRelative("substitution").intValue;
                            if (substitution == Substitution.below || substitution == Substitution.more ||
                                substitution == Substitution.under || substitution == Substitution.over)
                                obj.FindPropertyRelative("substitution").intValue = (int)Substitution.same;

                            EditorGUI.LabelField(new Rect(rect.x + VariableConditionX + 10, rect.y, 90, baseHeight), "Variable Key : ");
                            obj.FindPropertyRelative("targetKey").stringValue =
                                EditorGUI.TextField(new Rect(rect.x + VariableConditionX + 90, rect.y, 80, baseHeight), obj.FindPropertyRelative("targetKey").stringValue);
                            EditorGUI.PropertyField(new Rect(rect.x + VariableConditionX + 175, rect.y, 50, baseHeight), obj.FindPropertyRelative("substitution"), GUIContent.none);
                            obj.FindPropertyRelative("valueBoolean").boolValue =
                                EditorGUI.Toggle(new Rect(rect.x + VariableConditionX + 230, rect.y, 40, baseHeight), obj.FindPropertyRelative("valueBoolean").boolValue);
                            break;

                        case Condition.ObjectSensor:
                            EditorGUI.LabelField(new Rect(rect.x + VariableConditionX + 10, rect.y, 90, baseHeight), "Variable Key : ");
                            obj.FindPropertyRelative("targetKey").stringValue =
                                EditorGUI.TextField(new Rect(rect.x + VariableConditionX + 90, rect.y, 80, baseHeight), obj.FindPropertyRelative("targetKey").stringValue);
                            break;
                    }

                    rect.y += baseHeightSpace;
                    VariableConditionX += 10;
                    EditorGUI.ObjectField(new Rect(rect.x + 40, rect.y, 400, baseHeight), obj.FindPropertyRelative("IngameEvent"));
                    break;

                case GameEvent.Type.GameBadEnding:
                    break;

                case GameEvent.Type.GameExit:
                    break;

                case GameEvent.Type.CustomScript:
                    break;
            }

            reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("hight").floatValue = height;
        };
        reorderableList.onAddCallback = list =>
        {
            var prop = serializedObject.FindProperty("gameEvent");

            prop.arraySize++;
            list.index = prop.arraySize - 1;
        };
        reorderableList.elementHeightCallback = index =>
        {
            return reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("hight").floatValue;
        };
        reorderableList.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Game Event Data");
        };
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        NullableCheck();
        updateCallBack?.Invoke();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data : ", GUILayout.Width(50));
        gameEventdata = EditorGUILayout.ObjectField("", gameEventdata, typeof(GameEventData), false, GUILayout.Width(300)) as GameEventData;
        if (GUILayout.Button(new GUIContent("New"), GUILayout.Width(100)))
        {
            gameEventdata = CreateInstance<GameEventData>();
            Initialize(gameEventdata);
        }
        if (GUILayout.Button(new GUIContent("»ý¼º"), GUILayout.Width(100)))
        {
            AssetDatabase.CreateAsset(gameEventdata, AssetDatabase.GenerateUniqueAssetPath("Assets/ResourceDeta/Data/EventData/EventData.asset"));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);
         
        scrollBar = EditorGUILayout.BeginScrollView(scrollBar);
        doyoulist?.Invoke();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(10);
        ApplyModifiedCallBack?.Invoke();
    }

    private void NullableCheck()
    {
        if (gameEventdata == null && serializedObject != null)
        {
            updateCallBack -= serializedObject.Update;
            ApplyModifiedCallBack -= serializedObject.ApplyModifiedProperties;
            serializedObject = null;

            doyoulist -= dataList.DoLayoutList;
            dataList = null;
        }
        else if (gameEventdata != null && serializedObject == null)
        {
            Initialize(gameEventdata);
        }
    }
}
