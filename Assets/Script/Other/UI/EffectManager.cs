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
        public int poolSize;
    }

    public List<EffectPool> effectPools;
    private Dictionary<string, Queue<GameObject>> effectPoolDictionary;

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

        effectToPlay.transform.position = position;
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
