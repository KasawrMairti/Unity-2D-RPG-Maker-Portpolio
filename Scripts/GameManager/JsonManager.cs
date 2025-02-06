using System.IO;
using UnityEngine;
using Data = UserDataManager.Data;

public class JsonManager : GameManager<JsonManager>
{
    private string path;

    private Data data;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Save()
    {
        if (data.dtoPlayer == null)
        {
            PlayerStatus status;
            status.HpMax = 100.0f;
            status.Hp = status.HpMax;
            status.HpRegen = 1.0f;
            status.MpMax = 10.0f;
            status.Mp = status.MpMax;
            status.MpRegen = 0.5f;
            status.lv = 1;
            status.ExpMax = 100.0f;
            status.Exp = 0f;

            status.attackDamage = 10.0f;
            status.attackSpeed = 1.5f;
            status.attackCritical = 0.1f;
            status.attackCriticalDamage = 1.5f;
            status.deffence = 10.0f;
            status.avoid = 0.05f;

            status.Stat_Bonus = 10;
            status.Stat_STR = 1;
            status.Stat_DEX = 1;
            status.Stat_INT = 1;
            status.Stat_WIS = 1;
            status.Stat_CON = 1;
            status.Stat_LUK = 1;
            data.dtoPlayer = new DTOPlayer(status);

            data.objects = new();
            data.variableInt = new();
            data.variableBoolean = new();

            print("status Data 적용!");
        }

        string json = JsonUtility.ToJson(data, true);

        PlayerPrefs.SetString("data", json);
        //print(json);
    }
    
    public Data Load()
    {
        PlayerPrefs.DeleteKey("data");

        if (PlayerPrefs.GetString("data") == "")
        {
            Debug.Log("저장 파일이 없어 새로 만듭니다");
            Save();
            return data;
        }
        else
        {
            data = JsonUtility.FromJson<Data>(PlayerPrefs.GetString("data"));
            print(data);
        }

        return data;
    }
}
