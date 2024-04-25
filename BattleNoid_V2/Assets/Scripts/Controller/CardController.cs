using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private CardData myData;
    [SerializeField] private TMP_Text perkName;
    [SerializeField] private TMP_Text perkStats;
    [SerializeField] private Button button;
    void Awake()
    {
        myData = Managers.Card.GetCard();

        if(myData != null)
        {
            perkName.text = myData.cardName;
            string cardTesks = "";
            foreach(CardTesk card in myData.cardTesks)
            {
                cardTesks += $"{card.stat}¸¦ {card.amount}¸¸Å­ {card.operationType}\r\n";
            }
            perkStats.text = cardTesks;

            AddButtonTesk();
        }
    }
    private void AddButtonTesk()
    {
        button.onClick.AddListener(() => Managers.Card.UseMethod(myData));
    }
}
