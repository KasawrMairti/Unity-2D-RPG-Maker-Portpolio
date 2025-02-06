using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitEvent : GameEvent
{


    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        yield return null;

        IsEventEnd = true;
    }
}
