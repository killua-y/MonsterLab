using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsManager : Manager<ActsManager>
{
    public GameObject MapCanvas;
    public Transform Deck;
    public Transform mainDeckScrollContent;
    public Transform extraDeckScollContent;

    private List<Card> mainDeck;
    private List<Card> extraDeck;

    public static string CurrentEnemy = "AcidSlimeEnermy";
    private List<string> normalNnemyList = new List<string>();
    private List<string> eliteNnemyList = new List<string>();
    private List<string> bossList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        normalNnemyList.Add("EnemyBheavior");
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ActivateAct(BoxType _boxType)
    {
        switch (_boxType)
        {
            case BoxType.NormalFight:
                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.EliteFight:
                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.BossFight:
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

    public void OpenMap()
    {
        if (MapCanvas.activeSelf)
        {
            MapCanvas.SetActive(false);
        }
        else
        {
            MapCanvas.SetActive(true);
        }
    }
}
