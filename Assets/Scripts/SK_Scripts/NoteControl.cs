using System.Collections;
using UnityEngine;

public class NoteControl : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] private GameObject _miniNote = null;
    [SerializeField] private float _movementSped = 3f;
    [SerializeField] private float _score = 100f;
    [SerializeField] private float _timeBeforeTerminate = 3f;
    [SerializeField] private float _hitPointRadius = 0.2f;
    [SerializeField] private float _triggerVelocity = 2f;
    [SerializeField] private GameObject effect = null;
    #endregion
    #region Private Field
    private bool _isHit = false;
    #endregion

    #region Properties
    public float Score { get => _score; }
    public float HitPointRadius { get => _hitPointRadius; }
    public float TriggerVelocity { get => _triggerVelocity; }
    public bool IsHit { get => _isHit; }
    #endregion

    void Start()
    {
        PositionMiniNote();
        direction = (target.transform.position - transform.position).normalized *fastSpeed;
    }


    #region Initialize
    private void PositionMiniNote()
    {
        Transform notePosition = transform.GetChild(Random.Range(1, 6));
        notePosition.gameObject.SetActive(true);
        _miniNote.transform.position = notePosition.position;
        _miniNote.transform.rotation = notePosition.rotation;
    }
    #endregion

    Vector3 direction;
    public Transform target;
    public float fastSpeed = 20.0f;

    private void Update()
    {
        if (_isHit) return;

        transform.position += direction * Time.deltaTime;

        if (!GameStateChanger.Instance.CurrentGameState.Equals(GameState.GameOn))
        {
            gameObject.SetActive(false);
            _isHit = false;
            ObjPoolingManager.instance.NoteSetQueue_Right(gameObject);
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= 0.1f)
        {
            effect.SetActive(false);
            direction = -Vector3.forward * _movementSped;
        }
    }

    #region Terminate
    public void GotHit()
    {
        _isHit = true;
        StartCoroutine(TerminateAfterSeconds(_timeBeforeTerminate));
    }

    private IEnumerator TerminateAfterSeconds(float time)
    {

        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
        effect.SetActive(true);
        _isHit = false;
        ObjPoolingManager.instance.NoteSetQueue_Right(gameObject);
    }
    #endregion

}
