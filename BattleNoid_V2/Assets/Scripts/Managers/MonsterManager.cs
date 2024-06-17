using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using System.Linq;
using static UnityEngine.Rendering.HDROutputUtils;

[System.Serializable]
public class MonsterManager
{
    [HideInInspector] public int spawnedMonsterAmount;

    public HashSet<MonsterController> monsters = new HashSet<MonsterController>();

    [SerializeField] private List<SpawnerController> spawnerControllers = new List<SpawnerController>();

    [SerializeField] private float timer = 10f;
    [SerializeField] private float maxTime = 30f;

    public void CreateMonster(Vector3 pos, int monsterIndex, string monsterCode = null, int playerLevel = 0)
    {
        Entity_Enemy.Param monster = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary, monsterIndex, monsterCode);

        if(monster != null)
        {
            if(monsterCode == null)
            {
                monsterCode = monster.code;
            }

            var monsterObject = Managers.Data.Instantiate(monsterCode, null, true);
            monsterObject.transform.position = pos;
            var mc = monsterObject.GetComponent<MonsterController>();
            LoadMonsterData(monster, mc);
            mc.ChangeMonsterStats(OperationType.Plus,StatType.MAXHP, playerLevel * monster.additonalHp);
            mc.ChangeMonsterStats(OperationType.Plus, StatType.AttackDamage, playerLevel * monster.additionalDamage);
            mc.ChangeMonsterStats(OperationType.Plus, StatType.MoveSpeed, playerLevel * monster.additionalMoveSpeed);
            mc.Player = Managers.Player.player.playerController;
            monsters.Add(mc);
        }
    }

    public void CreateSpawner()
    {
        if(Managers.Player.player.playerController != null)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if(timer <= 0)
            {
                if (spawnerControllers.Count < 7)
                {
                    GameObject temp = Managers.Data.Instantiate("Spawner");
                    temp.transform.position = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
                    spawnerControllers.Add(temp.GetComponent<SpawnerController>());
                    timer = maxTime;
                }
            }
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



