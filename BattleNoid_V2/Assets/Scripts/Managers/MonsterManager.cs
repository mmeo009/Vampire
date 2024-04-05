using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class MonsterManager
{
    public HashSet<MonsterController> monsters = new HashSet<MonsterController>();
    public void CreateMonster(string monsterCode)
    {
        Entity_Enemy.Param monster = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary,0, monsterCode);
        if(monster != null)
        {
            
        }
    }
}



