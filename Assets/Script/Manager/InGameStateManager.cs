using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class InGameStateManager : Manager<InGameStateManager>
{
    public static bool gamePased = false;
    public static bool inGame = false;
    public static bool PreparePhase = false;
    public static bool BattelPhase = false;

    [Header("CardHolder")]
    [SerializeField]
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
    public Action OnGameEnd;

    public Action<CardBehavior, BaseEntity> OnSpellCardPlayed;
    public Action<CardBehavior, BaseEntity> OnItemCardPlayed;

    private InGameCardModel CardModel;

    public TextMeshProUGUI DrawPileText;
    public TextMeshProUGUI DiscardPileText;
    // Start is called before the first frame update

    new void Awake()
    {
        base.Awake();
        CardModel = FindObjectOfType<InGameCardModel>();
    }

    // 当战斗开始的时候，初始化卡组内的卡牌
    public void GameStart()
    {
        inGame = true;
        CardModel.InitialzeDeck();

        OnGameStart?.Invoke(); // Safe way to invoke the delegate

        InitizeExtraDeck();
        UpdatePileText();

        Invoke("PreparePhaseStart", 0);
    }

    // 当战斗开始的时候，初始化卡组内的卡牌
    public void GameEnd(int remainningTurn)
    {
        inGame = false;

        //将手牌中的卡片删除
        foreach (Transform child in hand)
        {
            GameObject.Destroy(child.gameObject);
        }

        RewardManager.Instance.GenerateReward(remainningTurn);

        OnGameEnd?.Invoke(); // Safe way to invoke the delegate
    }

    // 回合开始
    public void PreparePhaseStart()
    {
        PreparePhase = true;

        //5张抽牌, 将它们可视化
        DrawCards(5);

        OnPreparePhaseStart();
    }

    // 回合结束，丢弃所有手牌，进入战斗回合
    public void PreparePhaseEnd()
    {
        if (!PreparePhase)
        {
            return;
        }

        PreparePhase = false;

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
        BattelPhase = true;

        OnBattlePhaseStart();
    }

    public void BattlePhaseEnd()
    {
        BattelPhase = false;

        OnBattlePhaseEnd();

        if (inGame)
        {
            PreparePhaseStart();
        }
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
            CardDisplayView.Instance.DisPlaySingleCard(newCard, hand);
        }

        UpdatePileText();
    }

    // 抽一张牌
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawOneCard();
        }
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
        foreach (Transform child in extraDeck)
        {
            Destroy(child.gameObject);
        }

        List<Card> extraDeckPile = CardModel.GetExtraDeckPileCard();
        foreach (Card card in extraDeckPile)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, extraDeck);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<ExtraDeckCardOnClick>().SetUp(card);
        }
    }

    // 将一张卡添加到手牌
    public void AddToHand(Card card)
    {
        if (card != null)
        {
            CardModel.AddToHand(card);
            CardDisplayView.Instance.DisPlaySingleCard(card, hand);
        }
    }

    public void ShowDarwPile()
    {
        foreach (Transform child in drawPileParentContent)
        {
            Destroy(child.gameObject);
        }

        List<Card> drawPileCard = CardModel.GetDrawPileCard();
        drawPileCard.Sort((card1, card2) => card1.id.CompareTo(card2.id));

        foreach (Card card in drawPileCard)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, drawPileParentContent);
            cardObject.AddComponent<Scaling>();
        }
        drawPileParent.gameObject.SetActive(true);
    }

    public void ShowDiscardPile()
    {
        foreach (Transform child in discardPileParentContent)
        {
            Destroy(child.gameObject);
        }

        List<Card> discardPileCard = CardModel.GetDiscardPileCard();
        discardPileCard.Sort((card1, card2) => card1.id.CompareTo(card2.id));

        foreach (Card card in discardPileCard)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, discardPileParentContent);
            cardObject.AddComponent<Scaling>();
        }
        discardPileParent.gameObject.SetActive(true);
    }

    public void SpellCardPlayed(CardBehavior cardBehavior, BaseEntity targetMonster)
    {
        OnSpellCardPlayed?.Invoke(cardBehavior, targetMonster);
    }

    public void ItemCardPlayed(CardBehavior cardBehavior, BaseEntity targetMonster)
    {
        OnItemCardPlayed?.Invoke(cardBehavior, targetMonster);
    }
}
