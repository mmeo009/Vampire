using System.Collections.Generic;

namespace DataSupporter
{
    // IEntityWithCode는 코드(code) 속성을 가져야 하는 인터페이스. 다양한 클래스에서 공통적으로 가지고 있는 string 형식의 code를 사용하기 위해 사용
    public interface IEntityWithCode
    {
        // 코드를 나타내는 읽기 전용 속성
        string code { get; }
    }
    public class Util
    {

    }
    [System.Serializable]
    public class Enums
    {
        public enum PlayerType
        {
            TestPlayer
        }

        public enum EventType
        {
            NONE,
            GoToBattle = 100,
            CheckSTR = 1000,
            CheckDEX,
            CheckCON,
            CheckINT,
            CheckWIS,
            CheckCHA
        }

        public enum ResultType
        {
            ChangeHp,
            ChangeSp,
            AddExperience = 100,
            GoToShop = 1000,
            GoToNextStory = 2000,
            GoToRandomStory = 3000,
            GoToEnding = 10000
        }
        public enum DataType
        {
            NAME,
            MONEY,
            CHARACTER,
            PERK,
            PLAYER,
            MONSTER
        }
        public enum Operation
        {
            Plus,
            Minus,
            Set,
            Reset
        }
    }

    [System.Serializable]
    public class Stats
    {
        // 체력과 정신력
        public int hpPoint;
        public int spPoint;

        public int currentHpPoint;
        public int currentSpPoint;
        public int currentXpPoint;

        // 기본 스텟 설정(Ex D&D)
        public int strength;        //STR 힘
        public int dexterity;       //DEX 민첩
        public int consitiution;    //CON 건강
        public int Intelligence;    //INT 지능
        public int wisdom;          //WIS 지혜
        public int charisma;        //CHA 매력
    }
    [System.Serializable]
    public class GameData
    {
        public string myName;
        public int money;
        public Dictionary<string, CharacterData> characterData = new Dictionary<string, CharacterData>();
        public Dictionary<string, PerkData> perkData = new Dictionary<string, PerkData>();
    }
    [System.Serializable]
    public class CharacterData
    {
        public string code;
        public bool hasThisCharacter;
        public int level;
    }
    [System.Serializable]
    public class PerkData
    {
        public string code;
        public bool hasThisPerk;
        public int level;
    }
}
