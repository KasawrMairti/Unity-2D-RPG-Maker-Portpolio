using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveEvent : GameEvent
{
    public string targetObject;
    public bool IsActive;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        ObjectManager.objects[targetObject].SetActive(IsActive);

        yield return null;

        IsEventEnd = true;
    }
}
