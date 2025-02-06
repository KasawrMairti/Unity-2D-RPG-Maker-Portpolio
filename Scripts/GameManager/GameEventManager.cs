using System.Collections;
using UnityEngine;

public class GameEventManager : GameManager<GameEventManager>
{
    private GameEventData gameEventData;
    private GameEventData gameEventDataCondition;

    #region EvenList
    public  ScreenDialogueEvent screenDialogueEvent { get; private set; }
    public  TalkDialogueEvent talkDialogueEvent { get; private set; }
    public  ChoiceEvent choiceEvent { get; private set; }
    public  MovingControllerEvent movingControllerEvent { get; private set; }
    public  MovingOrderEvent movingOrderEvent { get; private set; }
    public  MovingTeloportEvent movingTeloportEvent { get; private set; }
    public  CameraLimitEvent cameraLimitEvent { get; private set; }
    public  CameraMovingEvent cameraMovingEvent { get; private set; }
    public  FadeInEvent fadeInEvent { get; private set; }
    public  FadeOutEvent fadeOutEvent { get; private set; }
    public  ScreenShakeEvent screenShakeEvent { get; private set; }
    public  BGMEvent bgmEvent { get; private set; }
    public  SceneLoadingEvent sceneLoadingEvent { get; private set; }
    public  DoorEvent doorEvent { get; private set; }
    public ObjectSpawnEvent objectSpawnEvent { get; private set; }
    public  SetActiveEvent setActiveEvent { get; private set; }
    public WaitEvent waitEvent { get; private set; }
    public VariableEvent variableEvent { get; private set; }
    public VariableRandomEvent variableRandomEvent { get; private set; }
    public VariableConditionEvent variableConditionEvent { get; private set; }
    public  GameExitEvent gameExitEvent { get; private set; }
    public  GameBadEndingEvent gameBadEndingEvent { get; private set; }
    public CustomScriptEvent customScriptEvent { get; private set;  }
    public UIVisibleEvent uiVisibleEvent { get; private set; }
    #endregion

    #region Function
    #endregion

    protected override void Awake()
    {
        base.Awake();

        screenDialogueEvent = GetComponent<ScreenDialogueEvent>();
        talkDialogueEvent = GetComponent<TalkDialogueEvent>();
        choiceEvent = GetComponent<ChoiceEvent>();
        movingOrderEvent = GetComponent<MovingOrderEvent>();
        movingTeloportEvent = GetComponent<MovingTeloportEvent>();
        cameraLimitEvent = GetComponent<CameraLimitEvent>();
        cameraMovingEvent = GetComponent<CameraMovingEvent>();
        waitEvent = GetComponent<WaitEvent>();
        fadeInEvent = GetComponent<FadeInEvent>();
        fadeOutEvent = GetComponent<FadeOutEvent>();
        screenShakeEvent = GetComponent<ScreenShakeEvent>();
        bgmEvent = GetComponent<BGMEvent>();
        customScriptEvent = GetComponent<CustomScriptEvent>();
        sceneLoadingEvent = GetComponent<SceneLoadingEvent>();
        gameExitEvent = GetComponent<GameExitEvent>();
        gameBadEndingEvent = GetComponent<GameBadEndingEvent>();
        movingControllerEvent = GetComponent<MovingControllerEvent>();
        doorEvent = GetComponent<DoorEvent>();
        objectSpawnEvent = GetComponent<ObjectSpawnEvent>();
        setActiveEvent = GetComponent<SetActiveEvent>();
        variableConditionEvent = GetComponent<VariableConditionEvent>();
        variableEvent = GetComponent<VariableEvent>();
        variableRandomEvent = GetComponent<VariableRandomEvent>();
        uiVisibleEvent = GetComponent<UIVisibleEvent>();
    }

    public bool EventTriggerCondition(GameEventData gameEventData)
    {
        if (gameEventDataCondition != gameEventData)
        {
            gameEventDataCondition = gameEventData;

            StartCoroutine(Event(gameEventData));

            return false;
        }
        return this.gameEventData.IsEventEnd;
    }

    public bool EventTrigger(GameEventData gameEventData)
    {
        if (this.gameEventData != gameEventData)
        {
            this.gameEventData = gameEventData;

            StartCoroutine(Event(gameEventData));

            return false;
        }
        return this.gameEventData.IsEventEnd;
    }

    private IEnumerator Event(GameEventData gameEventData)
    {
            foreach (var events in gameEventData.gameEvent)
            {
                yield return null;

                switch (events.type)
                {
                    case GameEvent.Type.ScreenDialogueEvent:
                        screenDialogueEvent.screenDialogues = events.screenDialogueData;
                        yield return new WaitUntil(() => screenDialogueEvent.Trigger());
                        break;

                    case GameEvent.Type.TalkDialogueEvent:
                        talkDialogueEvent.talkDialogues = events.talkDialogueData;
                        yield return new WaitUntil(() => talkDialogueEvent.Trigger());
                        break;

                    case GameEvent.Type.ChoiceEvent:
                        choiceEvent.choiceData = events.choiceData;
                        yield return new WaitUntil(() => choiceEvent.Trigger());
                        break;

                    case GameEvent.Type.MovingControllerEvent:
                        movingControllerEvent.IsControl = events.IsControl;
                        yield return new WaitUntil(() => movingControllerEvent.Trigger());
                        break;

                    case GameEvent.Type.MovingOrderEvent:
                        movingOrderEvent.targetObject = events.targetObject;
                        movingOrderEvent.IsGlobal = events.IsGlobal;
                        movingOrderEvent.movingOrderPos = events.movingPos;
                        yield return new WaitUntil(() => movingOrderEvent.Trigger());
                        break;

                    case GameEvent.Type.MovingTeloportEvent:
                        movingTeloportEvent.targetObject = events.targetObject;
                        movingTeloportEvent.IsGlobal = events.IsGlobal;
                        movingTeloportEvent.movingOrderPos = events.movingPos;
                        yield return new WaitUntil(() => movingTeloportEvent.Trigger());
                        break;

                    case GameEvent.Type.CameraLimit:
                        cameraLimitEvent.targetObject = events.targetObject;
                        cameraLimitEvent.IsLimited = events.IsLimited;
                        yield return new WaitUntil(() => cameraLimitEvent.Trigger());
                        break;

                    case GameEvent.Type.CameraMoving:
                        cameraMovingEvent.targetObject = events.targetObject;
                        cameraMovingEvent.IsGlobal = events.IsGlobal;
                        cameraMovingEvent.posLast = events.movingPos;
                        cameraMovingEvent.moveTime = events.delayTime;
                        yield return new WaitUntil(() => cameraMovingEvent.Trigger());
                        break;

                    case GameEvent.Type.FadeIn:
                        fadeInEvent.delayTime = events.delayTime;
                        yield return new WaitUntil(() => fadeInEvent.Trigger());
                        break;

                    case GameEvent.Type.FadeOut:
                        fadeOutEvent.delayTime = events.delayTime;
                        yield return new WaitUntil(() => fadeOutEvent.Trigger());
                        break;

                    case GameEvent.Type.ScreenShake:
                        screenShakeEvent.delayTime = events.delayTime;
                        screenShakeEvent.power = events.power;
                        yield return new WaitUntil(() => screenShakeEvent.Trigger());
                        break;

                    case GameEvent.Type.BGM:
                        bgmEvent.clip = events.audioClip;
                        yield return new WaitUntil(() => bgmEvent.Trigger());
                        break;

                    case GameEvent.Type.SceneLoading:
                        sceneLoadingEvent.sceneName = events.sceneName;
                        sceneLoadingEvent.movePos = events.movingPos;
                        yield return new WaitUntil(() => sceneLoadingEvent.Trigger());
                        break;

                    case GameEvent.Type.DoorEvent:
                        doorEvent.targetObject = events.targetObject;
                        doorEvent.IsOpend = events.IsOpened;
                        yield return new WaitUntil(() => doorEvent.Trigger());
                        break;

                    case GameEvent.Type.ObjectSpawnEvent:
                        objectSpawnEvent.spawnObj = events.spawnObject;
                        objectSpawnEvent.spawnPos = events.movingPos;
                        yield return new WaitUntil(() => objectSpawnEvent.Trigger());
                        break;

                    case GameEvent.Type.SetActiveEvent:
                        setActiveEvent.targetObject = events.targetObject;
                        setActiveEvent.IsActive = events.IsActive;
                        yield return new WaitUntil(() => setActiveEvent.Trigger());
                        break;

                    case GameEvent.Type.Wait:
                        waitEvent.rateTime = events.waitTime;
                        yield return new WaitUntil(() => waitEvent.Trigger());
                        break;

                    case GameEvent.Type.UIVisiableEvent:
                        uiVisibleEvent.IsActive = events.IsActive;
                        yield return new WaitUntil(() => uiVisibleEvent.Trigger());
                        break;

                    case GameEvent.Type.VariableEvent:
                        variableEvent.condition = events.condition;
                        variableEvent.operators = events.operators;
                        variableEvent.targetKey = events.targetKey;
                        variableEvent.valueInt = events.valueInt;
                        variableEvent.valueBoolean = events.valueBoolean;
                        yield return new WaitUntil(() => variableEvent.Trigger());
                        break;

                    case GameEvent.Type.VariableRandomEvent:
                        variableRandomEvent.targetKey = events.targetKey;
                        variableRandomEvent.valueMin = events.minValue;
                        variableRandomEvent.valueMax = events.maxValue;
                        yield return new WaitUntil(() => variableRandomEvent.Trigger());
                        break;

                    case GameEvent.Type.VariableConditionEvent:
                        variableConditionEvent.condition = events.condition;
                        variableConditionEvent.substitution = events.substitution;
                        variableConditionEvent.targetKey = events.targetKey;
                        variableConditionEvent.valueInt = events.valueInt;
                        variableConditionEvent.valueBoolean = events.valueBoolean;
                        variableConditionEvent.gameEvent = events.IngameEvent;
                        yield return new WaitUntil(() => variableConditionEvent.Trigger());
                        break;

                    case GameEvent.Type.GameBadEnding:
                        break;

                    case GameEvent.Type.GameExit:
                        break;

                    case GameEvent.Type.CustomScript:
                        break;
                }
            }

            gameEventData.IsEventEnd = true;
    }
}
