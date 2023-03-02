using System.Collections;
using System.Collections.Generic;
using OVR;
using UnityEngine;

public class BoxingBag : MonoBehaviour
{
    #region Serialize Field
    [Space, Header("Boxing Bag")]
    [SerializeField] private Rigidbody ForceTarget = null;
    [SerializeField] private float _hitTriggerValue = 2f;
    [SerializeField] private float _playTriggerValue = 6f;
    [SerializeField] private ConfigurableJoint _rootChain = null;
    [SerializeField] private SpringJoint _springChain = null;
    [SerializeField] private float _breakForce = 6f;

    [Space, Header("Flicker")]
    [SerializeField] private GameObject FlickerTarget = null;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private float _minValue = 0f;
    [SerializeField] private float _maxValue = 1f;
    [Range(1, 50)] [SerializeField] private int _smoothing = 5;

    [Space, Header("Audio")]
    [SerializeField] SoundFXRef ChainBreakSound = null;
    [SerializeField] SoundFXRef ChainSound = null;
    #endregion

    #region Private Field
    private Queue<float> _smoothQueue = null;
    private float _smoothSum = 0f;
    private Renderer _renderer = null;
    private Color _color;
    #endregion

    #region Properties
    public float HitTriggerValue { get => _hitTriggerValue; }
    public float PlayTriggerValue { get => _playTriggerValue; }
    #endregion

    private void Awake()
    {
        Initialize();
    }

    #region Initialize
    private void Initialize()
    {
        _smoothQueue = new Queue<float>(_smoothing);
        FlickerTarget.TryGetComponent<Renderer>(out _renderer);
        _color = _renderer.material.color;
    }
    #endregion

    #region Light Flicker
    public void ActivateFlicker()
    {
        StopAllCoroutines();
        StartCoroutine(Flick(_duration));
    }

    private IEnumerator Flick(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            RandomAverage(_renderer, _color);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private Color RandomAverage(Renderer renderer, Color color)
    {
        while (_smoothQueue.Count >= _smoothing)
        {
            _smoothSum -= _smoothQueue.Dequeue();
        }
        float randomValue = Random.Range(_minValue, _maxValue);
        _smoothQueue.Enqueue(randomValue);
        _smoothSum += randomValue;
        print(_smoothSum);
        renderer.material.SetColor("_EmissionColor", color * _smoothSum);
        return color;
    }
    #endregion

    #region Break Joint
    public void DisconnectBoxingBag(Rigidbody rb)
    {
        _rootChain.breakForce = 1f;
        _springChain.breakForce = 1f;
        ForceTarget.drag = 0f;
        ForceTarget.AddForceAtPosition(rb.velocity * _breakForce, rb.position, ForceMode.Impulse);
        ChainBreakSound.PlaySoundAt(_rootChain.transform.position);
    }
    #endregion
}