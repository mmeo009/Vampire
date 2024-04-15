using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using Supporter; // Supporter ���ӽ����̽��� ���
using System.Text;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class DataManager
{
    // ���� ���� ���
    [SerializeField] private string saveFilePath;

    // ��ȣȭ Ű
    private string key = "ThisIsAAWESOMEKey";

    // �÷��̾� �� �� ������
    [SerializeField] private Entity_Player entity_Player;
    [SerializeField] private Entity_Enemy entity_Enemy;
    [SerializeField] private Entity_Perk entity_Perk;

    // �÷��̾� �� �� �����͸� ��� ��ųʸ�
    public Dictionary<int, Entity_Player.Param> playerDictionary = new Dictionary<int, Entity_Player.Param>();
    public Dictionary<int, Entity_Enemy.Param> enemyDictionary = new Dictionary<int, Entity_Enemy.Param>();
    public Dictionary<int, Entity_Perk.Param> perkDictionary = new Dictionary<int, Entity_Perk.Param>();

    // ���� ������
    [SerializeField] public GameData gameData { get; private set; }

    // ���ҽ� ���� ������
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
        //��������Ʈ�� ��� ������ü�� �̸����� �ε��ϸ� ��������Ʈ�� �ε��� �� 
        string loadKey = Key;
        if (Key.Contains(".sprite"))
            loadKey = $"{Key}[{Key.Replace(".sprite", "")}]";

        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);

        asyncOperation.Completed += (op) =>
        {
            //ĳ��Ȯ�� 
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

    // �⺻ ������ �ε� ó�� �ѹ��� ����
    public async Task LoadBaseData<T>(string key) where T : UnityEngine.Object
    {
        // ���� ���� ��� ����
        saveFilePath = Application.persistentDataPath + "/BattleNoidData.json";

        // ���ҽ����� ���� �ε�
        var operation = Addressables.LoadAssetAsync<T>(key);
        // ����ؼ� �ε� ��ٸ���
        await operation.Task;

        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            var loadedData = operation.Result;
            Debug.Log(loadedData.name);

            // �ε��� ������ ó��
            ProcessLoadedBaseData(loadedData);
        }
        else
        {
            Debug.LogError($"{key}�� ���� �����Ͱ� �������� �ʽ��ϴ�.");
        }
    }

    // �ε��� ������ ó��
    private void ProcessLoadedBaseData<T>(T loadedData) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(Entity_Player))
        {
            entity_Player = loadedData as Entity_Player;
            // �÷��̾� ������ ��ųʸ��� �߰�
            AddPlayerDataToDictionary(entity_Player.param, playerDictionary);
        }
        else if (typeof(T) == typeof(Entity_Enemy))
        {
            entity_Enemy = loadedData as Entity_Enemy;
            // �� ������ ��ųʸ��� �߰�
            AddEnemyDataToDictionary(entity_Enemy.param, enemyDictionary);
            Debug.Log(enemyDictionary[1].name);
        }
        else if (typeof(T) == typeof(Entity_Perk))
        {
            entity_Perk = loadedData as Entity_Perk;
            // �� ������ ��ųʸ��� �߰�
            AddPerkDataToDictionary(entity_Perk.param, perkDictionary);
            Debug.Log(perkDictionary[1].name);
        }
        else
        {
            Debug.LogError("�ҷ��� �����͸� �����ϼ���!");
        }
    }

    // �÷��̾� �����͸� ��ųʸ��� �߰�
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
                Debug.LogError($"�̹� �����ϴ� �ε���: {item.index}");
            }
        }
    }

    // �� �����͸� ��ųʸ��� �߰�
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
                Debug.LogError($"�̹� �����ϴ� �ε���: {item.index}");
            }
        }
    }

    // �÷��̾� �����͸� ��ųʸ��� �߰�
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
                Debug.LogError($"�̹� �����ϴ� �ε���: {item.index}");
            }
        }
    }

    // ������ ��������
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
            Debug.LogError("�ε����� �ڵ带 �Է��ϼ���!");
            return default(T);
        }
    }

    // �ε����� ������ ��������
    private T GetDataByIndex<T>(Dictionary<int, T> dictionary, int index)
    {
        if (dictionary.ContainsKey(index))
        {
            return dictionary[index];
        }
        else
        {
            Debug.LogError($"�ε��� {index}�� �ش��ϴ� �����Ͱ� �����ϴ�!");
            return default(T);
        }
    }

    // �ڵ�� ������ ��������
    private T GetDataByCode<T>(Dictionary<int, T> dictionary, string code)
    {
        // FirstOrDefault(x => x(����)) ���ǰ� �����ϴ� ù��° ���� ã�ƿ��� �޼��� System.Linq;
        var pair = dictionary.FirstOrDefault(x => ((IEntityWithCode)x.Value).code == code);
        if (!pair.Equals(default(KeyValuePair<int, T>)))
        {
            return pair.Value;
        }
        else
        {
            Debug.LogError($"�ڵ� {code}�� �ش��ϴ� �����Ͱ� �����ϴ�!");
            return default(T);
        }
    }

    // ���� ������ ����
    public void SaveGameData(GameData data)
    {
        // JSON ����ȭ
        string jsonData = JsonConvert.SerializeObject(data);

        // �����͸� ����Ʈ �迭�� ��ȯ
        byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(jsonData);

        // ��ȣȭ
        byte[] encryptedBytes = Encrypt(bytesToEncrypt);

        // ��ȣȭ�� �����͸� Base64 ���ڿ��� ��ȯ
        string encryptedData = Convert.ToBase64String(encryptedBytes);

        // ���� ����
        File.WriteAllText(saveFilePath, encryptedData);
    }

    // ���� ������ �ҷ�����
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
            // ���Ͽ��� ��ȣȭ�� ������ �б�
            string encryptedData = File.ReadAllText(saveFilePath);

            // Base64���ڿ��� ����Ʈ �迭�� ��ȯ
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // ��ȣȭ
            byte[] decryptedBytes = Decrypt(encryptedBytes);

            // byte �迭�� ���ڿ��� ��ȯ
            string jsonData = Encoding.UTF8.GetString(decryptedBytes);

            // JSON ���� �� ����ȭ
            GameData data = JsonConvert.DeserializeObject<GameData>(jsonData);
            return data;
        }
        else
        {
            return null;
        }
    }

    // ������ ��ȣȭ
    byte[] Encrypt(byte[] plainBytes)
    {
        // AES ��ȣȭ �˰��� ����
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = AdjustKeySize(key, 256); // 256 ��Ʈ (32 ����Ʈ)�� Ű ũ�� ����
            aesAlg.IV = new byte[16];   // �ʱ�ȭ ����

            // ��ȣȭ ��ȯ�� ����
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // �޸� ��Ʈ�� ����
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // ��ȣȭ ��Ʈ�� ����
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // ������ ����
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    // ��ȣȭ�� �����͸� ����Ʈ �迭�� ��ȯ
                    return msEncrypt.ToArray();
                }
            }
        }
    }

    // ������ ��ȣȭ
    byte[] Decrypt(byte[] encryptedBytes)
    {
        // AES ��ȣȭ �˰��� ����
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = AdjustKeySize(key, 256); // 256 ��Ʈ (32 ����Ʈ)�� Ű ũ�� ����
            aesAlg.IV = new byte[16];   // �ʱ�ȭ ����

            // ��ȣȭ ��ȯ�� ����
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // �޸� ��Ʈ�� ����
            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            {
                // ��ȣȭ ��Ʈ�� ����
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    // ��ȣȭ�� �����͸� ���� ����Ʈ �迭 ����
                    byte[] decryptedBytes = new byte[encryptedBytes.Length];

                    // ������ �б�
                    int decryptedByteCount = csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);

                    // ������ ���� ũ�� ��ŭ�� ����Ʈ �迭�� ��ȯ
                    return decryptedBytes.Take(decryptedByteCount).ToArray();
                }
            }
        }
    }

    // ������ ������ ����
    private byte[] AdjustKeySize(string key, int keySize)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        Array.Resize(ref keyBytes, keySize / 8); // ���ϴ� Ű ũ�⿡ �°� �迭 ũ�� ����
        return keyBytes;
    }

    // ���� ������ ����
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
                Debug.LogError("�������� �ʴ� ������ Ÿ���̾�");
                break;
        }
    }

    // �̸� ����
    private void ChangeName(string newName)
    {
        gameData.myName = string.IsNullOrEmpty(newName) ? "IDontHaveAnyName!" : newName;
    }

    // �� ����
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
                Debug.LogError("���� �̿�?");
                break;
        }
    }

    // ������ ����
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
                    Debug.LogError("�������� �ʴ� ������ Ÿ���Դϴ�.");
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
                    Debug.LogError("�������� �ʴ� ������ Ÿ���Դϴ�.");
                }
                dictionary.Add(key, newData);
            }
        }
        else
        {
            Debug.LogError("Ű�� ����� �Է��ϼ���.");
        }
    }
}
