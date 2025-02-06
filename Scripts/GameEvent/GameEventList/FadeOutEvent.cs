using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutEvent : GameEvent
{
    public float delayTime;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        UIManager.Instance.FadeOutEvent(delayTime);

        yield return new WaitUntil(() => UIManager.Instance.b_EventFadeInOut);

        IsEventEnd = true;
    }
}
