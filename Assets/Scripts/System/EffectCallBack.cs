using UnityEngine;

public class EffectCallBack : MonoBehaviour
{
    [SerializeField] EffectType _tag = EffectType.None;

    private void OnParticleSystemStopped()
    {
        EffectPool.Instance.RetrieveEffect(_tag, gameObject);
    }
}
