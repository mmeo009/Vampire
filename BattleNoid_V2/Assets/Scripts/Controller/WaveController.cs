using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveController : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private WaveData nowWaveData;
    private float timer;
    private Transform[] spawnPoints;
    private int nowWaveIndex;

    #endregion
    #region PublicVariables
    #endregion
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnMonster();
            }
        }
    }

    #region PrivateMethod
    private void SpawnMonster()
    {
        if(nowWaveData != waves[0])
        {
            nowWaveIndex += 1;
            nowWaveData = waves[nowWaveIndex];
        }

        for (int i = 0; i < nowWaveData.monsters.Count; i++)
        {
            for (int j = 0; j < nowWaveData.monsters[i].monsterAmount; j++)
            {
                Managers.Monster.CreateMonster(spawnPoints[Random.Range(0, spawnPoints.Length)], nowWaveData.monsters[i].monsterIndex,
                null, nowWaveData.monsters[i].additionalHp, nowWaveData.monsters[i].additionalDamage, nowWaveData.monsters[i].additionalMoveSpeed);
            }
        }

        timer = nowWaveData.timeToNextWave;
    }
    #endregion
    #region PublicMethod
    public void LoadWaveData(int stage)
    {
        if (spawnPoints != null)
        {
            spawnPoints = null;
        }

        var _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = new Transform[_spawnPoints.Length];

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            spawnPoints[i] = _spawnPoints[i].transform;
        }

        List<WaveData> keys = new List<WaveData>();

        foreach (var key in Managers.Monster.waveDatas.Keys)
        {
            string extracted = key.Substring(1, key.IndexOf('N') - 1);
            if (extracted == stage.ToString())
            {
                keys.Add(Managers.Monster.waveDatas[key]);
            }
        }

        if (waves != null)
        {
            waves.Clear();
        }

        waves = keys.OrderBy(num => num.stageData.waveNumber).ToList();

        nowWaveIndex = 0;
        nowWaveData = waves[nowWaveIndex];
        SpawnMonster();
        timer = nowWaveData.timeToNextWave;
    }
    #endregion
}
