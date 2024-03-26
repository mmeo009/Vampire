using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Entity_Enemy.Param enemy;
    public void Start()
    {
        Managers.Data.LoadBaseData<Entity_Player>("playerData");
        Managers.Data.LoadBaseData<Entity_Enemy>("enemyData");
        enemy = Managers.Data.GetValueFromDictionary(Managers.Data.enemyDictionary, 1);
    }
}
