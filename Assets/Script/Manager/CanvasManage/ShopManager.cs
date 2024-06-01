using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : Manager<ShopManager>
{
    public Transform CardHolder;
    public Transform DNAHolder;

    public GameObject AllDeck;
    public Transform AllDeckContent;

    // private
    private int refreshCost;
    private List<Card> cardList;
    private List<DNA> dnaList;

    public void GenerateShop()
    {
        refreshCost = 50;
        cardList = new List<Card>();
        for (int i = 0; i < 4; i ++)
        {
            int index = RewardManager.Instance.GetNextCardID();
            cardList.Add(CardDataModel.Instance.GetCard(index));
        }
        dnaList = new List<DNA>();
        for (int i = 0; i < 3; i++)
        {
            DNA dna = RewardManager.Instance.GetNextDNA();
            dnaList.Add(dna);
        }

        UpdateShopItemView();
    }

    public void RefreshShopCard()
    {
        refreshCost += 25;

        cardList = new List<Card>();
        for (int i = 0; i < 4; i++)
        {
            int index = RewardManager.Instance.GetNextCardID();
            cardList.Add(CardDataModel.Instance.GetCard(index));
        }
    }

    public void UpdateShopItemView()
    {
        
    }

    public void DeleteCard(int cardIndex)
    {
        // 扣钱


        OpenPlayerDeck();

        CardDataModel.Instance.DeleteCard(cardIndex);
    }

    public void BuyCard(int cardIndex, int price)
    {
        // 扣钱

        CardDataModel.Instance.ObtainCard(cardIndex);
    }

    public void BuyDNA(int cardIndex, int price)
    {
        // 扣钱

        CardDataModel.Instance.ObtainDNA(cardIndex);
    }

    public void OpenPlayerDeck()
    {
        if (AllDeck.activeSelf)
        {
            foreach (Transform child in AllDeckContent)
            {
                Destroy(child.gameObject);
            }

            AllDeck.SetActive(false);
        }
        else
        {
            cardList = new List<Card>();
            cardList = CardDataModel.Instance.InitializeDeck();
            foreach (Card card in CardDataModel.Instance.InitializeExtraDeck())
            {
                cardList.Add(card);
            }

            foreach (Card card in cardList)
            {
                GameObject newCard = CardDisplayView.Instance.DisPlaySingleCard(card, AllDeckContent);
                newCard.AddComponent<Scaling>();
                newCard.AddComponent<ShopCardDeleteOnClick>().SetUp(card.id);
            }

            AllDeck.SetActive(true);
        }
    }
}

public class ShopCardDeleteOnClick : MonoBehaviour, IPointerClickHandler
{
    private int cardIndex;

    public void SetUp(int index)
    {
        cardIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ShopManager.Instance.DeleteCard(cardIndex);
        }
    }
}

public class ShopCardBuyOnClick : MonoBehaviour, IPointerClickHandler
{
    private int cardIndex;
    private int price;

    public void SetUp(int index, int _price)
    {
        cardIndex = index;
        price = _price;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ShopManager.Instance.BuyCard(cardIndex, price);
        }
    }
}

public class ShopDNAOnClick : MonoBehaviour, IPointerClickHandler
{
    private int index;
    private int price;

    public void SetUp(int _index, int _price)
    {
        index = _index;
        price = _price;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ShopManager.Instance.BuyCard(index, price);
        }
    }
}