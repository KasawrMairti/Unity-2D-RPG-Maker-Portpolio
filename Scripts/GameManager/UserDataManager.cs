using System;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : GameManager<UserDataManager>
{
    [Serializable] public struct Data
    {
        public DTOPlayer dtoPlayer;
        public DTOStage dtoStage;

        public Dictionary<string, GameObject> objects;
        public Dictionary<string, int> variableInt;
        public Dictionary<string, bool> variableBoolean;
    }

    public Data data;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        data = JsonManager.Instance.Load();

        // print($"data.dtoPlayer : {data.dtoPlayer}");
        // print($"data.dtoStage : {data.dtoStage}");
        // print($"data.objects : {data.objects}");
    }

    private void Update()
    {
    }

    public PlayerStatus GetPlayerStatus()
    {
        return data.dtoPlayer.status;
    }
}
