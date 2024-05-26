using System.Collections.Generic;
using config;
using events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardWrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
    IPointerUpHandler, IPointerClickHandler {
    private const float EPS = 0.01f;

    public float targetRotation;
    public Vector2 targetPosition;
    public float targetVerticalDisplacement;
    public int uiLayer;

    private RectTransform rectTransform;
    private Canvas canvas;

    public ZoomConfig zoomConfig;
    public AnimationSpeedConfig animationSpeedConfig;
    public CardContainer container;

    // Arrow variable:
    private GameObject ArrowHeadPrefab;
    private GameObject ArrowNodePrefab;
    private int arrowNodeNum = 11;
    private float scaleFactor = 2f;

    private RectTransform origin;
    private List<RectTransform> arrowNodes = new List<RectTransform>();
    private List<Vector2> controlPoints = new List<Vector2>();
    private readonly List<Vector2> controlPointFactors = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };

    // other private variable
    private bool isHovered;
    private bool isDragged;
    private Vector2 dragStartPos;
    public EventsConfig eventsConfig;

    // 决定卡牌类型为target释放/nontarget释放
    private CardBehavior cardBehavior;
    private bool targetCard;

    public float width {
        get => rectTransform.rect.width * rectTransform.localScale.x;
    }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        canvas = GetComponent<Canvas>();
        cardBehavior = GetComponent<CardBehavior>();
        targetCard = cardBehavior.targetCard;

        // 加载箭头prefab
        ArrowHeadPrefab = Resources.Load<GameObject>("Arrow/ArrowHead");
        ArrowNodePrefab = Resources.Load<GameObject>("Arrow/ArrowNode");
    }

    private void Update() {
        if (!InGameStateManager.gamePased)
        {
            UpdateRotation();
            UpdateArrow();
            UpdatePosition();
            UpdateScale();
            UpdateUILayer();
            
        }
    }

    private void UpdateUILayer() {
        if (!isHovered && !isDragged) {
            canvas.sortingOrder = uiLayer;
        }
    }

    private void UpdatePosition() {
        if (!isDragged) {
            var target = new Vector2(targetPosition.x, targetPosition.y + targetVerticalDisplacement);
            if (isHovered && zoomConfig.overrideYPosition != -1) {
                target = new Vector2(target.x, zoomConfig.overrideYPosition);
            }

            var distance = Vector2.Distance(rectTransform.position, target);
            var repositionSpeed = rectTransform.position.y > target.y || rectTransform.position.y < 0
                ? animationSpeedConfig.releasePosition
                : animationSpeedConfig.position;
            rectTransform.position = Vector2.Lerp(rectTransform.position, target,
                repositionSpeed / distance * Time.deltaTime);
        }
    }

    private void UpdateArrow()
    {
        if (!isDragged)
        {
            return;
        }

        if (!targetCard)
        {
            var delta = ((Vector2)Input.mousePosition + dragStartPos);
            rectTransform.position = new Vector2(delta.x, delta.y);
        }
        else if (targetCard)
        {
            // P0 is at the arrow emitter point.
            this.controlPoints[0] = new Vector2(this.origin.position.x, this.origin.position.y);

            // P3 is at the mouse position.
            this.controlPoints[3] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // P1, P2 determines by P0 and P3.
            // P1 = P0 + (P3 - P0) * Vector2(-0.3f, 0.8f)
            // P2 = P0 + (P3 - P0) * Vector2(0.1f, 1.4f)
            this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[0];
            this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[1];

            for (int i = 0; i < this.arrowNodes.Count; ++i)
            {
                // Calculates t.
                // 适应箭头位置修改node的t
                var t = Mathf.Log(1f * i / (this.arrowNodes.Count - 0.5f) + 1f, 2f);

                if (i == (this.arrowNodes.Count - 1))
                {
                    t = Mathf.Log(1f * i / (this.arrowNodes.Count - 1) + 1f, 2f);
                }

                // Cubic Bezier curve
                // B(t) = (1-t)^3 * P0 + 3 * (1-t)^2 * t * P1 + 3 * (1-t) * t^2 * P2 + t^3 * P3
                this.arrowNodes[i].position =
                    Mathf.Pow(1 - t, 3) * this.controlPoints[0] +
                    3 * Mathf.Pow(1 - t, 2) * t * this.controlPoints[1] +
                    3 * (1 - t) * Mathf.Pow(t, 2) * this.controlPoints[2] +
                    Mathf.Pow(t, 3) * this.controlPoints[3];

                // Calculates rotations for each arrow node.
                if (i > 0)
                {
                    var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i - 1].position));
                    this.arrowNodes[i].rotation = Quaternion.Euler(euler);
                }

                // Calculates scales for each arrow node.
                var scale = this.scaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
                this.arrowNodes[i].localScale = new Vector3(scale, scale, 1f);

            }

            // The first arrow node's rotation.
            this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
        }
    }

    private void UpdateScale() {
        var targetZoom = (isDragged || isHovered) && zoomConfig.zoomOnHover ? zoomConfig.multiplier : 1;
        var delta = Mathf.Abs(rectTransform.localScale.x - targetZoom);
        var newZoom = Mathf.Lerp(rectTransform.localScale.x, targetZoom,
            animationSpeedConfig.zoom / delta * Time.deltaTime);
        rectTransform.localScale = new Vector3(newZoom, newZoom, 1);
    }

    private void UpdateRotation() {
        var crtAngle = rectTransform.rotation.eulerAngles.z;
        // If the angle is negative, add 360 to it to get the positive equivalent
        crtAngle = crtAngle < 0 ? crtAngle + 360 : crtAngle;
        // If the card is hovered and the rotation should be reset, set the target rotation to 0
        var tempTargetRotation = (isHovered || isDragged) && zoomConfig.resetRotationOnZoom
            ? 0
            : targetRotation;
        tempTargetRotation = tempTargetRotation < 0 ? tempTargetRotation + 360 : tempTargetRotation;
        var deltaAngle = Mathf.Abs(crtAngle - tempTargetRotation);
        if (!(deltaAngle > EPS)) return;

        // Adjust the current angle and target angle so that the rotation is done in the shortest direction
        var adjustedCurrent = deltaAngle > 180 && crtAngle < tempTargetRotation ? crtAngle + 360 : crtAngle;
        var adjustedTarget = deltaAngle > 180 && crtAngle > tempTargetRotation
            ? tempTargetRotation + 360
            : tempTargetRotation;
        var newDelta = Mathf.Abs(adjustedCurrent - adjustedTarget);

        var nextRotation = Mathf.Lerp(adjustedCurrent, adjustedTarget,
            animationSpeedConfig.rotation / newDelta * Time.deltaTime);
        rectTransform.rotation = Quaternion.Euler(0, 0, nextRotation);
    }


    public void SetAnchor(Vector2 min, Vector2 max) {
        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (isDragged) {
            // Avoid hover events while dragging
            return;
        }
        if (zoomConfig.bringToFrontOnHover) {
            canvas.sortingOrder = zoomConfig.zoomedSortOrder;
        }

        eventsConfig?.OnCardHover?.Invoke(new CardHover(this));
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (isDragged) {
            // Avoid hover events while dragging
            return;
        }
        canvas.sortingOrder = uiLayer;
        isHovered = false;
        eventsConfig?.OnCardUnhover?.Invoke(new CardUnhover(this));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isDragged)
            {
                PointUp(false);
                return;
            }

            isDragged = true;

            cardBehavior.OnPointDown();

            if (targetCard)
            {
                // Gets position of the arrows emitter point.
                this.origin = this.GetComponent<RectTransform>();

                // Instantiates the arrow nodes and arrow head.
                for (int i = 0; i < this.arrowNodeNum; ++i)
                {
                    this.arrowNodes.Add(Instantiate(this.ArrowNodePrefab, this.transform).GetComponent<RectTransform>());
                }

                this.arrowNodes.Add(Instantiate(this.ArrowHeadPrefab, this.transform).GetComponent<RectTransform>());

                // Hides the arrow nodes.
                this.arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

                // Initializes the control points list.
                for (int i = 0; i < 4; ++i)
                {
                    this.controlPoints.Add(Vector2.zero);
                }
            }

            dragStartPos = new Vector2(transform.position.x - eventData.position.x,
                transform.position.y - eventData.position.y);
            container.OnCardDragStart(this);
            eventsConfig?.OnCardUnhover?.Invoke(new CardUnhover(this));
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 但处于拖动时点击，则取消
            PointUp(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PointUp(false);
            OnPointerDown(eventData);
        }
    }

    private void PointUp(bool canceled = false)
    {
        // 防止取消释放后依然通过松开右键释放卡牌
        if (isDragged == false)
        {
            return;
        }

        isDragged = false;
        isHovered = false;

        cardBehavior.OnPointUp();

        // Destroy all arrow nodes
        foreach (RectTransform arrowNode in arrowNodes)
        {
            Destroy(arrowNode.gameObject);
        }
        arrowNodes.Clear();  // Clear the list after destroying the objects

        container.OnCardDragEnd(canceled);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerDown(eventData);
    }
}
