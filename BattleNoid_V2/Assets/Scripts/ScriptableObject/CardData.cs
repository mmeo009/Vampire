using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/card")]
public class CardData : ScriptableObject
{
    [Header("�� ī���� �̸�")] public string cardName;

    [Header("�� ī�尡 ���� ĳ����")] public PlayerType character;

    [Header("�� ī���� ��͵�")] public Rarity rarity;

    [Header("�� ī���� ȿ��")] public List<CardTesk> cardTesks = new List<CardTesk>();
}
[System.Serializable]
public class CardTesk
{
    [Header("������ ����")] public StatType stat;

    [Header("������ ���")] public OperationType operationType;

    [Header("������ ��")] public float amount;
}


