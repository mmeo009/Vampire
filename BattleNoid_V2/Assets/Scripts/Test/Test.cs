using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public async void Start()
    {
        await Managers.Data.LoadBaseData<Entity_Player>("PlayerData");
        await Managers.Data.LoadBaseData<Entity_Enemy>("EnemyData");
        await Managers.Data.LoadBaseData<Entity_Perk>("PerkData");
        Managers.Data.GetDataFromDictionary(Managers.Data.playerDictionary, 1);
        Managers.Monster.GetWaveDatas();
        Managers.Data.LoadAllAsync<Object>("Prefabs", (key, count, totalCount) =>
        {
            Debug.Log("key : " + key + " Count : " + count + " totalCount : " + totalCount);
            Managers.UI.optionWindowPrefab = Managers.Data.Load<GameObject>("OptionsWindow");
        });

    }
}
