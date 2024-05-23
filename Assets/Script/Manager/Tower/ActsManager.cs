using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActsManager : MonoBehaviour
{
    private CardDataModel cardData;
    public Transform Deck;
    public Transform drawPileScroll;
    public Transform drawPileScrollContent;
    public Transform discardPileScoll;
    public Transform discardPileScollContent;

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
            Deck.gameObject.SetActive(true);
        }
    }

    private void ShowDarwPile()
    {

    }

    private void ShowDiscardPile()
    {

    }
}
