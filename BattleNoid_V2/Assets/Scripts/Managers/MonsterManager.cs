using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class MonsterManager
{
    public Dictionary<string, MonststerStats> monsters = new Dictionary<string, MonststerStats>();
    public void CreateMonster(string monsterCode)
    {
        string monsterFilePath = $"Prefabs/Monsters/{monsterCode}";
    }
}



