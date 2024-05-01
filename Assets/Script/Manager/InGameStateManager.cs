using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameStateManager : MonoBehaviour
{
    public static bool PreparePhase = false;
    public static bool BattelPhase = false;
    public static GamePhase gamePhase;
    public Transform hand;

    private InGameCardModel CardModel;
    private CardDisplayView cardDisplayView;
    // Start is called before the first frame update
    void Start()
    {
        CardModel = GetComponent<InGameCardModel>();
        cardDisplayView = GetComponent<CardDisplayView>();

        Invoke("GameStart", 0);
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
    }

    // 回合开始
    public void TurnStart()
    {
        gamePhase = GamePhase.PreparePhase;

        //5张抽牌, 将它们可视化
        for (int i = 0; i < 5; i++)
        {
            DrawOneCard();
        }
    }

    // 回合结束，丢弃所有手牌，进入战斗回合
    public void PreparePhaseEnd()
    {
        gamePhase = GamePhase.BattlePhase;

        CardModel.DisCardAllCard();

        //将手牌中的卡片删除
        foreach (Transform child in hand)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    // 抽一张牌
    public void DrawOneCard()
    {
        //1张抽牌
        Card newCard = CardModel.DrawCard();

        // 如果确实抽到了牌，那么将它可视化
        if(newCard != null)
        {
            cardDisplayView.DisPlaySingleCard(newCard, hand);
        }
    }

}

public enum GamePhase
{
    InvestigationPhase,
    PreparePhase,
    BattlePhase
}
