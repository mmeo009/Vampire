using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Entity_Enemy.Param enemy;
    public async void Start()
    {
        await Managers.Data.LoadBaseData<Entity_Player>("PlayerData");
        await Managers.Data.LoadBaseData<Entity_Enemy>("EnemyData");
        await Managers.Data.LoadBaseData<Entity_Perk>("PerkData");
        enemy = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary, 1);
    }
}
