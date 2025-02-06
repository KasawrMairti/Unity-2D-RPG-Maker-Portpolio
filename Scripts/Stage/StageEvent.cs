using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEvent : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private bool b_PlayerSpawn = false;
    [SerializeField] private bool b_EnemySpawn = false;

    [Header("EnemySpawn")]
    [SerializeField] private List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

    [Header("PlayerSpawn")]
    [SerializeField] private List<PlayerSpawn> playerSpawns = new List<PlayerSpawn>();

    private void Start()
    {
        if (b_EnemySpawn)
        {
            foreach (var enemy in enemySpawns)
            {
                enemy.spawnEvent();
            }
        }

        if (b_PlayerSpawn)
        {
            foreach (var player in playerSpawns)
            {
                player.spawnEvent();
            }
        }
    }
}
