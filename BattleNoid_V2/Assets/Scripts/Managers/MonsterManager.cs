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
            LoadData(monster, mc);
            mc.AddStats(StatType.None);
            mc.AddStats(StatType.MAXHP, hp);
            mc.AddStats(StatType.AttackDamage, damage);
            mc.AddStats(StatType.MoveSpeed, speed);
            monsters.Add(mc);
        }
    }

    private void LoadData(Entity_Enemy.Param monsterData, MonsterController monsterController)
    {
        MonsterStats monster = new MonsterStats();
        monster.code = monsterData.code;
        monster.hp = monsterData.baseHp;
        monster.knockBackAmount = monsterData.knockBackAmount;
        monster.knockBackTime = monsterData.knockBackTime;
        monster.attackDamage = monsterData.baseDamage;
        monster.attackRange = monsterData.baseRange;
        monster.moveSpeed = monsterData.baseMoveSpeed;
        monster.rotationSpeed = monsterData.baserotationSpeed;
        monster.monsterController = monsterController;
    }
    public void WavePlus(int amount)
    {

    }

}



