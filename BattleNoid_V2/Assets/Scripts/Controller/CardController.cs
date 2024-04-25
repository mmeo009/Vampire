using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private CardData myData;  // ī�� �����͸� �����ϴ� ����
    [SerializeField] private TMP_Text perkName;  // ī���� �̸��� ǥ���ϴ� TMP_Text UI ���
    [SerializeField] private TMP_Text perkStats;  // ī���� Ư���� ǥ���ϴ� TMP_Text UI ���
    [SerializeField] private Button button;  // ī�带 ����ϱ� ���� ��ư UI ���

    void Awake()
    {
        myData = Managers.Card.GetCard();  // ī�� �����͸� ������

        if (myData != null)
        {
            perkName.text = myData.cardName;  // ī�� �̸��� UI�� ǥ��

            string cardTesks = "";
            foreach (CardTesk card in myData.cardTesks)
            {
                // ī���� Ư���� �ؽ�Ʈ �������� �����Ͽ� UI�� ǥ��
                cardTesks += $"{card.stat}�� {card.amount}��ŭ {card.operationType}\r\n";
            }
            perkStats.text = cardTesks;  // ī���� Ư���� UI�� ǥ��

            AddButtonTask();  // ��ư Ŭ�� �̺�Ʈ�� �߰�
        }
    }

    // ��ư Ŭ�� �̺�Ʈ�� �߰��ϴ� �޼���
    private void AddButtonTask()
    {
        // Ŭ�� �ÿ� ī�带 ����ϴ� �޼��带 ȣ��
        button.onClick.AddListener(() => Managers.Card.UseMethod(myData));
    }
}
