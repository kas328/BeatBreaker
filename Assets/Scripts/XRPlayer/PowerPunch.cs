using OVR;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PowerPunch : MonoBehaviour
{
    #region Serialize Field
    [Space, Header("Settings")]
    [SerializeField] private float _punchForce = 100f;
    [Range(0, 1)] [SerializeField] private float _reducePercentage = 0.6f;
    [SerializeField] private float _swingTriggerVale = 3f;
    [SerializeField] private XRController _controller = null;

    [Space, Header("Audio")]
    [SerializeField] private SoundFXRef _goodPunch = null;
    [SerializeField] private SoundFXRef _shadowPunch = null;
    [SerializeField] private SoundFXRef _badPunch = null;
    [SerializeField] private SoundFXRef _swing = null;
    #endregion

    #region Private Field
    private Rigidbody _rb = null;
    private float _finalScore = 0f;
    private bool _isCoroutineRunning = false;
    #endregion

    private void Awake()
    {
        GetComponents();
    }

    #region Initialize
    private void GetComponents()
    {
        _rb = GetComponent<Rigidbody>();
    }
    #endregion

    private void Update()
    {
        if (_rb.velocity.magnitude > _swingTriggerVale)
        {
            StartCoroutine(Swing());
        }
    }

    #region Swing
    private IEnumerator Swing()
    {
        if (_isCoroutineRunning) yield break;
        _isCoroutineRunning = true;

        _swing.PlaySoundAt(transform.position);
        yield return new WaitForSeconds(0.8f);

        _isCoroutineRunning = false;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        LayerMask beat = 1 << 10;
        LayerMask boxingBag = 1 << 20;

        if (((1 << collision.gameObject.layer) & beat) != 0)
        {
            GiveHaptic(_controller.inputDevice, 0.8f, 0.5f);
            ApplyForce(collision);
        }
        else if ((boxingBag & (1 << collision.gameObject.layer)) != 0)
        {
            if (_rb.velocity.magnitude > 1f)
            {
                EffectPool.Instance.PlayEffect(EffectType.punch, collision.contacts[0].point, -_rb.transform.forward);
                _goodPunch.PlaySoundAt(collision.contacts[0].point);
                _shadowPunch.PlaySoundAt(collision.contacts[0].point);
                GiveHaptic(_controller.inputDevice, 0.6f, 0.5f);
            }
        }
    }

    #region Punch Note
    /// <summary>
    /// Apply force and calculate final score that will be added to total score.
    /// </summary>
    private void ApplyForce(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody noteRB)) return;

        // Letting the collided note that it got hit to stop the movement.
        collision.gameObject.TryGetComponent<NoteControl>(out NoteControl noteControl);
        noteControl.GotHit();

        Transform hitPoint = collision.transform.GetChild(0);
        Vector3 collisionPoint = collision.contacts[0].point;
        float dstFromHitPoint = Vector3.Distance(hitPoint.position, collisionPoint);

        float score = noteControl.Score * 0.5f;
        Vector3 punchVelocity = _rb.velocity.normalized * _punchForce;

        if (_rb.velocity.magnitude > noteControl.TriggerVelocity)
        {
            EffectPool.Instance.PlayEffect(EffectType.punch, collision.contacts[0].point, -_rb.transform.forward);
            _goodPunch.PlaySoundAt(collisionPoint);
            _shadowPunch.PlaySoundAt(collisionPoint);

            if (dstFromHitPoint < noteControl.HitPointRadius)
            {
                float dotValue = Vector3.Dot(-hitPoint.up, _rb.velocity.normalized);
                _finalScore += score + Mathf.Clamp(score * dotValue, 0f, score);
                ScoreManager.Instance.AddScore(_finalScore);

                noteRB.AddForceAtPosition(punchVelocity, collisionPoint, ForceMode.Impulse);
            }
            else
            {
                _finalScore = score * Mathf.InverseLerp(0, 1, dstFromHitPoint);
                ScoreManager.Instance.AddScore(_finalScore);

                noteRB.AddForceAtPosition(punchVelocity * _reducePercentage, collisionPoint, ForceMode.Impulse);
            }
        }
        else
        {
            _badPunch.PlaySoundAt(collisionPoint);

            ScoreManager.Instance.Missed();
        }
    }
    #endregion

    // this needs to be move to input
    #region Punch Boxing Bag
    private void GiveHaptic(InputDevice device, float amplitude, float duration)
    {
        if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                device.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
    #endregion
}