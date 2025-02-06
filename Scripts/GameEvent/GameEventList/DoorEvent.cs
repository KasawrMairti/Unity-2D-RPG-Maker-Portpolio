using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : GameEvent
{
    public string targetObject;
    public bool IsOpend;

    protected override void Initialize()
    {
        
    }

    protected override IEnumerator Event()
    {
        EventCollider door = ObjectManager.objects[targetObject].GetComponent<EventCollider>();
        door.DoorEvent(IsOpend);

        yield return new WaitForSeconds(0.5f);

        IsEventEnd = true;
    }
}
