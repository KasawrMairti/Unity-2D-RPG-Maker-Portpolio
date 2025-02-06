using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingControllerEvent : GameEvent
{
    public bool IsControl;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        SystemManager.isControl = IsControl ? SystemManager.Control.CONTROL : SystemManager.Control.NOTCONTROL;

        yield return null;

        IsEventEnd = true;
    }
}
