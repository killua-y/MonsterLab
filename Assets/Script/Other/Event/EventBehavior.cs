using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventBehavior : MonoBehaviour
{
    // 从0开始数
    protected virtual int optionNumber { get; set; } = 2;
    protected virtual List<string> optionsText { get; set; } = new List<string>();
    protected List<Action> optionsAction;
    protected virtual List<string> eventText { get; set; } = new List<string>();
    protected virtual string eventImageLocation { get; set; } = "";

    protected EventManager eventCanvasBehavior;
    protected GameObject optionButtonPrefab;
    protected Transform buttonParent;

    protected virtual void bindAction()
    {
         
    }

    public virtual bool isValid()
    {
        return true;
    }

    public virtual void SetUp()
    {
        eventCanvasBehavior = this.gameObject.GetComponent<EventManager>();
        if (eventCanvasBehavior == null)
        {
            Debug.Log("please attach EventBehavior to event canvas");
            return;
        }
        optionButtonPrefab = eventCanvasBehavior.optionButtonPrefab;
        buttonParent = eventCanvasBehavior.buttonParent;

        // 生成图片
        eventCanvasBehavior.eventImage.sprite = Resources.Load<Sprite>(eventImageLocation);

        // 修改text
        eventCanvasBehavior.eventText.text = eventText[0];

        // 生成按钮
        bindAction();

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < optionNumber; i++)
        {
            GameObject buttonObject = Instantiate(optionButtonPrefab, buttonParent);
            Button optionButton = buttonObject.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = optionsText[i]; 
            optionButton.onClick.AddListener(() => optionsAction[0].Invoke()); // Assumes single option action
        }
    }

    protected void Leave()
    {
        eventCanvasBehavior.ChangePosition();
        Destroy(this);
    }
}
