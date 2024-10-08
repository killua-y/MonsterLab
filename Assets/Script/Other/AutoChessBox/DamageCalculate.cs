using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCalculate : MonoBehaviour
{
    public GameObject dealDamageBoard;
    public GameObject takeDamageBoard;
    public GameObject damageSliderPrefab;
    public TextMeshProUGUI Title;
    private Dictionary<BaseEntity, DamageSliderBehavior> dealDamageSliders;
    private int maxDealDamage;
    private Dictionary<BaseEntity, DamageSliderBehavior> takeDamageSliders;
    private int maxTakeDamage;

    private bool isOpen;
    private float closePosition;
    private float openPosition;
    // Start is called before the first frame update

    private void Awake()
    {
        isOpen = false;
        closePosition = transform.localPosition.x;
        openPosition = 965f;
    }

    private void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattlePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        BattleManager.Instance.OnUnitTakingDamage += OnUnitTakingDamage;
    }

    void OnUnitTakingDamage(int amount, DamageType damageType, BaseEntity from, BaseEntity to)
    {
        if ((!InGameStateManager.BattelPhase) || (from == null))
        {
            return;
        }

        // 更新造成伤害面板
        if (from.myTeam == Team.Player)
        {
            if ((dealDamageSliders[from].amount + amount) > maxDealDamage)
            {
                maxDealDamage = (dealDamageSliders[from].amount + amount);
                UpdateMaxDamage(dealDamageSliders, maxDealDamage);
            }

            dealDamageSliders[from].UpdateDamageSlide(amount);
        }
        // 更新承受伤害面板
        else if (from.myTeam == Team.Enemy)
        {
            if ((takeDamageSliders[to].amount + amount) > maxTakeDamage)
            {
                maxTakeDamage = (takeDamageSliders[to].amount + amount);
                UpdateMaxDamage(takeDamageSliders, maxTakeDamage);
            }

            takeDamageSliders[to].UpdateDamageSlide(amount);
        }
        else
        {
            Debug.Log("Should not get here");
        }

        UpdateOrder();
    }

    void UpdateMaxDamage(Dictionary<BaseEntity, DamageSliderBehavior> damageSliders, int maxDamage)
    {
        foreach (var slider in damageSliders)
        {
            slider.Value.UpdateMaxDamage(maxDamage);
        }
    }

    void UpdateOrder()
    {
        dealDamageBoard.GetComponent<DamageBoarderBehavior>().UpdateLayout();
        takeDamageBoard.GetComponent<DamageBoarderBehavior>().UpdateLayout();
    }

    void OnBattlePhaseStart()
    {
        foreach (Transform child in dealDamageBoard.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in takeDamageBoard.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (BaseEntity baseEntity in BattleManager.Instance.GetEntitiesAgainst(Team.Enemy))
        {
            // 造成伤害
            DamageSliderBehavior dealDamageSlider = Instantiate(damageSliderPrefab, dealDamageBoard.transform).GetComponent<DamageSliderBehavior>();
            dealDamageSlider.UpdateSmallIcon(baseEntity.cardModel);
            dealDamageSliders.Add(baseEntity, dealDamageSlider);

            // 承受伤害
            DamageSliderBehavior takeDamageSlider = Instantiate(damageSliderPrefab, takeDamageBoard.transform).GetComponent<DamageSliderBehavior>();
            takeDamageSlider.UpdateSmallIcon(baseEntity.cardModel);
            takeDamageSliders.Add(baseEntity, takeDamageSlider);
        }

        if (!isOpen)
        {
            ChangePosition();
        }
    }

    void OnPreparePhaseStart()
    {
        dealDamageSliders = new Dictionary<BaseEntity, DamageSliderBehavior>();
        maxDealDamage = 0;
        takeDamageSliders = new Dictionary<BaseEntity, DamageSliderBehavior>();
        maxTakeDamage = 0;
    }

    void OnCombatStart()
    {
        foreach (Transform child in dealDamageBoard.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in takeDamageBoard.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangePosition()
    {
        if (isOpen)
        {
            StartCoroutine(SmoothMoveCoroutine(openPosition, closePosition));
            isOpen = false;
        }
        else
        {
            StartCoroutine(SmoothMoveCoroutine(closePosition, openPosition));
            isOpen = true;
        }
    }

    private IEnumerator SmoothMoveCoroutine(float startX, float endX)
    {
        float elapsedTime = 0;
        float duration = 0.2f;
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(endX, startPosition.y, startPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the current position using Lerp
            float newX = Mathf.Lerp(startX, endX, elapsedTime / duration);
            transform.localPosition = new Vector3(newX, startPosition.y, startPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set
        transform.localPosition = targetPosition;
    }

    public void ShowDealDamageBoard()
    {
        dealDamageBoard.SetActive(true);
        takeDamageBoard.SetActive(false);
        Title.text = "Damage Dealt";
    }

    public void ShowTakeDamageBoard()
    {
        dealDamageBoard.SetActive(false);
        takeDamageBoard.SetActive(true);
        Title.text = "Damage Took";
    }
}
