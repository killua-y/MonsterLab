using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventBehavior : MonoBehaviour
{
    // 从0开始数
    protected virtual string startSceneImageLocation { get; set; } = "";
    protected virtual string startSceneEventText { get; set; } = "Event Text";
    protected virtual List<string> startSceneOptionsText { get; set; } = new List<string>();
    protected List<Action> startSceneOptionsAction;

    protected EventManager eventCanvasBehavior;
    protected GameObject optionButtonPrefab;
    protected Transform buttonParent;

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

        // 绑定按钮
        bindAction();

        // 生成按钮
        SetUpEventScene(startSceneImageLocation, startSceneEventText, startSceneOptionsText, startSceneOptionsAction);
    }

    protected void SetUpEventScene(string _imageLocation, string _eventText, List<string> _optionsText, List<Action> _optionsAction)
    {
        int optionNumber = _optionsAction.Count;

        // 生成图片
        eventCanvasBehavior.eventImage.sprite = Resources.Load<Sprite>(_imageLocation);

        // 修改text
        eventCanvasBehavior.eventText.text = _eventText;

        // 清除多余按钮
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // 生成按钮
        for (int i = 0; i < optionNumber; i++)
        {
            GameObject buttonObject = Instantiate(optionButtonPrefab, buttonParent);
            Button optionButton = buttonObject.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

            int localIndex = i; // Create a local copy of the loop variable
            if (CheckOptionValidity(_optionsText[localIndex]))
            {
                buttonText.text = _optionsText[localIndex];
                optionButton.onClick.AddListener(() => _optionsAction[localIndex].Invoke());
            }
            else
            {
                buttonText.text = "Cannot Select";
            }
        }
    }

    protected void SetUpLeaveEventScene(string _imageLocation, string _eventText)
    {
        // 生成图片
        eventCanvasBehavior.eventImage.sprite = Resources.Load<Sprite>(_imageLocation);

        // 修改text
        eventCanvasBehavior.eventText.text = _eventText;

        // 清除多余按钮
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // 离开生成按钮
        GameObject buttonObject = Instantiate(optionButtonPrefab, buttonParent);
        Button optionButton = buttonObject.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = "Leave";
        optionButton.onClick.AddListener(LeaveEvent);
    }

    protected virtual void bindAction()
    {

    }

    protected void CloseEventPanel()
    {
        eventCanvasBehavior.ChangePosition();
        Destroy(this);
    }

    protected void LeaveEvent()
    {
        CloseEventPanel();
        ActsManager.Instance.LeaveScene();
    }

    protected virtual bool CheckOptionValidity(string optionText = null)
    {
        return true;
    }
}

[System.Serializable]
public class QuestionMarkEvent
{
    public EventType eventType;
    public string name;
    public int layer;
    public string scriptLocation;

    public QuestionMarkEvent(EventType _eventType, string _name, int _layer, string _scriptLocation)
    {
        eventType = _eventType;
        this.name = _name;
        this.layer = _layer;
        this.scriptLocation = _scriptLocation;
    }
}

public enum EventType
{
    Event,
    BaseUnitEvent
}
