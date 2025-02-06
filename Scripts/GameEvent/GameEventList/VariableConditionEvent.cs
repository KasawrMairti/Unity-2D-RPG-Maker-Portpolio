using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Extensions.UIVerticalScroller;

public class VariableConditionEvent : GameEvent
{
    public enum Substitution
    {
        [Description("<")]  over,  [Description(">")]  under, [Description(">=")] more, 
        [Description("<=")] below, [Description("==")] same,  [Description("!=")] different
    }

    public enum Condition { VariableInt, VariableBoolean, ObjectSensor }

    public Condition condition;
    public Substitution substitution;
    public string targetKey;
    public int valueInt;
    public bool valueBoolean;

    public GameEventData gameEvent;
    public bool IsEvent;


    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        bool checkBoolean = false;

        if (!ObjectManager.variableInt.ContainsKey(targetKey))
            ObjectManager.Add(targetKey, 0);
        if (!ObjectManager.variableBoolean.ContainsKey(targetKey))
            ObjectManager.Add(targetKey, false);

        switch (condition)
        {
            case Condition.VariableInt:
                int resultInt = ObjectManager.variableInt[targetKey];
                switch (substitution)
                {
                    case Substitution.over:
                        if (resultInt < valueInt) checkBoolean = true;
                        break;

                    case Substitution.under:
                        if (resultInt > valueInt) checkBoolean = true;
                        break;

                    case Substitution.more:
                        if (resultInt <= valueInt) checkBoolean = true;
                        break;

                    case Substitution.below:
                        if (resultInt >= valueInt) checkBoolean = true;
                        break;

                    case Substitution.same:
                        if (resultInt == valueInt) checkBoolean = true;
                        break;

                    case Substitution.different:
                        if (resultInt != valueInt) checkBoolean = true;
                        break;
                }
                break;

            case Condition.VariableBoolean:
                bool resultBoolean = ObjectManager.variableBoolean[targetKey];
                switch (substitution)
                {
                    case Substitution.same:
                        if (resultBoolean == valueBoolean) checkBoolean = true;
                        break;

                    case Substitution.different:
                        if (resultBoolean != valueBoolean) checkBoolean = true;
                        break;
                }
                break;

            case Condition.ObjectSensor:
                {
                    Debug.Log($"targetKey : {targetKey}");
                    Debug.Log($"ObjectManager.objects[targetKey] : {ObjectManager.objects[targetKey]}");

                    checkBoolean = ObjectManager.objects[targetKey].gameObject.GetComponentInChildren<ColliderSensor>().b_IsTarget;
                }
                break;
        }

        if (checkBoolean) GameEventManager.Instance.EventTriggerCondition(gameEvent);

        yield return null;

        IsEvent = false;
        IsEventEnd = true;
    }
}
