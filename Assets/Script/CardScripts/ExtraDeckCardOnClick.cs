using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraDeckCardOnClick : MonoBehaviour, IPointerClickHandler
{
    private Card cardModel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InGameStateManager.Instance.AddToHand(cardModel);
        Destroy(this.gameObject);
    }

    public void SetUp(Card card)
    {
        cardModel = card;
    }
}
