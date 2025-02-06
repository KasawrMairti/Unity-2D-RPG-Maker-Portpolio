using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObjectSpawnEvent : GameEvent
{
    public GameObject spawnObj;
    public Vector2 spawnPos;
    public int count;

    protected override void Initialize()
    {
        
    }

    protected override IEnumerator Event()
    {
        for (int i = 0; i < count; i ++)
        {
            GameObject obj = Instantiate(spawnObj);
            obj.transform.position = spawnPos;
        }

        yield return null;

        IsEventEnd = true;
    }
}
