using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject damageText;
    public Canvas canvas; // Reference to the Canvas

    [System.Serializable]
    public class EffectPool
    {
        public string effectName;
        public GameObject effectPrefab;
        public Vector2 effectOffset;
        public int poolSize;
    }

    public List<EffectPool> effectPools;
    private Dictionary<string, Queue<GameObject>> effectPoolDictionary;
    private Dictionary<string, Vector2> effectPositionOffset;

    // Start is called before the first frame update
    void Start()
    {
        if (canvas == null)
        {
            canvas = this.GetComponent<Canvas>();
        }

        InitializeEffectPools();
    }

    public void GenerateDamageText(Vector2 position, int amount)
    {
        // Convert world position to screen position
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);

        // Convert screen position to canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out Vector2 canvasPosition);

        // 生成伤害
        GameObject instance = Instantiate(damageText, canvas.transform);
        instance.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        instance.GetComponent<DamageTextBehavior>().Setup(amount);
    }

    private void InitializeEffectPools()
    {
        effectPoolDictionary = new Dictionary<string, Queue<GameObject>>();
        effectPositionOffset = new Dictionary<string, Vector2>();

        foreach (EffectPool pool in effectPools)
        {
            Queue<GameObject> effectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject effectObject = Instantiate(pool.effectPrefab);
                effectObject.SetActive(false);
                effectPool.Enqueue(effectObject);
            }

            effectPoolDictionary.Add(pool.effectName, effectPool);

            if (!effectPositionOffset.ContainsKey(pool.effectName))
            {
                effectPositionOffset.Add(pool.effectName, pool.effectOffset);
            }
        }
    }

    public void PlayEffect(string effectName, Vector2 position)
    {
        if (!effectPoolDictionary.ContainsKey(effectName))
        {
            Debug.LogWarning("EffectManager: Effect " + effectName + " doesn't exist in the pool.");
            return;
        }

        GameObject effectToPlay = effectPoolDictionary[effectName].Dequeue();

        // 根据offse和传入位置修改位置
        effectToPlay.transform.position = position + effectPositionOffset[effectName];
        effectToPlay.SetActive(true);

        StartCoroutine(ReturnEffectToPool(effectName, effectToPlay, effectToPlay.GetComponent<ParticleSystem>().main.duration));
    }

    private IEnumerator ReturnEffectToPool(string effectName, GameObject effectObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        effectObject.SetActive(false);
        effectPoolDictionary[effectName].Enqueue(effectObject);
    }

}
