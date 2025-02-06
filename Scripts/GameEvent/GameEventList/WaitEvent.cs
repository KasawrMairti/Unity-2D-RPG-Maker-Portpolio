using System.Collections;
using UnityEngine;

public class WaitEvent : GameEvent
{
    public float rateTime = 0.0f;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        yield return new WaitForSeconds(rateTime);
        IsEventEnd = true;
    }
}
