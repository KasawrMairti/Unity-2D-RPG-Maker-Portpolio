using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static VariableConditionEvent;
using static VariableEvent;


[Serializable] public struct ChoiceData
{
    public string choiceName;
    public GameEventData gameEventData;
}

[Serializable] public struct GameEventDatas
{
    public GameEvent.Type type;

    // Data Function
    public float hight;

    public GameObject gameObject;

    // Event Function
    public string targetObject;

    // ScreenDialogueEvent
    public ScreenDialogueData screenDialogueData;

    // TalkDialogueEvent
    public TalkDialogueData talkDialogueData;

    // ChoiceEvent
    public List<ChoiceData> choiceData;    

    // MovingControllerEvent
    public bool IsControl;

    // MovingOrderEvent
    // MovingTeloportEvent
    // CameraMoving
    // : targetObject
    public bool IsGlobal;
    public Vector2 movingPos;

    // CameraLimit
    public bool IsLimited;
    

    // FadeIn
    // FadeOut
    public float delayTime;
    
    // ScreenShake
    public float power;

    // BGM
    public AudioClip audioClip;

    // SceneLoading
    public string sceneName;

    // DoorEvent
    public bool IsOpened;

    // ObjectSpawnEvent
    public GameObject spawnObject;

    // SetActiveEvent
    public bool IsActive;

    // Variable Event
    public Condition condition;
    public Operator operators;
    // Variable Random Event
    public int minValue;
    public int maxValue;
    // Variable Condition Event
    public Substitution substitution;
    public string targetKey;
    public int valueInt;
    public bool valueBoolean;
    public GameEventData IngameEvent;

    // Wait
    public float waitTime;

    // GameExit

    // GameBadEnding

    // CustomScript
}

public class GameEventData : ScriptableObject
{
    public List<GameEventDatas> gameEvent = new();
    public bool IsEventEnd = false;
}
