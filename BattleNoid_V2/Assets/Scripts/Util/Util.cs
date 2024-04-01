using System.Collections.Generic;
using UnityEngine;

namespace Supporter
{
    // IEntityWithCode�� �ڵ�(code) �Ӽ��� ������ �ϴ� �������̽�. �پ��� Ŭ�������� ���������� ������ �ִ� string ������ code�� ����ϱ� ���� ���
    public interface IEntityWithCode
    {
        // �ڵ带 ��Ÿ���� �б� ���� �Ӽ�
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
        [SerializeField]
        public enum ActonType
        {
            SceneMove,
            ExitGame,
            PauseGame,
            SaveGame,
            LoadGame,
            DefaultAction
        }
        [SerializeField]
        public enum PlayerType
        {
            TestPlayer
        }
        [SerializeField]
        public enum DataType
        {
            NAME,
            MONEY,
            CHARACTER,
            PERK,
            PLAYER,
            MONSTER
        }
        [SerializeField]
        public enum Operation
        {
            Plus,
            Minus,
            Set,
            Reset
        }
        [SerializeField]
        public enum Skill
        {
            TestPlayer_Default,
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
