using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : Manager<ShopManager>
{
    public Transform CardHolder;
    public Transform DNAHolder;

    public TextMeshProUGUI refreshCostText;
    public TextMeshProUGUI deleteCostText;

    // private
    private int refreshCost;
    private int deleteCost;
    private List<Card> cardList;
    private List<DNA> dnaList;

    private bool isOpen = false;

    public void GenerateShop()
    {
        ChangePosition();

        refreshCost = 0;
        //RefreshDNA();
        RefreshShopCard();
        refreshCost = 50;
        deleteCost = 100;

        UpdateShopCostView();
    }

    void ChangePosition()
    {
        if (isOpen)
        {
            StartCoroutine(SmoothMoveCoroutine(0, 1080));
            isOpen = false;
        }
        else
        {
            StartCoroutine(SmoothMoveCoroutine(1080, 0));
            isOpen = true;
        }
    }

    private IEnumerator SmoothMoveCoroutine(float startY, float endY)
    {
        float elapsedTime = 0;
        float duration = 0.2f;
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(startPosition.x, endY, startPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the current position using Lerp
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set
        transform.localPosition = targetPosition;
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
                    price = GameSetting.InCombatRand.Next(50, 101);
                    break;
                case CardRarity.Rare:
                    price = GameSetting.InCombatRand.Next(100, 151);
                    break;
                case CardRarity.Legend:
                    price = GameSetting.InCombatRand.Next(150, 201);
                    break;
                default:
                    break;
            }

            newCard.AddComponent<ShopCardBuyOnClick>().SetUp(card, price);
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

    public void DeleteCard()
    {
        // 扣钱
        if (PlayerStatesManager.Gold < deleteCost)
        {
            Debug.Log("Not enough gold");
            return;
        }
        else
        {
            CardHolder.gameObject.SetActive(false);
            PlayerStatesManager.Instance.DecreaseGold(deleteCost);
            deleteCost += 50;
            CardSelectPanelBehavior.Instance.SelectCardFromDeck(DeleteCardHelper);
        }
    }

    private void DeleteCardHelper(Card card)
    {
        CardDataModel.Instance.DeleteCard(card);
        CardHolder.gameObject.SetActive(true);
    }

    public void BuyCard(Card _card, int price, GameObject card)
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

        CardDataModel.Instance.ObtainCard(_card);
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
}

public class ShopCardBuyOnClick : MonoBehaviour, IPointerClickHandler
{
    private Card card;
    private int price;

    public TextMeshProUGUI priceText;

    public void SetUp(Card _card, int _price)
    {
        card = _card;
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
            ShopManager.Instance.BuyCard(card, price, this.gameObject);
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
            ShopManager.Instance.BuyDNA(index, price, this.gameObject);
        }
    }
}