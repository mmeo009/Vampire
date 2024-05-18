using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveController : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] private int waveNum;

    [SerializeField] private float timer;
    [SerializeField] private Transform[] spawnPoints;
    #endregion
    #region PublicVariables

    #endregion
    private void OnEnable()
    {
        if(spawnPoints == null) 
        {
            spawnPoints = FindSpawnPoints();
            SpawnMonster();
        }
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
    private Transform[] FindSpawnPoints()
    {
        var _points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        var tempSpawnPoints = new Transform[_points.Length];

        for (int i = 0; i < _points.Length; i++)
        {
            tempSpawnPoints[i] = _points[i].transform;
        }
        return tempSpawnPoints;
    }

    private void SpawnMonster()
    {
        int spawnPoint = Random.Range(0,spawnPoints.Length);
        int playerLevel = Managers.Player.player.level / 3;
        int monsterIndex = Random.Range(0, Managers.Data.enemyDictionary.Count);
        int spawnAmount = Random.Range(0,100 - Managers.Monster.spawnedMonsterAmount);

        for(int i = 0;i < spawnAmount; i++)
        {
            Managers.Monster.CreateMonster(spawnPoints[spawnPoint], monsterIndex, null, playerLevel);
            Managers.Monster.spawnedMonsterAmount++;
        }

        timer = spawnAmount * 5;
    }
    #endregion
    #region PublicMethod

    #endregion
}
