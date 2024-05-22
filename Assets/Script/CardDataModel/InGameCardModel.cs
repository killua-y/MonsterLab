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
        currentAssignedID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 加载局内卡牌, call CardDataModel里的InitializeDeck()
    public void InitialzeDeck()
    {
        // 将玩家拥有的卡牌导入局内卡牌
        drawPileList = FindObjectOfType<CardDataModel>().InitializeDeck(currentAssignedID);

        // 更新currentAssignedID
        currentAssignedID += drawPileList.Count;
        Shuffle(drawPileList);

        // 将玩家拥有的卡牌导入局内卡牌
        extraDeckPileList = FindObjectOfType<CardDataModel>().InitializeExtraDeck(currentAssignedID);

        // 更新currentAssignedID
        currentAssignedID += extraDeckPileList.Count;
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

    public List<Card> GetDrawPiledCard()
    {
        return drawPileList;
    }

    public List<Card> GetDiscardPiledCard()
    {
        return discardPileList;
    }

    public void DebugCall()
    {

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
        Shuffle(discardPileList);

        // 将卡牌加入抽牌堆，并清空弃牌堆
        drawPileList.AddRange(discardPileList);
        discardPileList.Clear();
    }

    // 洗牌helper method
    public static void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
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
}
