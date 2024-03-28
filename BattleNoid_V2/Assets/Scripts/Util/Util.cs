using System.Collections.Generic;

namespace DataSupporter
{
    // IEntityWithCode�� �ڵ�(code) �Ӽ��� ������ �ϴ� �������̽�. �پ��� Ŭ�������� ���������� ������ �ִ� string ������ code�� ����ϱ� ���� ���
    public interface IEntityWithCode
    {
        // �ڵ带 ��Ÿ���� �б� ���� �Ӽ�
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
        // ü�°� ���ŷ�
        public int hpPoint;
        public int spPoint;

        public int currentHpPoint;
        public int currentSpPoint;
        public int currentXpPoint;

        // �⺻ ���� ����(Ex D&D)
        public int strength;        //STR ��
        public int dexterity;       //DEX ��ø
        public int consitiution;    //CON �ǰ�
        public int Intelligence;    //INT ����
        public int wisdom;          //WIS ����
        public int charisma;        //CHA �ŷ�
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
