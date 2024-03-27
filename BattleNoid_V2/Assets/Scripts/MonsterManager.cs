using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    public Dictionary<string, MonststerStats> monsters = new Dictionary<string, MonststerStats>();
    public void CreateMonster(string monsterCode)
    {
        string monsterFilePath = $"Prefabs/Monsters/{monsterCode}";
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
