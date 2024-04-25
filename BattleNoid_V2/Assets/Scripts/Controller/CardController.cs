using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private CardData myData;  // 카드 데이터를 저장하는 변수
    [SerializeField] private TMP_Text perkName;  // 카드의 이름을 표시하는 TMP_Text UI 요소
    [SerializeField] private TMP_Text perkStats;  // 카드의 특성을 표시하는 TMP_Text UI 요소
    [SerializeField] private Button button;  // 카드를 사용하기 위한 버튼 UI 요소

    void Awake()
    {
        myData = Managers.Card.GetCard();  // 카드 데이터를 가져옴

        if (myData != null)
        {
            perkName.text = myData.cardName;  // 카드 이름을 UI에 표시

            string cardTesks = "";
            foreach (CardTesk card in myData.cardTesks)
            {
                // 카드의 특성을 텍스트 형식으로 생성하여 UI에 표시
                cardTesks += $"{card.stat}를 {card.amount}만큼 {card.operationType}\r\n";
            }
            perkStats.text = cardTesks;  // 카드의 특성을 UI에 표시

            AddButtonTask();  // 버튼 클릭 이벤트를 추가
        }
    }

    // 버튼 클릭 이벤트를 추가하는 메서드
    private void AddButtonTask()
    {
        // 클릭 시에 카드를 사용하는 메서드를 호출
        button.onClick.AddListener(() => Managers.Card.UseMethod(myData));
    }
}
