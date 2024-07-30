using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardRewardBehavior : MonoBehaviour, IPointerClickHandler
{
    private List<Card> cards;

    public void SetUp(List<Card> _cards)
    {
        cards = _cards;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FindAnyObjectByType<RewardCardSelectBehavior>().AddCard(cards, this.gameObject);
    }
}

public class DNARewardBehavior : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public DNA DNAModel;
    public TextMeshProUGUI nameText;

    public void SetUp(DNA _DNAModel)
    {
        nameText = GetComponentInChildren<TextMeshProUGUI>();
        DNAModel = _DNAModel;
        nameText.text = DNAModel.DNAName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CanvasManager.Instance.HideDNAPreview();
        PlayerStatesManager.Instance.AcquireDNA(DNAModel);
        Destroy(this.gameObject);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        CanvasManager.Instance.GenerateDNAPreview(DNAModel.DNAName, DNAModel.effectText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanvasManager.Instance.HideDNAPreview();
    }
}

public class GoldRewardBehavior : MonoBehaviour, IPointerClickHandler
{
    public int gold;
    public TextMeshProUGUI goldText;

    public void SetUp(int _gold)
    {
        goldText = GetComponentInChildren<TextMeshProUGUI>();
        gold = _gold;
        goldText.text = "Gold: " + gold;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(this.gameObject);
        PlayerStatesManager.Instance.IncreaseGold(gold);
    }
}
