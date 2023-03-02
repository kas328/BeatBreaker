using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : Singleton<EffectPool>
{
    #region Serialize Field
    [Space, Header("Effects")]
    [SerializeField] private List<Pool> _pools = null;
    [SerializeField] private Dictionary<string, Queue<GameObject>> _pool = null;
    #endregion

    private void Awake()
    {
        InitVariables();
        StartCoroutine(CreatePools());
    }

    #region Initialize
    private void InitVariables()
    {
        _pool = new Dictionary<string, Queue<GameObject>>();
    }

    private IEnumerator CreatePools()
    {
        foreach (Pool pool in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                int randomIndex = Random.Range(0, pool.Prefabs.Count - 1);
                GameObject obj = Instantiate(pool.Prefabs[randomIndex]);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                yield return null;
            }
            _pool.Add(pool.Tag.ToString(), objectPool);
        }
    }
    #endregion

    #region Public Methods
    public GameObject PlayEffect(EffectType tag, Vector3 position, Vector3 forward)
    {
        string sTag = tag.ToString();
        if (!_pool.ContainsKey(sTag) || _pool[sTag].Count <= 0) return null;

        GameObject effect = _pool[sTag].Dequeue();
        effect.SetActive(true);
        effect.transform.position = position;
        effect.transform.forward = forward;

        return effect;
    }

    public void RetrieveEffect(EffectType tag, GameObject effect)
    {
        string sTag = tag.ToString();

        effect.SetActive(false);
        _pool[sTag].Enqueue(effect);
    }
    #endregion
}

[System.Serializable]
public class Pool
{
    #region Serialize Field
    [Space, Header("Pool")]
    [SerializeField] private EffectType _tag = EffectType.None;
    [SerializeField] private List<GameObject> _Prefabs = null;
    [SerializeField] private int _size = 0;
    #endregion

    #region Properties
    public EffectType Tag { get => _tag; set => _tag = value; }
    public List<GameObject> Prefabs { get => _Prefabs; set => _Prefabs = value; }
    public int Size { get => _size; set => _size = value; }
    #endregion
}

public enum EffectType { None, punch }