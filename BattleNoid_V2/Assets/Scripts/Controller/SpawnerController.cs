using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerController   : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] private int waveNum;
    [SerializeField] private float timer;
    [SerializeField] private int monsterIndex;
    #endregion
    #region PublicVariables
    public float radius;
    #endregion
    private void OnEnable()
    {
        waveNum = Random.Range(0, Managers.Data.enemyDictionary.Count);
        SpawnMonster();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), radius);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(GetSpawnPos(radius), 0.2f);
    }

    private Vector3 GetSpawnPos(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        Vector3 pointOnCircle = new Vector3(transform.position.x + x, transform.position.y + 1, transform.position.z + z);

        return pointOnCircle;
    }
    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                SpawnMonster();
            }
        }
    }
    #region PrivateMethod

    private void SpawnMonster()
    {
        if(Managers.Monster.monsters.Count >= 90)
        {
            return;
        }

        Vector3 pos = GetSpawnPos(radius);
        int playerLevel = Managers.Player.player.level / 3;
        int spawnAmount = Random.Range(0,100 - Managers.Monster.spawnedMonsterAmount);

        for(int i = 0;i < spawnAmount; i++)
        {
            Managers.Monster.CreateMonster(pos, monsterIndex, null, playerLevel);
            Managers.Monster.spawnedMonsterAmount++;
        }

        timer = spawnAmount * 5;
    }
    #endregion
    #region PublicMethod

    #endregion
}
