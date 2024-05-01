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
    [HideInInspector] public Transform[] SpawnPoints
    {
        get
        {
            var _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            spawnPoints = new Transform[_spawnPoints.Length];

            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                spawnPoints[i] = _spawnPoints[i].transform;
            }
            return spawnPoints;
        }
    }


    #endregion
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
