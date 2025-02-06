using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : GameManager<StageManager>
{
    [SerializeField] private PlayerSpawn playerSpawn;
    [SerializeField] private List<EnemySpawn> EnemySpawn = new List<EnemySpawn>();

    public bool b_IsSpawn = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ClearFuction()
    {
        playerSpawn = null;

        EnemySpawn.Clear();
    }

    public void SetPlayerSpawn(PlayerSpawn player)
    {
        playerSpawn = player;
    }

    public void SetEnemySpawn(EnemySpawn enemy)
    {
        EnemySpawn.Add(enemy);
    }
}
