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
    public Transform drawPileParent;
    public Transform discardPileParent;

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
        Debug.Log("Game Initialzed");

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
        DrawPileText.text = CardModel.GetDrawPiledCard().Count + "";
        DiscardPileText.text = CardModel.GetDiscardPiledCard().Count + "";
    }

    public void ShowDrawPileCard()
    {
        Debug.Log("Not yet implement");
    }
}

public enum GamePhase
{
    InvestigationPhase,
    PreparePhase,
    BattlePhase
}
