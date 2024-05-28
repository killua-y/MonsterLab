using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UpdateCardView(Card _card)
    {
        UpdateIndividualCardView(_card);

        Card originalCard = CardDataModel.Instance.GetCard(_card.id);
        UpdateColor(_card, originalCard);
    }

    public virtual void UpdateIndividualCardView(Card _card)
    {
        Debug.Log("Please attach specific card display script to card: " + _card.cardName);
    }

    public virtual void UpdateColor(Card _card, Card originalCard)
    {

    }
}
