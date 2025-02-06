using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovingOrderEvent : GameEvent
{
    public string targetObject;
    public bool IsGlobal;
    public Vector2 movingOrderPos;

    private NavMeshAgent agent;
    private GameObject obj;
    private float moveX;
    private float moveY;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        obj = ObjectManager.objects[targetObject];
        agent = obj.GetComponent<NavMeshAgent>();

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

        agent.isStopped = false;

        Vector3 lastPos = new Vector3(moveX, moveY, agent.gameObject.transform.position.z);
        obj.GetComponent<Player>()?.Order(lastPos);

        yield return new WaitUntil(() => agent.SetDestination(lastPos) && !agent.pathPending);

        IsEventEnd = true;
    }
}
