using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : Manager<ShopManager>
{
    public Transform CardHolder;
    public Transform DNAHolder;

    public GameObject AllDeck;
    public Transform AllDeckContent;

    public TextMeshProUGUI refreshCostText;
    public TextMeshProUGUI deleteCostText;

    // private
    private int refreshCost;
    private int deleteCost;
    private List<Card> cardList;
    private List<DNA> dnaList;

    public void GenerateShop()
    {
        refreshCost = 0;
        //RefreshDNA();
        RefreshShopCard();
        refreshCost = 50;
        deleteCost = 100;

        UpdateShopCostView();
    }

    public void RefreshShopCard()
    {
        // 扣钱
        if (PlayerStatesManager.Gold < refreshCost)
        {
            Debug.Log("Not enough gold");
            return;
        }
        else
        {
            PlayerStatesManager.Instance.DecreaseGold(refreshCost);
        }

        cardList = new List<Card>();
        List<int> cardAlreadyHave = new List<int>();
        while (cardList.Count < 4)
        {
            int index = RewardManager.Instance.GetNextCardID();

            if (!cardAlreadyHave.Contains(index))
            {
                cardAlreadyHave.Add(index);
                cardList.Add(CardDataModel.Instance.GetCard(index));
            }
        }

        refreshCost += 25;

        UpdateShopItemView();
        UpdateShopCostView();
    }

    public void RefreshDNA()
    {
        dnaList = new List<DNA>();
        for (int i = 0; i < 3; i++)
        {
            DNA dna = RewardManager.Instance.GetNextDNA();
            dnaList.Add(dna);
        }

        UpdateShopDNAView();
    }

    public void UpdateShopItemView()
    {
        foreach (Transform child in CardHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in cardList)
        {
            GameObject newCard = CardDisplayView.Instance.DisPlaySingleCard(card, CardHolder);
            newCard.AddComponent<Scaling>();

            int price = 0;
            switch (card.cardRarity)
            {
                case CardRarity.Normal:
                    price = GameSetting.shuffleCardRand.Next(50, 101);
                    break;
                case CardRarity.Rare:
                    price = GameSetting.shuffleCardRand.Next(100, 151);
                    break;
                case CardRarity.Legend:
                    price = GameSetting.shuffleCardRand.Next(150, 201);
                    break;
                default:
                    break;
            }

            newCard.AddComponent<ShopCardBuyOnClick>().SetUp(card.id, price);
        }

        // 还没写
        //foreach (DNA dna in dnaList)
        //{

        //}

        refreshCostText.text = refreshCost + "";
    }

    public void UpdateShopDNAView()
    {
        // 还没写
        //foreach (DNA dna in dnaList)
        //{

        //}
    }

    public void UpdateShopCostView()
    {
        refreshCostText.text = refreshCost + "";
        deleteCostText.text = deleteCost + "";
    }

    public void DeleteCard(int cardIndex)
    {
        // 扣钱
        if (PlayerStatesManager.Gold < deleteCost)
        {
            Debug.Log("Not enough gold");
            return;
        }
        else
        {
            PlayerStatesManager.Instance.DecreaseGold(deleteCost);
        }

        OpenPlayerDeck();

        CardDataModel.Instance.DeleteCard(cardIndex);

        deleteCost += 50;
    }

    public void BuyCard(int cardIndex, int price, GameObject card)
    {
        // 扣钱
        if (PlayerStatesManager.Gold < price)
        {
            Debug.Log("Not enough gold");
            return;
        }
        else
        {
            PlayerStatesManager.Instance.DecreaseGold(price);
        }

        CardDataModel.Instance.ObtainCard(cardIndex);
        Destroy(card);
    }

    public void BuyDNA(int cardIndex, int price, GameObject dna)
    {
        // 扣钱
        if (PlayerStatesManager.Gold < price)
        {
            Debug.Log("Not enough gold");
            return;
        }
        else
        {
            PlayerStatesManager.Instance.DecreaseGold(price);
        }

        CardDataModel.Instance.ObtainDNA(cardIndex);
        Destroy(dna);
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

    public TextMeshProUGUI priceText;

    public void SetUp(int index, int _price)
    {
        cardIndex = index;
        price = _price;

        // Check if a TextMeshProUGUI component already exists, otherwise create one
        if (priceText == null)
        {
            // Create a new GameObject to hold the TextMeshProUGUI component
            GameObject textObject = new GameObject("PriceText");
            textObject.transform.SetParent(this.transform);
            textObject.transform.localPosition = new Vector3(0, -255, 0); // Move down by -255 units

            // Add the TextMeshProUGUI component
            priceText = textObject.AddComponent<TextMeshProUGUI>();
            priceText.fontSize = 34; // Set the font size
            priceText.alignment = TextAlignmentOptions.Center; // Set the alignment
        }

        priceText.text = "Price: " + price;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ShopManager.Instance.BuyCard(cardIndex, price, this.gameObject);
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
            ShopManager.Instance.BuyCard(index, price, this.gameObject);
        }
    }
}