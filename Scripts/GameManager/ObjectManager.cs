using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : GameManager<ObjectManager>
{
    public static Dictionary<string, GameObject> objects { get; private set; }
    public static Dictionary<string, int> variableInt { get; private set; }
    public static Dictionary<string, bool> variableBoolean { get; private set; }

    protected override void Awake()
    {
    }

    private void Start()
    {
        objects = UserDataManager.Instance.data.objects;
        variableInt = UserDataManager.Instance.data.variableInt;
        variableBoolean = UserDataManager.Instance.data.variableBoolean;
    }

    public static void Add(string text, GameObject gameObject)
    {
        if (objects.ContainsKey(text)) objects[text] = gameObject;
        else objects.Add(text, gameObject);

        JsonManager.Instance.Save();
    }

    public static void Add(string text, int value)
    {
        if (variableInt.ContainsKey(text)) variableInt[text] = value;
        else variableInt.Add(text, value);

        JsonManager.Instance.Save();
    }

    public static void Add(string text, bool value)
    {
        if (variableBoolean.ContainsKey(text)) variableBoolean[text] = value;
        else variableBoolean.Add(text, value);

        JsonManager.Instance.Save();
    }

    public static void ClearObject()
    {
        objects.Clear();
    }

    public static void ClearVariableInt()
    {
        variableInt.Clear();
    }

    public static void ClearVariableBoolean()
    {
        variableBoolean.Clear();
    }
}
