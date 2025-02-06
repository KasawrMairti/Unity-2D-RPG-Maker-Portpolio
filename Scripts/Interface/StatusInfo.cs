[System.Serializable]
public struct MonsterStatus
{
    public string name;
    public string description;
    public int lv;

    public float hp;
    public float hpMax;
    public float attack;
    public float attackSpeed;
    public float deffence; 
}

[System.Serializable]
public struct PlayerStatus
{
    public float HpMax;
    public float Hp;
    public float HpRegen;

    public float MpMax;
    public float Mp;
    public float MpRegen;

    public int lv;
    public float ExpMax;
    public float Exp;

    public float attackDamage;
    public float attackSpeed;
    public float attackCritical;
    public float attackCriticalDamage;
    public float deffence;
    public float avoid;

    public int Stat_Bonus;
    public int Stat_STR;
    public int Stat_DEX;
    public int Stat_INT;
    public int Stat_WIS;
    public int Stat_CON;
    public int Stat_LUK;
}
