using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardBehavior : MonoBehaviour
{
    public Transform CardHolder;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            GameObject newCard = CardDisplayView.Instance.DisPlaySingleCard(card, CardHolder);
            newCard.GetComponent<CardDisplay>().UpdateCardView(card);
            newCard.AddComponent<Scaling>();
            newCard.AddComponent<SingleCardOnClick>();
            newCard.GetComponent<SingleCardOnClick>().SetUp(card.id, this.gameObject);
        }
    }
}
