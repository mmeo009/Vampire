using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using DataSupporter; // DataTypes ���ӽ����̽��� ����մϴ�.
using System.Text;

public class DataManager
{
    // ���� ���� ���
    public string saveFilePath;

    // ��ȣȭ Ű
    private string key = "ThisIsAWESOMEKey";

    // �÷��̾� �� �� ������
    public Entity_Player entity_Player;
    public Entity_Enemy entity_Enemy;

    // �÷��̾� �� �� �����͸� ��� ��ųʸ�
    public Dictionary<int, Entity_Player.Param> playerDictionary = new Dictionary<int, Entity_Player.Param>();
    public Dictionary<int, Entity_Enemy.Param> enemyDictionary = new Dictionary<int, Entity_Enemy.Param>();

    // ���� ������
    public GameData gameData { get; private set; }

    // �⺻ ������ �ε� ó�� �ѹ��� ����
    public void LoadBaseData<T>(string fileName) where T : UnityEngine.Object
    {
        // ���� ���� ��� ����
        saveFilePath = Application.persistentDataPath + "/BattleNoidData.json";

        // ���ҽ����� ���� �ε�
        T loadedData = Resources.Load<T>($"Excel/{fileName}");

        // �ε��� ������ ó��
        ProcessLoadedBaseData<T>(loadedData);
    }

    // �ε��� ������ ó��
    private void ProcessLoadedBaseData<T>(T loadedData) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(Entity_Player))
        {
            entity_Player = loadedData as Entity_Player;
            // �÷��̾� ������ ��ųʸ��� �߰�
            AddPlayerDataToDictionary(entity_Player.param, playerDictionary);
            Debug.Log(playerDictionary[1].name);
        }
        else if (typeof(T) == typeof(Entity_Enemy))
        {
            entity_Enemy = loadedData as Entity_Enemy;
            // �� ������ ��ųʸ��� �߰�
            AddEnemyDataToDictionary(entity_Enemy.param, enemyDictionary);
            Debug.Log(enemyDictionary[1].name);
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
    void SaveData(GameData data)
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
    GameData LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            // ���� �ȿ� ������ �б�

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
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
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
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
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

    // ���� ������ ����
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
                else if (data is PerkData perkData)
                {
                    perkData.hasThisPerk = value;
                    perkData.level = level;
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
                else if (newData is PerkData perkData)
                {
                    perkData.code = key;
                    perkData.hasThisPerk = value;
                    perkData.level = level;
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
