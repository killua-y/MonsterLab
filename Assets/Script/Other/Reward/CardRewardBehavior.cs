using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardBehavior : MonoBehaviour
{
    public RectTransform CardHolder;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddCard(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            // Set the anchored position to zero
            CardHolder.anchoredPosition = new Vector2(0, -150);

            GameObject newCard = CardDisplayView.Instance.DisPlaySingleCard(card, CardHolder);
            newCard.AddComponent<Scaling>();
            newCard.AddComponent<SingleCardOnClick>();
            newCard.GetComponent<SingleCardOnClick>().SetUp(card.id, this.gameObject);
        }
    }
}
