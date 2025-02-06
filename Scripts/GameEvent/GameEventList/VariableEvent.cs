using System.Collections;
using System.ComponentModel;
using static VariableConditionEvent;

public class VariableEvent : GameEvent
{
    public enum Operator
    {
        [Description("=")] Set, [Description("+")] Add, [Description("-")] Sub,
        [Description("*")] Mul, [Description("/")] Div, [Description("%")] Mod
    }

    public Condition condition;
    public Operator operators;
    public string targetKey;
    public int valueInt;
    public bool valueBoolean;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        if (!ObjectManager.variableInt.ContainsKey(targetKey))
            ObjectManager.Add(targetKey, 0);
        if (!ObjectManager.variableBoolean.ContainsKey(targetKey))
            ObjectManager.Add(targetKey, false);

        switch (condition)
        {
            case Condition.VariableInt:
                switch (operators)
                {
                    case Operator.Set:
                        ObjectManager.variableInt[targetKey] = valueInt;
                        break;

                    case Operator.Add:
                        ObjectManager.variableInt[targetKey] += valueInt;
                        break;

                    case Operator.Sub:
                        ObjectManager.variableInt[targetKey] -= valueInt;
                        break;

                    case Operator.Mul:
                        ObjectManager.variableInt[targetKey] *= valueInt;
                        break;

                    case Operator.Div:
                        ObjectManager.variableInt[targetKey] /= valueInt;
                        break;

                    case Operator.Mod:
                        ObjectManager.variableInt[targetKey] %= valueInt;
                        break;
                }
                break;

            case Condition.VariableBoolean:
                ObjectManager.variableBoolean[targetKey] = valueBoolean;
                break;
        }
        

        yield return null;

        IsEventEnd = true;
    }
}
