using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManagers
{
    public Dictionary<string, MonststerStats> monsters = new Dictionary<string, MonststerStats>();
    public void CreateMonster(string monsterCode)
    {
        string monsterFilePath = $"Prefabs/";
    }
}


public class MonststerStats
{
    public string code;
    public string name;
    public double hp;
    public double damage;
    public double moveSpeed;
}
