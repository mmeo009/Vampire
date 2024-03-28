using System.Collections.Generic;
using UnityEngine;

namespace Supporter
{
    // IEntityWithCode는 코드(code) 속성을 가져야 하는 인터페이스. 다양한 클래스에서 공통적으로 가지고 있는 string 형식의 code를 사용하기 위해 사용
    public interface IEntityWithCode
    {
        // 코드를 나타내는 읽기 전용 속성
        string code { get; }
    }
    public class Util
    {
        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();

            return component;
        }
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null) return null;

            return transform.gameObject;
        }
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null)
                return null;

            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }
            return null;
        }
    }
    [System.Serializable]
    public class Enums
    {
        public enum PlayerType
        {
            TestPlayer
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

        public enum Skill
        {
            TestPlayer_Default,
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
    [System.Serializable]
    public class MonststerStats
    {
        public Entity_Enemy entity_Enemy;
        public string code;
        public string name;
        public double hp;
        public double damage;
        public double moveSpeed;
    }
}
