using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;             // XML ����ϱ� ����
using System.IO;
using UnityEngine;


public class DataManager
{
    public enum DataType
    {
        NAME,
        MONEY,
        CHARACTER,
        PERK,
        PLAYER,
        MONSTER
    }

    public enum CharacterDataType
    {
        HP,
        NAME,
        DMG,
        SPEED,
        RANGE,
        STAGE
    }

    public string saveFilePath;

    public Entity_Player entity_Player;         // file name = playerData
    public Entity_Enemy entity_Enemy;           // file name = enemyData

    public Dictionary<int, Entity_Player.Param> playerDictionary = new Dictionary<int, Entity_Player.Param>();
    public Dictionary<int, Entity_Enemy.Param> enemyDictionary = new Dictionary<int, Entity_Enemy.Param>();
    public GameData gameData { get; private set; }
    public void Start()
    {
        LoadBaseData<Entity_Player>("playerData");
        LoadBaseData<Entity_Enemy>("enemyData");
    }
    public void LoadBaseData<T>(string fileName) where T : UnityEngine.Object
    {
        saveFilePath = Application.persistentDataPath + "/GameData.Xml";
        // ���� �̸��� ���� ������ ��θ� ������
        string filePath = $"Excel/{fileName}";

        // ���� ��ο��� ������ �ε��� ��
        T loadedData = Resources.Load<T>(filePath);

        // ���� Ÿ���� Entity_Player�� ���
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
                    Debug.LogError("�̹� �����ϴ� �÷��̾� index: " + playerData.index);
                }
            }
            Debug.Log(playerDictionary[1].name);
        }
        // ���� Ÿ���� Entity_Enemy�� ���
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
                    Debug.LogError("�̹� �����ϴ� ���� index: " + enemyData.index);
                }
            }
            Debug.Log(enemyDictionary[1].name);
        }
        else
        {
            Debug.LogError("������ �ҷ����� �����Գ�!");
        }
    }

    public string FindFromDictionary (DataType playerOrMonster, int indexNum, CharacterDataType dataType, bool isAdditional = false)
    {
            if(indexNum > 0)
            {
                if(playerOrMonster == DataType.PLAYER)
                {
                    if (isAdditional == false)
                    {
                        switch (dataType)
                        {
                            case CharacterDataType.NAME:
                                return "�÷��̾�� �̸��� ����!";
                            case CharacterDataType.STAGE:
                                return "0";
                            case CharacterDataType.HP:
                                return playerDictionary[indexNum].baseHp.ToString();
                            case CharacterDataType.DMG:
                                return playerDictionary[indexNum].baseDamage.ToString();
                            case CharacterDataType.SPEED:
                                return playerDictionary[indexNum].baseMoveSpeed.ToString();
                            case CharacterDataType.RANGE:
                                return playerDictionary[indexNum].baseRange.ToString();
                        }
                    }
                    else
                    {
                        switch (dataType)
                        {
                            case CharacterDataType.NAME:
                                return "�÷��̾�� �̸��� ����!";
                            case CharacterDataType.STAGE:
                                return "0";
                            case CharacterDataType.HP:
                                return playerDictionary[indexNum].additionalHp.ToString();
                            case CharacterDataType.DMG:
                                return playerDictionary[indexNum].additionalDamage.ToString();
                            case CharacterDataType.SPEED:
                                return playerDictionary[indexNum].additionalMoveSpeed.ToString();
                            case CharacterDataType.RANGE:
                                return playerDictionary[indexNum].additionalRange.ToString();
                        }
                    }
                }
                else if(playerOrMonster == DataType.MONSTER)
                {
                    if (isAdditional == false)
                    {
                        switch (dataType)
                        {
                            case CharacterDataType.NAME:
                                return enemyDictionary[indexNum].name;
                            case CharacterDataType.STAGE:
                                return enemyDictionary[indexNum].stage.ToString();
                            case CharacterDataType.HP:
                                return enemyDictionary[indexNum].baseHp.ToString();
                            case CharacterDataType.DMG:
                                return enemyDictionary[indexNum].baseDamage.ToString();
                            case CharacterDataType.SPEED:
                                return enemyDictionary[indexNum].baseMoveSpeed.ToString();
                            case CharacterDataType.RANGE:
                                return "�� ģ���� ���;�";
                        }
                    }
                    else
                    {
                        switch (dataType)
                        {
                            case CharacterDataType.NAME:
                                return enemyDictionary[indexNum].name;
                            case CharacterDataType.STAGE:
                                return enemyDictionary[indexNum].stage.ToString();
                            case CharacterDataType.HP:
                                return enemyDictionary[indexNum].additionalHp.ToString();
                            case CharacterDataType.DMG:
                                return enemyDictionary[indexNum].additionalDamage.ToString();
                            case CharacterDataType.SPEED:
                                return enemyDictionary[indexNum].additionalMoveSpeed.ToString();
                            case CharacterDataType.RANGE:
                                return enemyDictionary[indexNum].additionalRange.ToString();
                        }
                    }
                }
            }
            else
            {

            }
        return null;
    }

    public void SaveGameData(GameData dataToBeStored)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream stream = new FileStream(saveFilePath, FileMode.Create);           // ���� ��Ʈ�� �Լ��� ���� ����
        serializer.Serialize(stream, dataToBeStored);                                // Ŭ���� -> XML ��ȯ �� ����
        stream.Close();
    }

    public GameData LoadGmaeData()
    {
        if (File.Exists(saveFilePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            FileStream stream = new FileStream(saveFilePath, FileMode.Open);        // ���� �б� ���� ���� ����
            GameData data = (GameData)serializer.Deserialize(stream);               // XML -> Ŭ���� �о ��ȯ
            stream.Close();                                                         // �� �ҷ��� �� �ݱ�
            return data;
        }
        else
        {
            return null;
        }
    }
    public void ChangeGameData(DataType type, string _string = null, int _int = 0, bool _bool = false)
    {
        // type : ���� ���
        // _string : �̸�, ��(Plus,Minus,Set,Reset), ĳ���� Ȥ�� ���� �ڵ�
        // _int : ���� �߰���, ĳ���� Ȥ�� ���� ����
        // _bool : ĳ���� Ȥ�� ���� ���� ����
        if(gameData == null)
        {
            gameData = new GameData();
        }
        if(type == DataType.NAME)
        {
            if (_string != null)
            {
                gameData.myName = _string;
            }
            else
            {
                Debug.LogWarning("�̸��� ���Ե��� �ʾҽ��ϴ�.");
                gameData.myName = "IDontHaveAnyName";
            }
        }
        else if(type == DataType.MONEY)
        {
            if(_string != null)
            {
                if(_string == "Plus"|| _string == "Minus")
                {
                    gameData.money += _int;
                }
                else if(_string == "Set")
                {
                    gameData.money = _int;
                }
                else if(_string == "Reset")
                {
                    gameData.money = 0;
                }
                if(gameData.money < 0)
                {
                    gameData.money = 0;
                }
            }
            else
            {
                Debug.LogError("�ش� �۾��� ������ �� �����ϴ�. : �� ������ ����");
            }
        }
        else if(type == DataType.CHARACTER)
        {
            if (_string != null)
            {
                if (gameData.characterData[_string] != null)
                {
                    gameData.characterData[_string].hasThisCharacter = _bool;
                    gameData.characterData[_string].level = _int;
                }
                else
                {
                    CharacterData character = new CharacterData();
                    character.code = _string;
                    character.hasThisCharacter = _bool;
                    character.level = _int;
                    gameData.characterData.Add(_string, character);
                }
            }
            else
            {
                Debug.LogError("�ش� �۾��� ������ �� �����ϴ�. : ĳ���� ������ �߰�");
            }
        }
        else if(type == DataType.PERK)
        {
            if (_string != null)
            {
                if (gameData.perkData[_string] != null)
                {
                    gameData.perkData[_string].hasThisPerk = _bool;
                    gameData.perkData[_string].level = _int;
                }
                else
                {
                    PerkData perk = new PerkData();
                    perk.code = _string;
                    perk.hasThisPerk = _bool;
                    perk.level = _int;
                    gameData.perkData.Add(_string, perk);
                }
            }
            else
            {
                Debug.LogError("�ش� �۾��� ������ �� �����ϴ�. : �� ������ �߰�");
            }
        }
    }
}

public class GameData
{
    public string myName;
    public int money;
    public Dictionary<string, CharacterData> characterData = new Dictionary<string, CharacterData>();
    public Dictionary<string, PerkData> perkData = new Dictionary<string, PerkData>();
}
public class CharacterData
{
    public string code;
    public bool hasThisCharacter;
    public int level;
}
public class PerkData
{
    public string code;
    public bool hasThisPerk;
    public int level;
}
