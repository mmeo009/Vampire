using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class MonsterManager
{
    public HashSet<MonsterController> monsters = new HashSet<MonsterController>();
    public int waveNum = 0;
    [SerializeField] private int additionalHp;
    [SerializeField] private int additionalSpeed;

    public void CreateMonster(Transform pos, int monsterIndex, string monsterCode = null, float hp = 0, float damage = 0, float speed = 0)
    {
        Entity_Enemy.Param monster = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary, monsterIndex, monsterCode);
        if(monster != null)
        {
            GameObject monsterObject = Managers.Data.Instantiate(monsterCode, null, true);
            monsterObject.transform.position = pos.position;
            MonsterController mc = monsterObject.GetComponent<MonsterController>();
            mc.LoadMyData(monster.index, monsterCode);
            
            monsters.Add(mc);
        }
    }

    private void LoadData()
    {
        MonsterStats monster = new MonsterStats();
    }
    public void WavePlus(int amount)
    {

    }

}



