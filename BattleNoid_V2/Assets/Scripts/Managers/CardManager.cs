using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
public class CardManager
{

    [SerializeField] private GameObject cardBackGround;
    public List<CardData> CardDatas = new List<CardData>();


    public void ShowCards()
    {
        var card = Managers.Data.Instantiate("CardPrefab", null, false);                //TODO : ÇÁ¸®ÆÕ Á¦ÀÛ
        card.transform.position = Vector3.zero;
        cardBackGround = card;
    }

    private void DeletCards()
    {
        Managers.Pool.Destroy(cardBackGround);
    }

    public CardData GetCard()
    {
        if(CardDatas.Count > 0)
        {
            int cardRarity = Random.Range(0, 100);
            Rarity rarity;

            if(cardRarity <= 49)
            {
                rarity = Rarity.Common;
            }
            else if(cardRarity <= 79)
            {
                rarity = Rarity.Rare;
            }
            else if(cardRarity <= 91)
            {
                rarity = Rarity.Epic;
            }
            else
            {
                rarity = Rarity.Legendary;
            }

            List<CardData> selectedCards = new List<CardData>();

            foreach(CardData card in CardDatas)
            {
                if(card.rarity == rarity)
                {
                    selectedCards.Add(card);
                }
            }

            if(selectedCards.Count > 0)
            {
                int cardNum = Random.Range(0, selectedCards.Count);
                return selectedCards[cardNum];
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;
        }    
    }

    public void UseMethod(CardData card)
    {
        for(int i = 0;i < card.cardTesks.Count;i++)
        {
            Managers.Player.SetStats(card.cardTesks[i].operationType, card.cardTesks[i].stat, card.cardTesks[i].amount);
        }

        DeletCards();
    }

}
