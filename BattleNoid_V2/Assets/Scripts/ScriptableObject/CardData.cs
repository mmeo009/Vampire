using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/card")]
public class CardData : ScriptableObject
{
    [Header("이 카드의 이름")] public string cardName;

    [Header("이 카드가 나올 캐릭터")] public PlayerType character;

    [Header("이 카드의 희귀도")] public Rarity rarity;

    [Header("이 카드의 효과")] public List<CardTesk> cardTesks = new List<CardTesk>();
}
[System.Serializable]
public class CardTesk
{
    [Header("변경할 스텟")] public StatType stat;

    [Header("변경할 방법")] public OperationType operationType;

    [Header("변경할 양")] public float amount;
}


