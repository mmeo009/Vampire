using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveController : MonoBehaviour
{
    #region PrivateVariables
   [SerializeField] private List<WaveData> waves;
    private float timer;
    private float nowTime;
    private Transform[] spawnPoints;
    private int thisWaveNumber;

    #endregion
    #region PublicVariables
    #endregion
    private void Update()
    {
        nowTime -= Time.deltaTime;
        if(nowTime <= 0)
        {
            SpawnMonster();
        }    
    }

    #region PrivateMethod
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

        if(waves != null)
        {
            waves.Clear();
        }

        waves = keys;

        nowTime = 10f;
    }

    private void SpawnMonster()
    {
        
    }
    #endregion

}
