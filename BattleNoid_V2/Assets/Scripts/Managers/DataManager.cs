using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using DataSupporter; // DataTypes 네임스페이스를 사용합니다.
using System.Text;

public class DataManager
{
    // 저장 파일 경로
    public string saveFilePath;

    // 암호화 키
    private string key = "ThisIsAWESOMEKey";

    // 플레이어 및 적 데이터
    public Entity_Player entity_Player;
    public Entity_Enemy entity_Enemy;

    // 플레이어 및 적 데이터를 담는 딕셔너리
    public Dictionary<int, Entity_Player.Param> playerDictionary = new Dictionary<int, Entity_Player.Param>();
    public Dictionary<int, Entity_Enemy.Param> enemyDictionary = new Dictionary<int, Entity_Enemy.Param>();

    // 게임 데이터
    public GameData gameData { get; private set; }

    // 기본 데이터 로드 처음 한번만 실행
    public void LoadBaseData<T>(string fileName) where T : UnityEngine.Object
    {
        // 저장 파일 경로 설정
        saveFilePath = Application.persistentDataPath + "/BattleNoidData.json";

        // 리소스에서 파일 로드
        T loadedData = Resources.Load<T>($"Excel/{fileName}");

        // 로드한 데이터 처리
        ProcessLoadedBaseData<T>(loadedData);
    }

    // 로드한 데이터 처리
    private void ProcessLoadedBaseData<T>(T loadedData) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(Entity_Player))
        {
            entity_Player = loadedData as Entity_Player;
            // 플레이어 데이터 딕셔너리에 추가
            AddPlayerDataToDictionary(entity_Player.param, playerDictionary);
            Debug.Log(playerDictionary[1].name);
        }
        else if (typeof(T) == typeof(Entity_Enemy))
        {
            entity_Enemy = loadedData as Entity_Enemy;
            // 적 데이터 딕셔너리에 추가
            AddEnemyDataToDictionary(entity_Enemy.param, enemyDictionary);
            Debug.Log(enemyDictionary[1].name);
        }
        else
        {
            Debug.LogError("불러올 데이터를 선택하세요!");
        }
    }

    // 플레이어 데이터를 딕셔너리에 추가
    private void AddPlayerDataToDictionary(List<Entity_Player.Param> data, Dictionary<int, Entity_Player.Param> dictionary)
    {
        foreach (var item in data)
        {
            if (!dictionary.ContainsKey(item.index))
            {
                dictionary.Add(item.index, item);
            }
            else
            {
                Debug.LogError($"이미 존재하는 인덱스: {item.index}");
            }
        }
    }

    // 적 데이터를 딕셔너리에 추가
    private void AddEnemyDataToDictionary(List<Entity_Enemy.Param> data, Dictionary<int, Entity_Enemy.Param> dictionary)
    {
        foreach (var item in data)
        {
            if (!dictionary.ContainsKey(item.index))
            {
                dictionary.Add(item.index, item);
            }
            else
            {
                Debug.LogError($"이미 존재하는 인덱스: {item.index}");
            }
        }
    }

    // 데이터 가져오기
    public T GetDataFromDictionary<T>(Dictionary<int, T> dictionary, int index, string code = null)
    {
        if (index > 0)
        {
            return GetDataByIndex(dictionary, index);
        }
        else if (code != null)
        {
            return GetDataByCode(dictionary, code);
        }
        else
        {
            Debug.LogError("인덱스나 코드를 입력하세요!");
            return default(T);
        }
    }

    // 인덱스로 데이터 가져오기
    private T GetDataByIndex<T>(Dictionary<int, T> dictionary, int index)
    {
        if (dictionary.ContainsKey(index))
        {
            return dictionary[index];
        }
        else
        {
            Debug.LogError($"인덱스 {index}에 해당하는 데이터가 없습니다!");
            return default(T);
        }
    }

    // 코드로 데이터 가져오기
    private T GetDataByCode<T>(Dictionary<int, T> dictionary, string code)
    {
        // FirstOrDefault(x => x(조건)) 조건과 부합하는 첫번째 값을 찾아오는 메서드 System.Linq;
        var pair = dictionary.FirstOrDefault(x => ((IEntityWithCode)x.Value).code == code);
        if (!pair.Equals(default(KeyValuePair<int, T>)))
        {
            return pair.Value;
        }
        else
        {
            Debug.LogError($"코드 {code}에 해당하는 데이터가 없습니다!");
            return default(T);
        }
    }

    // 게임 데이터 저장
    void SaveData(GameData data)
    {
        // JSON 직렬화
        string jsonData = JsonConvert.SerializeObject(data);

        // 데이터를 바이트 배열로 변환
        byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(jsonData);

        // 암호화
        byte[] encryptedBytes = Encrypt(bytesToEncrypt);

        // 암호화된 데이터를 Base64 문자열로 변환
        string encryptedData = Convert.ToBase64String(encryptedBytes);

        // 파일 저장
        File.WriteAllText(saveFilePath, encryptedData);
    }

    // 게임 데이터 불러오기
    GameData LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            // 파일 안에 데이터 읽기

            // 파일에서 암호화된 데이터 읽기
            string encryptedData = File.ReadAllText(saveFilePath);

            // Base64문자열을 바이트 배열로 변환
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // 복호화
            byte[] decryptedBytes = Decrypt(encryptedBytes);

            // byte 배열을 문자열로 변환
            string jsonData = Encoding.UTF8.GetString(decryptedBytes);

            // JSON 파일 역 직렬화
            GameData data = JsonConvert.DeserializeObject<GameData>(jsonData);
            return data;
        }
        else
        {
            return null;
        }
    }

    // 데이터 암호화
    byte[] Encrypt(byte[] plainBytes)
    {
        // AES 암호화 알고리즘 생성
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16];   // 초기화 벡터

            // 암호화 변환기 생성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // 메모리 스트림 생성
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // 암호화 스트림 생성
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // 데이터 쓰기
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    // 암호화된 데이터를 바이트 배열로 반환
                    return msEncrypt.ToArray();
                }
            }
        }
    }

    // 데이터 복호화
    byte[] Decrypt(byte[] encryptedBytes)
    {
        // AES 복호화 알고리즘 생성
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16];   // 초기화 벡터

            // 복호화 변환기 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // 메모리 스트림 생성
            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            {
                // 복호화 스트림 생성
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    // 복호화된 데이터를 담을 바이트 배열 생성
                    byte[] decryptedBytes = new byte[encryptedBytes.Length];

                    // 데이터 읽기
                    int decryptedByteCount = csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);

                    // 실제로 읽힌 크기 만큼의 바이트 배열을 반환
                    return decryptedBytes.Take(decryptedByteCount).ToArray();
                }
            }
        }
    }

    // 게임 데이터 변경
    public void ChangeGameData(Enums.DataType type, string dataKey = null, int intValue = 0, bool boolValue = false, Enums.Operation operation = Enums.Operation.Set)
    {
        if (gameData == null)
        {
            gameData = new GameData();
        }

        switch (type)
        {
            case Enums.DataType.NAME:
                ChangeName(dataKey);
                break;
            case Enums.DataType.MONEY:
                ChangeMoney(operation, intValue);
                break;
            case Enums.DataType.CHARACTER:
                ChangeData(gameData.characterData, dataKey, boolValue, intValue);
                break;
            case Enums.DataType.PERK:
                ChangeData(gameData.perkData, dataKey, boolValue, intValue);
                break;
            default:
                Debug.LogError("존재하지 않는 데이터 타입이야");
                break;
        }
    }

    // 이름 변경
    private void ChangeName(string newName)
    {
        gameData.myName = string.IsNullOrEmpty(newName) ? "IDontHaveAnyName!" : newName;
    }

    // 돈 변경
    private void ChangeMoney(Enums.Operation operation, int amount)
    {
        switch (operation)
        {
            case Enums.Operation.Plus:
                gameData.money += amount;
                break;
            case Enums.Operation.Minus:
                gameData.money -= amount;
                if (gameData.money < 0)
                {
                    gameData.money = 0;
                }
                break;
            case Enums.Operation.Set:
                gameData.money = amount;
                break;
            case Enums.Operation.Reset:
                gameData.money = 0;
                break;
            default:
                Debug.LogError("돈이 미워?");
                break;
        }
    }

    // 데이터 변경
    private void ChangeData<T>(Dictionary<string, T> dictionary, string key, bool value, int level) where T : new()
    {
        if (!string.IsNullOrEmpty(key))
        {
            if (dictionary.ContainsKey(key))
            {
                T data = dictionary[key];
                if (data is CharacterData characterData)
                {
                    characterData.hasThisCharacter = value;
                    characterData.level = level;
                }
                else if (data is PerkData perkData)
                {
                    perkData.hasThisPerk = value;
                    perkData.level = level;
                }
                else
                {
                    Debug.LogError("지원되지 않는 데이터 타입입니다.");
                }
            }
            else
            {
                T newData = new T();
                if (newData is CharacterData characterData)
                {
                    characterData.code = key;
                    characterData.hasThisCharacter = value;
                    characterData.level = level;
                }
                else if (newData is PerkData perkData)
                {
                    perkData.code = key;
                    perkData.hasThisPerk = value;
                    perkData.level = level;
                }
                else
                {
                    Debug.LogError("지원되지 않는 데이터 타입입니다.");
                }
                dictionary.Add(key, newData);
            }
        }
        else
        {
            Debug.LogError("키를 제대로 입력하세요.");
        }
    }
}
