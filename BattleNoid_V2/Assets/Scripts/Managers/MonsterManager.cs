using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using System.Linq;
using static UnityEngine.Rendering.HDROutputUtils;

[System.Serializable]
public class MonsterManager
{
    public HashSet<MonsterController> monsters = new HashSet<MonsterController>();

    public Dictionary<string, WaveData> waveDatas;

    public void GetWaveDatas()
    {
        waveDatas = new Dictionary<string, WaveData>();
        Managers.Data.LoadAllAsync<WaveData>("Wave", (key, count, totalCount) =>
        {
            Debug.Log("key : " + key + " Count : " + count + " totalCount : " + totalCount);
            var data = Managers.Data.Load<WaveData>(key);
            waveDatas.Add($"S{data.stageData.stageNumber}N{data.stageData.waveNumber}", data);
            Debug.Log(waveDatas.ContainsValue(data));
        });
    }

    public void CreateMonster(Transform pos, int monsterIndex, string monsterCode = null, float hp = 0, float damage = 0, float speed = 0)
    {
        Entity_Enemy.Param monster = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary, monsterIndex, monsterCode);

        if(monster != null)
        {
            if(monsterCode == null)
            {
                monsterCode = monster.code;
            }

            var monsterObject = Managers.Data.Instantiate(monsterCode, null, true);
            monsterObject.transform.position = pos.position;
            var mc = monsterObject.GetComponent<MonsterController>();
            LoadMonsterData(monster, mc);
            mc.ChangeMonsterStats(OperationType.Plus,StatType.MAXHP, hp);
            mc.ChangeMonsterStats(OperationType.Plus, StatType.AttackDamage, damage);
            mc.ChangeMonsterStats(OperationType.Plus, StatType.MoveSpeed, speed);
            monsters.Add(mc);
        }
    }

    private void LoadMonsterData(Entity_Enemy.Param monsterData, MonsterController monsterController)
    {
        MonsterStats monster = new MonsterStats();
        monster.code = monsterData.code;
        monster.hp = monsterData.baseHp;
        monster.knockBackAmount = monsterData.knockBackAmount;
        monster.knockBackTime = monsterData.knockBackTime;
        monster.attackDamage = monsterData.baseDamage;
        monster.attackRange = monsterData.baseRange;
        monster.attackSpeed = monsterData.attackSpeed;
        monster.moveSpeed = monsterData.baseMoveSpeed;
        monster.rotationSpeed = monsterData.baserotationSpeed;
        monster.attackType = monsterData.attackType;
        monster.viewingAngle = monsterData.viewingAngle;
        monster.monsterController = monsterController;
        monsterController.GetMonsterStats(monster);
    }

}



