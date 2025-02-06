using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimitEvent : GameEvent
{
    public string targetObject;
    public bool IsLimited;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        playerCamera obj = ObjectManager.objects["PlayerCamera"].GetComponent<playerCamera>();
        GameObject target;
        if (targetObject != "" 
            && targetObject != null)
             target = ObjectManager.objects[targetObject];
        else target = ObjectManager.objects["Player"];

        if (IsLimited)
        {
            obj.virtualCamera.Follow = target.transform;
        }
        else
        {
            obj.virtualCamera.Follow = null;
        }

        yield return null;

        IsEventEnd = true;
    }
}
