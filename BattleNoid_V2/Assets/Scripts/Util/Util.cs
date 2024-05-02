using System.Collections.Generic;
using UnityEngine;

namespace Supporter
{
    public interface ICodeProvider
    {
        string GetCode();
    }
    public class Util
    {
        #region PublicMethod
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

            if (transform == null) 
                return null;

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
        #endregion
    }
    [SerializeField]
    public enum ActionType
    {
        SceneMove,
        ExitGame,
        PauseGame,
        SaveGame,
        LoadGame,
        LoadWaveData,
        DefaultAction
    }
    [SerializeField]
    public enum PlayerType
    {
        All,
        TestPlayer,
        Serena,
        Ember
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
    public enum OperationType
    {
        Plus,
        Minus,
        Set,
        Reset,
        PlusPercent,
        MinusPercent,
        None
    }
    [SerializeField]
    public enum Skill
    {
        TestPlayer_Default,
    }

    public enum StatType
    {
        None = 0,
        AttackSpeed,
        AttackDamage,
        AttackRange,
        MoveSpeed,
        CurrentHP,
        MAXHP,
        CurrentXP,
        MAXXP,
        LeftAmount,
        RightAmount,
        ForwardAmount,
        BackAmount,
        FirstSkillCoolDown,
        SecondSkillCoolDown,
    }
    public enum BulletType
    {
        Forward,
        Left,
        Right,
        Back,
        Target,
        Enemy,
        Freeze,
    }
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
    [System.Serializable]
    public class PlayerStats
    {
        public string code;
        public int level;

        public float hp;
        public float xp;

        public float currentHp;
        public float currentXp;

        public int hpPerSec;
        public int spPerSec;

        public float attackDamage;
        public float attackRange;
        public float attackSpeed;
        public float moveSpeed;
        public float bulletSpeed;
        public float rotationSpeed;

        public string firstSkill;
        public string secondSkill;

        public float firstCoolDown;
        public float secondCoolDown;

        public float currentFirstCoolDown;
        public float currentSecondCoolDown;

        public bool isFirstSkillActive;
        public bool isSecondSkillActive;

        public int leftAttackAmount;
        public int rightAttackAmount;
        public int forwardAttackAmount;
        public int backwardAttackAmount;

        public int attackTime;

        public PlayerController playerController;
    }

    [System.Serializable]
    public class MonsterStats
    {
        public string code;

        public float hp;

        public float knockBackAmount;
        public float knockBackTime;

        public float damagedInfi;

        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;
        public float rotationSpeed;

        public int attackType;
        public float viewingAngle;

        public MonsterController monsterController;
    }
    [System.Serializable]
    public class GameData
    {
        public string myName;
        public int money;
        public Dictionary<string, CharacterData> characterData = new Dictionary<string, CharacterData>();
    }
    [System.Serializable]
    public class CharacterData
    {
        public string code;
        public bool hasThisCharacter;
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
