using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class InGameStateManager : Singleton<InGameStateManager>
{
    public static bool gamePased;
    public static bool inCombat;
    public static bool PreparePhase;
    public static bool BattelPhase;

    [Header("CardHolder")]
    [SerializeField]
    public Transform hand;
    public Transform extraDeck;
    public Transform drawPileParent;
    public Transform drawPileParentContent;
    public Transform discardPileParent;
    public Transform discardPileParentContent;
    public GameObject EndTurnButton;

    public Action OnCombatStart;
    public Action OnPreparePhaseStart;
    public Action OnPreparePhaseEnd;
    public Action OnBattlePhaseStart;
    public Action OnBattlePhaseEnd;

    [Header("Button")]
    public Action OnCombatEnd;

    public Action<CardBehavior, BaseEntity> OnSpellCardPlayed;
    public Action<CardBehavior, BaseEntity> OnItemCardPlayed;

    private InGameCardModel inGameCardModel;

    [Header("CardHolder Text")]
    public TextMeshProUGUI DrawPileText;
    public TextMeshProUGUI DiscardPileText;

    // private

    protected override void Awake()
    {
        base.Awake();
        gamePased = false;
        inCombat = false;
        PreparePhase = false;
        BattelPhase = false;
        inGameCardModel = FindAnyObjectByType<InGameCardModel>();
    }

    // 当战斗开始的时候，初始化卡组内的卡牌
    public void CombatStart()
    {
        inCombat = true;

        inGameCardModel.InitialzeDeck();

        OnCombatStart?.Invoke(); // Safe way to invoke the delegate

        InitizeExtraDeck();
        UpdatePileText();

        Invoke("PreparePhaseStart", 0);
    }

    // 当战斗结束的时候，删除手牌
    public void CombatEnd(int remainningTurn)
    {
        inCombat = false;

        //将手牌中的卡片删除
        foreach (Transform child in hand)
        {
            GameObject.Destroy(child.gameObject);
        }

        OnCombatEnd?.Invoke(); // Safe way to invoke the delegate

        ActsManager.Instance.OnCombatEnd(remainningTurn);
    }

    // 回合开始
    public void PreparePhaseStart()
    {
        PreparePhase = true;

        // 显示战斗开始按钮
        EndTurnButton.SetActive(true);

        //5张抽牌, 将它们可视化
        DrawCards(5);

        OnPreparePhaseStart();
    }

    // 回合结束，丢弃所有手牌，进入战斗回合
    public void PreparePhaseEnd()
    {
        if (!PreparePhase)
        {
            Debug.Log("called prepare phase end when it's not in prepare phase");
            return;
        }

        // 隐藏战斗开始按钮
        EndTurnButton.SetActive(false);

        PreparePhase = false;

        DisCardAllCard();

        OnPreparePhaseEnd();

        BattlePhaseStart();
    }

    public void BattlePhaseStart()
    {
        if (BattelPhase)
        {
            Debug.Log("called BattlePhaseStart when it already in battle phase");
            return;
        }

        BattelPhase = true;

        OnBattlePhaseStart();
    }

    public void BattlePhaseEnd()
    {
        BattelPhase = false;

        OnBattlePhaseEnd();

        if (inCombat)
        {
            PreparePhaseStart();
        }
    }

    public void DisCardAllCard()
    {
        inGameCardModel.DisCardAllCard();

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
        Card newCard = inGameCardModel.DrawCard();

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

    // 抽一张指定的牌
    public void DrawSpecificCard(Card card)
    {
        //1张抽牌
        Card newCard = inGameCardModel.DrawSpecificCard(card);

        // 如果确实抽到了牌，那么将它可视化
        if (newCard != null)
        {
            CardDisplayView.Instance.DisPlaySingleCard(newCard, hand);
        }
        else
        {
            Debug.Log("Did not find the card " + card.cardName + " in draw pile");
        }

        UpdatePileText();
    }

    // 弃一张牌
    public void DiscardOneCard(Card _card)
    {
        inGameCardModel.DiscardCard(_card);
        UpdatePileText();
    }

    // 消耗一张牌
    public void ExhaustOneCard(Card _card)
    {
        inGameCardModel.ExhaustOneCard(_card);
        UpdatePileText();
    }

    public void UpdatePileText()
    {
        DrawPileText.text = inGameCardModel.GetDrawPileCard().Count + "";
        DiscardPileText.text = inGameCardModel.GetDiscardPileCard().Count + "";
    }

    public void InitizeExtraDeck()
    {
        foreach (Transform child in extraDeck)
        {
            Destroy(child.gameObject);
        }

        List<Card> extraDeckPile = inGameCardModel.GetExtraDeckPileCard();
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
            inGameCardModel.AddToHand(card);
            CardDisplayView.Instance.DisPlaySingleCard(card, hand);
        }
    }

    // 将一张卡添加抽牌堆
    public void AddToDrawPile(Card card)
    {
        if (card != null)
        {
            inGameCardModel.AddToDrawPile(card);
        }
        UpdatePileText();
    }

    // 将一张卡添加抽牌堆
    public void AddToDiscardPile(Card card)
    {
        if (card != null)
        {
            inGameCardModel.AddToDiscardPile(card);
        }
        UpdatePileText();
    }

    public void ShowDarwPile()
    {
        foreach (Transform child in drawPileParentContent)
        {
            Destroy(child.gameObject);
        }

        List<Card> drawPileCard = new List<Card>();
        foreach (Card card in inGameCardModel.GetDrawPileCard())
        {
            drawPileCard.Add(card);
        }
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

        List<Card> discardPileCard = new List<Card>();
        foreach (Card card in inGameCardModel.GetDiscardPileCard())
        {
            discardPileCard.Add(card);
        }
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
