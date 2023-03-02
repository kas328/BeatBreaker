using UnityEngine;

public class PhysicsButton : MonoBehaviour
{
    #region Public Field
    public delegate void ActivateLevelSelectFunction(LevelSelectFunctions func);
    public event ActivateLevelSelectFunction onButtonPush = null;

    public delegate void ActivateKeyboardKeyFunction(KeyboardKeys key, Vector3 pos);
    public event ActivateKeyboardKeyFunction onKeyPush = null;
    #endregion

    #region Serialize Field
    [SerializeField] private LevelSelectFunctions _selectedFunction = LevelSelectFunctions.None;
    [SerializeField] private KeyboardKeys _selectedKey = KeyboardKeys.None;
    [SerializeField] private float _pushPointOffset = -0.3f;

    [Space, Header("Spring Settings")]
    [SerializeField] private float _springPower = 1300f;
    #endregion

    #region Private Field
    private GameObject _btn = null;
    private Vector3 _originPos = Vector3.zero;
    private Vector3 _pushPoint = Vector3.zero;
    private bool _isButtonPushing = false;
    private float _distance = 0f;
    private float _currentDistance = 0f;
    #endregion

    private void Awake()
    {
        InitVariables();
        ButtonSettings();
    }

    #region Initialize
    private void InitVariables()
    {
        _btn = transform.GetChild(0).gameObject;
        _originPos = _btn.transform.position;
        _pushPoint = _btn.transform.position + _btn.transform.up * _pushPointOffset;
        _distance = Vector3.Distance(_originPos, _pushPoint);
    }

    private void ButtonSettings()
    {
        SpringJoint joint = _btn.GetComponent<SpringJoint>();
        joint.spring = _springPower;
        joint.tolerance = 0f;
        joint.damper = 0.2f;
        joint.enablePreprocessing = true;
    }
    #endregion

    private void Update()
    {
        IsButtonPushed();
    }

    #region Invoke Event
    private void IsButtonPushed()
    {
        _currentDistance = Vector3.Distance(_btn.transform.position, _pushPoint);
        if (_currentDistance < _distance * 0.5f && !_isButtonPushing)
        {
            _isButtonPushing = true;

            if (!_selectedFunction.Equals(LevelSelectFunctions.None))
            {
                onButtonPush?.Invoke(_selectedFunction);
            }
            else if (!_selectedKey.Equals(KeyboardKeys.None))
            {
                onKeyPush?.Invoke(_selectedKey, _btn.transform.position);
            }
        }
        else if (_currentDistance > _distance * 0.6f && _isButtonPushing)
        {
            _isButtonPushing = false;
        }
    }
    #endregion

    private void FixedUpdate()
    {
        //ButtonPushLimit();
    }

    #region Button Limitations
    private void ButtonPushLimit()
    {
        if (_currentDistance < _distance * 0.55f)
        {
            _btn.transform.position = _btn.transform.up * _distance * 0.55f;
        }
        if (_currentDistance > _distance * 0.61f)
        {
            _btn.transform.position = _originPos * 0.61f;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_pushPoint, 0.03f);

        Gizmos.DrawWireSphere(_originPos, 0.03f);
    }
}

public enum LevelSelectFunctions { None, Play, Previous, Next }
public enum KeyboardKeys
{
    None, Q, W, E, R, T, Y, U, I, O, P,
    A, S, D, F, G, H, J, K, L,
    Z, X, C, V, B, N, M, UnderBar,
    Delete, Enter, Menu
}