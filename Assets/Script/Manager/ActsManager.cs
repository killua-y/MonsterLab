using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsManager : Manager<ActsManager>
{
    private CardDataModel cardData;
    public Transform Deck;
    public Transform mainDeckScrollContent;
    public Transform extraDeckScollContent;

    private List<Card> mainDeck;
    private List<Card> extraDeck;

    // Start is called before the first frame update
    void Start()
    {
        cardData = FindObjectOfType<CardDataModel>();
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

        mainDeck = cardData.InitializeDeck(0);
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

        extraDeck = cardData.InitializeExtraDeck(0);
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
                SceneManager.LoadScene("BattleScene");
                break;

            case BoxType.EliteFight:
                SceneManager.LoadScene("BattleScene");
                break;

            case BoxType.BossFight:
                SceneManager.LoadScene("BattleScene");
                break;

            case BoxType.Events:
                RewardManager.Instance.GenerateReward();
                break;

            case BoxType.Merchant:
                RewardManager.Instance.GenerateReward();
                break;

            case BoxType.Treasure:
                RewardManager.Instance.GenerateReward();
                break;

            default:
                break;
        }
    }

    public void ChangeDeckFromMainToExtra(int cardIndex, bool fromMainToExtra, GameObject cardObject)
    {
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

        cardData.ChangeDeckFromMainToExtra(cardIndex, fromMainToExtra);
    }
}
