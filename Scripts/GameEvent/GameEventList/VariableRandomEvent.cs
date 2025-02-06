using System.Collections;
using UnityEngine;

public class VariableRandomEvent : GameEvent
{
    public string targetKey;
    public int valueMin;
    public int valueMax;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        if (ObjectManager.variableInt.ContainsKey(targetKey))
            ObjectManager.variableInt[targetKey] = Random.Range(valueMin, valueMax);
        else
            ObjectManager.variableInt.Add(targetKey, Random.Range(valueMin, valueMax));

        yield return null;

        IsEventEnd = true;
    }
}
