using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Serializable] public struct EnemyInfo
    {
        public GameObject obj;
        public int spawnCount;
        public float spawnTime;
        public Vector2 orderPosition;
    }

    [SerializeField] public List<EnemyInfo> info = new List<EnemyInfo>();
    public float spawnDelay;
    public float nextTime;

    private IEnumerator spawnEventer()
    {
        yield return new WaitForSeconds(spawnDelay);

        foreach (EnemyInfo info in info)
        {
            for (int i = 0; i < info.spawnCount; i++)
            {
                GameObject obj = Instantiate(info.obj);
                obj.transform.position = transform.position;
                obj.GetComponent<Monster>().Order(info.orderPosition);

                yield return new WaitForSeconds(info.spawnTime);
            }

            yield return new WaitForSeconds(nextTime);
        }
    }

    public void spawnEvent()
    {
        StartCoroutine(spawnEventer());
    }
}
