using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Entity_Enemy.Param enemy;
    public void Start()
    {
        Managers.Data.LoadBaseData<Entity_Player>("PlayerData");
        Managers.Data.LoadBaseData<Entity_Enemy>("EnemyData");
        Managers.Data.LoadBaseData<Entity_Perk>("PerkData");
        enemy = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary, 1);
    }
}
