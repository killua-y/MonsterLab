using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class InGameStateManager : Manager<InGameStateManager>
{
    public static bool gamePased = false;
    public static bool PreparePhase = false;
    public static bool BattelPhase = false;
    public static GamePhase gamePhase;
    public Transform hand;
    public Transform extraDeck;
    public Transform drawPileParent;
    public Transform drawPileParentContent;
    public Transform discardPileParent;
    public Transform discardPileParentContent;

    public Action OnGameStart;
    public Action OnPreparePhaseStart;
    public Action OnPreparePhaseEnd;
    public Action OnBattlePhaseStart;
    public Action OnBattlePhaseEnd;

    private InGameCardModel CardModel;
    private CardDisplayView cardDisplayView;

    public TextMeshProUGUI DrawPileText;
    public TextMeshProUGUI DiscardPileText;
    // Start is called before the first frame update

    new void Awake()
    {
        base.Awake();
        CardModel = FindObjectOfType<InGameCardModel>();
        cardDisplayView = FindObjectOfType<CardDisplayView>();
    }

    void Start()
    {
        GameStart();
        UpdatePileText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 当战斗开始的时候，初始化卡组内的卡牌
    public void GameStart()
    {
        CardModel.InitialzeDeck();
        InitizeExtraDeck();
        Debug.Log("Game Initialzed");

        OnGameStart?.Invoke(); // Safe way to invoke the delegate
        Invoke("PreparePhaseStart", 0);
    }

    // 回合开始
    public void PreparePhaseStart()
    {
        gamePhase = GamePhase.PreparePhase;
        PreparePhase = true;
        BattelPhase = false;

        //5张抽牌, 将它们可视化
        for (int i = 0; i < 5; i++)
        {
            DrawOneCard();
        }

        OnPreparePhaseStart();
    }

    // 回合结束，丢弃所有手牌，进入战斗回合
    public void PreparePhaseEnd()
    {
        gamePhase = GamePhase.BattlePhase;
        PreparePhase = false;
        BattelPhase = true;

        CardModel.DisCardAllCard();

        //将手牌中的卡片删除
        foreach (Transform child in hand)
        {
            GameObject.Destroy(child.gameObject);
        }

        OnPreparePhaseEnd();

        BattlePhaseStart();
    }

    public void BattlePhaseStart()
    {
        OnBattlePhaseStart();
    }

    public void BattlePhaseEnd()
    {
        OnBattlePhaseEnd();

        PreparePhaseStart();
    }

    public void DisCardAllCard()
    {
        CardModel.DisCardAllCard();

        //将手牌中的卡片删除
        foreach (Transform child in hand)
        {
            GameObject.Destroy(child.gameObject);
        }

        UpdatePileText();
    }

    // 抽一张牌
    public void DrawOneCard()
    {
        //1张抽牌
        Card newCard = CardModel.DrawCard();

        // 如果确实抽到了牌，那么将它可视化
        if (newCard != null)
        {
            cardDisplayView.DisPlaySingleCard(newCard, hand);
        }

        UpdatePileText();
    }

    // 弃一张牌
    public void DiscardOneCard(Card _card)
    {
        CardModel.DiscardCard(_card);
        UpdatePileText();
    }

    // 消耗一张牌
    public void ExhaustOneCard(Card _card)
    {
        CardModel.ExhaustOneCard(_card);
        UpdatePileText();
    }

    public void UpdatePileText()
    {
        DrawPileText.text = CardModel.GetDrawPileCard().Count + "";
        DiscardPileText.text = CardModel.GetDiscardPileCard().Count + "";
    }

    public void InitizeExtraDeck()
    {
        List<Card> extraDeckPile = CardModel.GetExtraDeckPileCard();
        foreach (Card card in extraDeckPile)
        {
            GameObject cardObject = cardDisplayView.DisPlaySingleCard(card, extraDeck);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<ExtraDeckCardOnClick>().SetUp(card);
        }
    }

    public void ShowExtraDeck()
    {
        if (extraDeck.gameObject.activeSelf)
        {
            extraDeck.gameObject.SetActive(false);
        }
        else
        {
            extraDeck.gameObject.SetActive(true);
        }
    }

    // 将一张卡添加到手牌
    public void AddToHand(Card card)
    {
        if (card != null)
        {
            CardModel.AddToHand(card);
            cardDisplayView.DisPlaySingleCard(card, hand);
        }
    }

    public void ShowDarwPile()
    {
        if (drawPileParent.gameObject.activeSelf)
        {
            foreach (Transform child in drawPileParentContent)
            {
                Destroy(child.gameObject);
            }


            drawPileParent.gameObject.SetActive(false);
        }
        else
        {
            List<Card> drawPileCard = CardModel.GetDrawPileCard();
            drawPileCard.Sort((card1, card2) => card1.id.CompareTo(card2.id));

            foreach (Card card in drawPileCard)
            {
                GameObject cardObject = cardDisplayView.DisPlaySingleCard(card, drawPileParentContent);
                cardObject.AddComponent<Scaling>();
            }
            drawPileParent.gameObject.SetActive(true);
        }
    }

    public void ShowDiscardPile()
    {
        if (discardPileParent.gameObject.activeSelf)
        {
            foreach (Transform child in discardPileParentContent)
            {
                Destroy(child.gameObject);
            }

            discardPileParent.gameObject.SetActive(false);
        }
        else
        {
            List<Card> discardPileCard = CardModel.GetDiscardPileCard();
            discardPileCard.Sort((card1, card2) => card1.id.CompareTo(card2.id));

            foreach (Card card in discardPileCard)
            {
                GameObject cardObject = cardDisplayView.DisPlaySingleCard(card, discardPileParentContent);
                cardObject.AddComponent<Scaling>();
            }
            discardPileParent.gameObject.SetActive(true);
        }
    }
}

public enum GamePhase
{
    InvestigationPhase,
    PreparePhase,
    BattlePhase
}
