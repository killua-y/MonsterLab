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

    private bool isOpen = true;
    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattlePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        BattleManager.Instance.OnUnitTakingDamage += OnUnitTakingDamage;
    }

    void OnUnitTakingDamage(BaseEntity from, BaseEntity to, int amount)
    {
        // 更新造成伤害面板
        if (from.myTeam == Team.Player)
        {
            if ((dealDamageSliders[from].amount + amount) > maxDealDamage)
            {
                maxDealDamage = (dealDamageSliders[from].amount + amount);
            }

            dealDamageSliders[from].UpdateDamageSlide(maxDealDamage, amount);
        }
        // 更新承受伤害面板
        else if (from.myTeam == Team.Enemy)
        {
            if ((takeDamageSliders[to].amount + amount) > maxTakeDamage)
            {
                maxTakeDamage = (takeDamageSliders[to].amount + amount);
            }

            takeDamageSliders[to].UpdateDamageSlide(maxTakeDamage, amount);
        }
        else
        {
            Debug.Log("Should not get here");
        }

        UpdateOrder();
    }

    void UpdateOrder()
    {
        dealDamageBoard.GetComponent<DamageBoarderBehavior>().SetLayoutVertical();
        takeDamageBoard.GetComponent<DamageBoarderBehavior>().SetLayoutVertical();
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
            dealDamageSlider.UpdateSmallIcon(baseEntity.cardModel.smallIconLocation);
            dealDamageSliders.Add(baseEntity, dealDamageSlider);

            // 承受伤害
            DamageSliderBehavior takeDamageSlider = Instantiate(damageSliderPrefab, takeDamageBoard.transform).GetComponent<DamageSliderBehavior>();
            takeDamageSlider.UpdateSmallIcon(baseEntity.cardModel.smallIconLocation);
            takeDamageSliders.Add(baseEntity, takeDamageSlider);
        }
    }

    void OnPreparePhaseStart()
    {
        dealDamageSliders = new Dictionary<BaseEntity, DamageSliderBehavior>();
        maxDealDamage = 0;
        takeDamageSliders = new Dictionary<BaseEntity, DamageSliderBehavior>();
        maxTakeDamage = 0;
    }

    public void ChangePosition()
    {
        if (isOpen)
        {
            StartCoroutine(SmoothMoveCoroutine(956, 1160));
            isOpen = false;
        }
        else
        {
            StartCoroutine(SmoothMoveCoroutine(1160, 956));
            isOpen = true;
        }
    }

    private IEnumerator SmoothMoveCoroutine(float startX, float endX)
    {
        float elapsedTime = 0;
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(endX, startPosition.y, startPosition.z);

        while (elapsedTime < 0.5f)
        {
            // Calculate the current position using Lerp
            float newX = Mathf.Lerp(startX, endX, elapsedTime / 0.5f);
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
