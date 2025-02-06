using System.Collections;
using UnityEngine;

public class MovingTeloportEvent : GameEvent
{
    public string targetObject;
    public bool IsGlobal;
    public Vector2 movingOrderPos;

    private GameObject obj;
    private float moveX;
    private float moveY;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        obj = ObjectManager.objects[targetObject];

        if (IsGlobal)
        {
            moveX = movingOrderPos.x;
            moveY = movingOrderPos.y;
        }
        else
        {
            moveX = obj.transform.position.x + movingOrderPos.x;
            moveY = obj.transform.position.y + movingOrderPos.y;
        }

        obj.transform.position = new Vector2(moveX, moveY);

        yield return null;

        IsEventEnd = true;
    }
}
