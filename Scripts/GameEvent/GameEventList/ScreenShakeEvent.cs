using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenShakeEvent : GameEvent
{
    public float power;
    public float delayTime;

    private CameraLimitEvent cameraEvent;

    protected override void Initialize()
    {

    }

    private void Start()
    {
        cameraEvent = GameEventManager.Instance.cameraLimitEvent;
    }

    protected override IEnumerator Event()
    {
        cameraEvent.IsLimited = false;
        yield return new WaitUntil(() => cameraEvent.Trigger());

        float curTime = 0.0f;
        while (curTime < delayTime)
        {
            curTime += Time.deltaTime;

            cameraEvent.gameObject.transform.position = new Vector2(Random.Range(-power, power), Random.Range(-power, power));

            yield return null;
        }

        cameraEvent.IsLimited = true;
        yield return new WaitUntil(() => cameraEvent.Trigger());

        IsEventEnd = true;
    }
}
