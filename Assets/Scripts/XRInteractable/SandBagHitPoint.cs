using UnityEngine;

public class SandBagHitPoint : MonoBehaviour
{
    #region Public Field
    public delegate void ActivateHitFunction(HitFunctions func);
    public event ActivateHitFunction onHitSandBag = null;
    #endregion

    #region Serialize Field
    [Space, Header("Event Function")]
    [SerializeField] private HitFunctions _selectedHitFunc = HitFunctions.Play;
    #endregion

    #region Private Field
    private float _eventTriggerValue = 0f;
    private BoxingBag BoxingBag = null;
    #endregion

    private void Start()
    {
        BoxingBag = GetComponentInParent<BoxingBag>();
    }

    private void OnTriggerEnter(Collider other)
    {
        LayerMask handLayer = 1 << 9;

        if ((handLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (!other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody hand)) return;
            TiggerEvent(hand);
        }
    }

    #region Invoke Event
    private void TiggerEvent(Rigidbody hand)
    {
        BoxingBag.ActivateFlicker();

        if (_selectedHitFunc.Equals(HitFunctions.Play))
        {
            _eventTriggerValue = BoxingBag.PlayTriggerValue;
            if (hand.velocity.magnitude > _eventTriggerValue)
            {
                BoxingBag.DisconnectBoxingBag(hand);
                onHitSandBag?.Invoke(_selectedHitFunc);
            }
        }
        else
        {
            _eventTriggerValue = BoxingBag.HitTriggerValue;
            if (hand.velocity.magnitude < 6f && hand.velocity.magnitude > _eventTriggerValue)
            {
                onHitSandBag?.Invoke(_selectedHitFunc);
            }
        }
    }
    #endregion
}

public enum HitFunctions { Previous, Play, Next }