using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Serializable] public struct PlayerInfo
    {
        public GameObject obj;
        public int spawnCount;
        public float spawnTime;
        public Vector2 orderPosition;
    }

    [SerializeField] public List<PlayerInfo> info = new List<PlayerInfo>();
    public float spawnDelay;
    public float nextTime;

    private IEnumerator spawnEventer()
    {
        yield return new WaitForSeconds(spawnDelay);

        foreach (PlayerInfo info in info)
        {
            for (int i = 0; i < info.spawnCount; i++)
            {
                GameObject obj = Instantiate(info.obj);
                obj.transform.position = transform.position;
                obj.GetComponent<Player>().Order(info.orderPosition);

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
