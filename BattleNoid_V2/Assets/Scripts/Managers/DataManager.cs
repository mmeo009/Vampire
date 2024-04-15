using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using Supporter; // Supporter 네임스페이스를 사용
using System.Text;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class DataManager
{
    // 저장 파일 경로
    [SerializeField] private string saveFilePath;

    // 암호화 키
    private string key = "ThisIsAAWESOMEKey";

    // 플레이어 및 적 데이터
    [SerializeField] private Entity_Player entity_Player;
    [SerializeField] private Entity_Enemy entity_Enemy;
    [SerializeField] private Entity_Perk entity_Perk;

    // 플레이어 및 적 데이터를 담는 딕셔너리
    public Dictionary<int, Entity_Player.Param> playerDictionary = new Dictionary<int, Entity_Player.Param>();
    public Dictionary<int, Entity_Enemy.Param> enemyDictionary = new Dictionary<int, Entity_Enemy.Param>();
    public Dictionary<int, Entity_Perk.Param> perkDictionary = new Dictionary<int, Entity_Perk.Param>();

    // 게임 데이터
    [SerializeField] public GameData gameData { get; private set; }

    // 리소스 관련 데이터
    //=========================================================================================================

    Dictionary<string, UnityEngine.Object> resourcesDictionary = new Dictionary<string, UnityEngine.Object>();
    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (resourcesDictionary.TryGetValue(key, out UnityEngine.Object resource))
        {
            return resource as T;
        }

        if (typeof(T) == typeof(Sprite))
        {
            key = key + ".sprite";
            if (resourcesDictionary.TryGetValue(key, out UnityEngine.Object temp))
            {
                return temp as T;
            }
        }

        return null;
    }
    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
        }

        if (pooling)
            return Managers.Pool.Pop(prefab);

        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);

        go.name = prefab.name;
        return go;
    }
    public void LoadAsync<T>(string Key, Action<T> callback = null) where T : UnityEngine.Object
    {
        //스프라이트인 경우 하위객체의 이름으로 로드하면 스프라이트로 로딩이 됨 
        string loadKey = Key;
        if (Key.Contains(".sprite"))
            loadKey = $"{Key}[{Key.Replace(".sprite", "")}]";

        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);

        asyncOperation.Completed += (op) =>
        {
            //캐시확인 
            if (resourcesDictionary.TryGetValue(Key, out UnityEngine.Object resource))
            {
                callback?.Invoke(op.Result);
                return;
            }

            resourcesDictionary.Add(Key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var OpHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));

        OpHandle.Completed += (op) =>
        {
            int loadCount = 0;

            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                if (result.PrimaryKey.Contains(".sprite"))
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
                else
                {
                    LoadAsync<T>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
            }
        };
    }
    //=========================================================================================================

    // 기본 데이터 로드 처음 한번만 실행
    public async Task LoadBaseData<T>(string key) where T : UnityEngine.Object
    {
        // 저장 파일 경로 설정
        saveFilePath = Application.persistentDataPath + "/BattleNoidData.json";

        // 리소스에서 파일 로드
        var operation = Addressables.LoadAssetAsync<T>(key);
        // 대기해서 로드 기다리기
        await operation.Task;

        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            var loadedData = operation.Result;
            Debug.Log(loadedData.name);

            // 로드한 데이터 처리
            ProcessLoadedBaseData(loadedData);
        }
        else
        {
            Debug.LogError($"{key}를 가진 데이터가 존재하지 않습니다.");
        }
    }

    // 로드한 데이터 처리
    private void ProcessLoadedBaseData<T>(T loadedData) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(Entity_Player))
        {
            entity_Player = loadedData as Entity_Player;
            // 플레이어 데이터 딕셔너리에 추가
            AddPlayerDataToDictionary(entity_Player.param, playerDictionary);
        }
        else if (typeof(T) == typeof(Entity_Enemy))
        {
            entity_Enemy = loadedData as Entity_Enemy;
            // 적 데이터 딕셔너리에 추가
            AddEnemyDataToDictionary(entity_Enemy.param, enemyDictionary);
            Debug.Log(enemyDictionary[1].name);
        }
        else if (typeof(T) == typeof(Entity_Perk))
        {
            entity_Perk = loadedData as Entity_Perk;
            // 적 데이터 딕셔너리에 추가
            AddPerkDataToDictionary(entity_Perk.param, perkDictionary);
            Debug.Log(perkDictionary[1].name);
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
                Debug.Log(item.index + item.name);
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
                Debug.Log(item.index + item.name);
            }
            else
            {
                Debug.LogError($"이미 존재하는 인덱스: {item.index}");
            }
        }
    }

    // 플레이어 데이터를 딕셔너리에 추가
    private void AddPerkDataToDictionary(List<Entity_Perk.Param> data, Dictionary<int, Entity_Perk.Param> dictionary)
    {
        foreach (var item in data)
        {
            if (!dictionary.ContainsKey(item.index))
            {
                dictionary.Add(item.index, item);
                Debug.Log(item.index + item.name);
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
    public void SaveGameData(GameData data)
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
    public void LoadGameData()
    {
        GameData loadedData = new GameData();
        loadedData = LoadData();

        gameData = loadedData;
    }
    GameData LoadData()
    {
        if (File.Exists(saveFilePath))
        {
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
            aesAlg.Key = AdjustKeySize(key, 256); // 256 비트 (32 바이트)로 키 크기 조정
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
            aesAlg.Key = AdjustKeySize(key, 256); // 256 비트 (32 바이트)로 키 크기 조정
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

    // 데이터 사이즈 조절
    private byte[] AdjustKeySize(string key, int keySize)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        Array.Resize(ref keyBytes, keySize / 8); // 원하는 키 크기에 맞게 배열 크기 조정
        return keyBytes;
    }

    // 게임 데이터 변경
    public void ChangeGameData(DataType type, string dataKey = null, int intValue = 0, bool boolValue = false, OperationType operation = OperationType.Set)
    {
        if (gameData == null)
        {
            gameData = new GameData();
        }

        switch (type)
        {
            case DataType.NAME:
                ChangeName(dataKey);
                break;
            case DataType.MONEY:
                ChangeMoney(operation, intValue);
                break;
            case DataType.CHARACTER:
                ChangeData(gameData.characterData, dataKey, boolValue, intValue);
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
    private void ChangeMoney(OperationType operation, int amount)
    {
        switch (operation)
        {
            case OperationType.Plus:
                gameData.money += amount;
                break;
            case OperationType.Minus:
                gameData.money -= amount;
                if (gameData.money < 0)
                {
                    gameData.money = 0;
                }
                break;
            case OperationType.Set:
                gameData.money = amount;
                break;
            case OperationType.Reset:
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
