using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovingEvent : GameEvent
{
    public string targetObject = "";

    public bool IsGlobal;
    public Vector3 posStart; // 이동 시작 위치
    public Vector3 posLast; // 위치로 보게 할 Vector2
    public float moveTime; // 몇 초 까지 가게 할 것인가?
    public float moveTimeCur; // 몇 초 까지 가게 할 것인가?

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


        playerCamera camera = ObjectManager.objects["PlayerCamera"].GetComponent<playerCamera>();
        posStart = camera.transform.position;
        posLast = new Vector3(posLast.x, posLast.y, -10.0f);

        if (targetObject == "" || targetObject == null)
        {
            if (!IsGlobal) 
                posLast = camera.gameObject.transform.position + posLast;
        }
        else
        {
            posLast = ObjectManager.objects[targetObject].transform.position;
        }

        while (moveTimeCur <= moveTime)
        {
            moveTimeCur += Time.deltaTime;
            camera.gameObject.transform.position = Vector3.Lerp(posStart, posLast, moveTimeCur / moveTime);

            yield return null;
        }

        IsEventEnd = true;
        moveTimeCur = 0.0f;
    }
}
