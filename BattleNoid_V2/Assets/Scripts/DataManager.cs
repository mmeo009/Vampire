using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;             // XML 사용하기 위한
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
        // 파일 이름을 통해 파일의 경로를 저장함
        string filePath = $"Excel/{fileName}";

        // 파일 경로에서 파일을 로드해 옴
        T loadedData = Resources.Load<T>(filePath);

        // 만약 타입이 Entity_Player일 경우
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
        // 만약 타입이 Entity_Enemy일 경우
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
                                return "플레이어는 이름이 없다!";
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
                                return "플레이어는 이름이 없다!";
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
                                return "이 친구는 몬스터야";
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
        FileStream stream = new FileStream(saveFilePath, FileMode.Create);           // 파일 스트림 함수로 파일 생성
        serializer.Serialize(stream, dataToBeStored);                                // 클래스 -> XML 변환 후 저장
        stream.Close();
    }

    public GameData LoadGmaeData()
    {
        if (File.Exists(saveFilePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            FileStream stream = new FileStream(saveFilePath, FileMode.Open);        // 파일 읽기 모드로 파일 열기
            GameData data = (GameData)serializer.Deserialize(stream);               // XML -> 클래스 읽어서 변환
            stream.Close();                                                         // 다 불러온 후 닫기
            return data;
        }
        else
        {
            return null;
        }
    }
    public void ChangeGameData(DataType type, string _string = null, int _int = 0, bool _bool = false)
    {
        // type : 수행 방식
        // _string : 이름, 돈(Plus,Minus,Set,Reset), 캐릭터 혹은 퍽의 코드
        // _int : 돈의 추가량, 캐릭터 혹은 퍽의 레벨
        // _bool : 캐릭터 혹은 퍽의 보유 여부
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
                Debug.LogWarning("이름이 기입되지 않았습니다.");
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
                Debug.LogError("해당 작업을 수행할 수 없습니다. : 돈 데이터 변경");
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
                Debug.LogError("해당 작업을 수행할 수 없습니다. : 캐릭터 데이터 추가");
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
                Debug.LogError("해당 작업을 수행할 수 없습니다. : 퍽 데이터 추가");
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
