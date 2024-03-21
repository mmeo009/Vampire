using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public Entity_Player entity_Player;         // file name = playerData
    public Entity_Enemy entity_Enemy;           // file name = enemyData
    public Dictionary<int, Entity_Player.Param> playerDictionary = new Dictionary<int, Entity_Player.Param>();
    public Dictionary<int, Entity_Enemy.Param> enemyDictionary = new Dictionary<int, Entity_Enemy.Param>();

    public void Start()
    {
        LoadGameData<Entity_Player>("playerData");
        LoadGameData<Entity_Enemy>("enemyData");
    }
    public void LoadGameData<T>(string fileName) where T : UnityEngine.Object
    {
        // typeof 연산자를 사용하여 T의 형식을 가져옴
        string filePath = $"Excel/{fileName}";

        // 파일 로드
        T loadedData = Resources.Load<T>(filePath);

        if (typeof(T) == typeof(Entity_Player))
        {
            entity_Player = loadedData as Entity_Player;

            foreach (Entity_Player.Param playerData in entity_Player.param)
            {
                if (!playerDictionary.ContainsKey(playerData.index))
                {
                    playerDictionary.Add(playerData.index, playerData);
                }
                else
                {
                    Debug.LogError("이미 존재하는 플레이어 index: " + playerData.index);
                }
            }
            Debug.Log(playerDictionary[1].name);
        }
        else if (typeof(T) == typeof(Entity_Enemy))
        {
            entity_Enemy = loadedData as Entity_Enemy;

            foreach (Entity_Enemy.Param enemyData in entity_Enemy.param)
            {
                if (!enemyDictionary.ContainsKey(enemyData.index))
                {
                    enemyDictionary.Add(enemyData.index, enemyData);
                }
                else
                {
                    Debug.LogError("이미 존재하는 몬스터 index: " + enemyData.index);
                }
            }
            Debug.Log(enemyDictionary[1].name);
        }
        else
        {
            Debug.LogError("무엇을 불러오고 싶은게냐!");
        }


    }
}
