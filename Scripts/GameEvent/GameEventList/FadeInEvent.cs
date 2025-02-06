using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInEvent : GameEvent
{
    public float delayTime;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        UIManager.Instance.FadeInEvent(delayTime);

        yield return new WaitUntil(() => UIManager.Instance.b_EventFadeInOut);

        IsEventEnd = true;
    }
}
