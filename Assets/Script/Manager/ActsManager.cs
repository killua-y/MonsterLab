using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsManager : Manager<ActsManager>
{
    // 涉及玩家存档
    public static int currentLayer = 1;
    public static EnemyType currentEnemyType;
    public static string currentEnemy = "AcidSlimeEnermy";

    public GameObject MapCanvas;
    public Transform Deck;
    public Transform mainDeckScrollContent;
    public Transform extraDeckScollContent;

    private List<Card> mainDeck;
    private List<Card> extraDeck;

    private List<Enemy> allEnemyList = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        LoadEnermyList();
    }

    private void LoadEnermyList()
    {
        allEnemyList = CardDataModel.Instance.enemyList;
    }

    private Enemy FindEnemy(int _layer, EnemyType _enemyType)
    {
        Enemy enemyFind = null;
        for (int i = 0; i < allEnemyList.Count; i++)
        {
            if ((allEnemyList[i].layer == _layer) && (allEnemyList[i].enemyType == _enemyType))
            {
                enemyFind = allEnemyList[i];
                allEnemyList.RemoveAt(i);
            }
        }
        return enemyFind;
    }

    public void ActivateAct(BoxType _boxType)
    {
        switch (_boxType)
        {
            case BoxType.NormalFight:
                currentEnemyType = EnemyType.Normal;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Normal).scriptLocation;

                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.EliteFight:
                currentEnemyType = EnemyType.Elite;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Elite).scriptLocation;

                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.BossFight:
                currentEnemyType = EnemyType.Boss;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Boss).scriptLocation;

                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.Events:
                RewardManager.Instance.GenerateReward(2, 0);
                break;

            case BoxType.Merchant:
                RewardManager.Instance.GenerateReward(0, 1);
                break;

            case BoxType.Treasure:
                RewardManager.Instance.GenerateReward(0, 1);
                break;

            default:
                break;
        }
    }

    public void OpenDeck()
    {
        if (Deck.gameObject.activeSelf)
        {
            Deck.gameObject.SetActive(false);
        }
        else
        {
            ShowMainDeck();
            ShowExtraDeck();
            Deck.gameObject.SetActive(true);
        }
    }

    private void ShowMainDeck()
    {
        foreach (Transform child in mainDeckScrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        mainDeck = CardDataModel.Instance.InitializeDeck();
        foreach (Card card in mainDeck)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, mainDeckScrollContent);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<DeckManageCardOnClick>().SetUp(card.id, true);
        }
    }

    private void ShowExtraDeck()
    {
        foreach (Transform child in extraDeckScollContent.transform)
        {
            Destroy(child.gameObject);
        }

        extraDeck = CardDataModel.Instance.InitializeExtraDeck();
        foreach (Card card in extraDeck)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, extraDeckScollContent);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<DeckManageCardOnClick>().SetUp(card.id, false);
            //Debug.Log("Get card with index : " + card.id);
        }
    }

    public void ChangeDeckFromMainToExtra(int cardIndex, bool fromMainToExtra, GameObject cardObject)
    {
        if (InGameStateManager.inGame)
        {
            return;
        }

        if (fromMainToExtra)
        {
            cardObject.transform.SetParent(extraDeckScollContent);
            cardObject.GetComponent<DeckManageCardOnClick>().isMainDeck = false;
        }
        else
        {
            cardObject.transform.SetParent(mainDeckScrollContent);
            cardObject.GetComponent<DeckManageCardOnClick>().isMainDeck = true;
        }

        CardDataModel.Instance.ChangeDeckFromMainToExtra(cardIndex, fromMainToExtra);
    }
}
