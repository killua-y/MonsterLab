using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCardModel : MonoBehaviour
{
    // 卡牌管理区
    public GameObject cardDataManager;

    // 局内游戏手牌和卡组
    private List<Card> handList = new List<Card>(); // 局内存储手牌数据的链表
    private List<Card> drawPileList = new List<Card>(); // 局内抽堆数据的链表
    private List<Card> discardPileList = new List<Card>(); // 局内弃牌堆数据的链表
    private List<Card> extraDeckPileList = new List<Card>(); // 局内弃牌堆数据的链表

    private int currentAssignedID;

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnGameEnd += OnGameEnd;
    }

    // 加载局内卡牌, call CardDataModel里的InitializeDeck()，对输出的每张卡进行clone
    public void InitialzeDeck()
    {
        currentAssignedID = 0;

        foreach (Card card in CardDataModel.Instance.InitializeDeck())
        {
            Card newCard = Card.CloneCard(card);
            newCard.uniqueID = currentAssignedID;
            currentAssignedID += 1;
            drawPileList.Add(newCard);
        }

        HelperFunction.Shuffle(drawPileList);

        foreach (Card card in CardDataModel.Instance.InitializeExtraDeck())
        {
            Card newCard = Card.CloneCard(card);
            newCard.uniqueID = currentAssignedID;
            currentAssignedID += 1;
            extraDeckPileList.Add(newCard);
        }
    }

    // 三个helper，用于传出三个卡list信息
    public List<Card> GetHandCard()
    {
        foreach(Card _card in handList)
        {
            Debug.Log("Hand contain: " + _card.cardName);
        }

        return handList;
    }

    public List<Card> GetDrawPileCard()
    {
        List<Card> result = new List<Card>();
        foreach (Card card in drawPileList)
        {
            result.Add(card);
        }
        return result;
    }

    public List<Card> GetDiscardPileCard()
    {
        List<Card> result = new List<Card>();
        foreach (Card card in discardPileList)
        {
            result.Add(card);
        }
        return result;
    }

    public List<Card> GetExtraDeckPileCard()
    {
        return extraDeckPileList;
    }

    // 抽牌
    public Card DrawCard()
    {
        Card cardDrawed = null;

        // 查看抽牌堆是否有牌
        if (drawPileList.Count > 0)
        {
            // 移除抽牌堆第一张牌
            cardDrawed = drawPileList[0];
            drawPileList.RemoveAt(0);

            // 加入手牌堆的最后面
            handList.Add(cardDrawed);
        }
        // 否则，查看弃牌堆是否有牌，有的话重新洗牌
        else if (discardPileList.Count > 0)
        {
            ShuffleDeck();

            // 然后再抽一张牌
            cardDrawed = DrawCard();
        }

        return cardDrawed;
    }

    // 重新洗牌
    public void ShuffleDeck()
    {
        // Shuffle 弃牌堆
        HelperFunction.Shuffle(discardPileList);

        // 将卡牌加入抽牌堆，并清空弃牌堆
        drawPileList.AddRange(discardPileList);
        discardPileList.Clear();
    }

    // 弃牌，传入需要弃掉的卡牌信息
    public void DiscardCard(Card _card)
    {
        if(handList.Contains(_card))
        {
            handList.RemoveAt(handList.IndexOf(_card));

            // 加入弃牌堆的最后面
            discardPileList.Add(_card);

            //Debug.Log("Discard card: " + _card.cardName);
        }
        else
        {
            Debug.Log("Cannot find the card to Discard named: " + _card.cardName);
        }
    }

    // 丢弃所有牌
    public void DisCardAllCard()
    {
        while (handList.Count > 0)
        {
            DiscardCard(handList[0]);
            
        }
    }

    // 消耗一张牌，传入需要弃掉的卡牌信息
    public void ExhaustOneCard(Card _card)
    {
        if (handList.Contains(_card))
        {
            handList.RemoveAt(handList.IndexOf(_card));

            // 不加入弃牌堆的最后面，直接消耗
        }
        else
        {
            Debug.Log("Cannot find the card to Exhaust named: " + _card.cardName);
        }
    }

    // 将一张卡加入手牌
    public void AddToHand(Card card)
    {
        handList.Add(card);
    }

    private void OnGameEnd()
    {
        //清空列表
        ClearCardList(handList);
        ClearCardList(drawPileList);
        ClearCardList(discardPileList);
        ClearCardList(extraDeckPileList);
    }

    // helper，用于清空列表
    private void ClearCardList(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i] = null;
        }
        cardList.Clear();
    }
}
