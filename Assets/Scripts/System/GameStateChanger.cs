using OVR;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using VRCollie;

public class GameStateChanger : Singleton<GameStateChanger>
{
    #region Serialized Field
    [SerializeField] private GameObject _resultScreen = null;
    #endregion

    #region Private Field
    private GameObject _player = null;
    private LocomotionProvider _locomotionProvider = null;
    private MovementProvider _movementProvider = null;

    private GameState _currentGameState = GameState.MapSelection;
    private bool _isGameOn = false;
    private SoundFXRef _selectedSong = null;
    private float _remainingGameTime = 0f;
    private TimeSpan _timeSpan;

    private Color _normalColor = new Color(122f / 255f, 163f / 255f, 195f / 255f); // from zero to one value
    private Color _feverColor = new Color(0, 0, 0);
    #endregion

    public GameObject SongCover { get; set; } = null;
    public GameState CurrentGameState { get => _currentGameState; }

    private void Awake()
    {
        InitVariables();
        GetComponents();
    }

    #region Initialize
    private void InitVariables()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void GetComponents()
    {
        _locomotionProvider = _player.GetComponent<LocomotionProvider>();
        _movementProvider = _player.GetComponent<MovementProvider>();
    }
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateGameState;
    }

    #region Scene Events
    private void UpdateGameState(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) _currentGameState = GameState.Load;
        else if (scene.buildIndex == 1) _currentGameState = GameState.MapSelection;
        else if (scene.buildIndex > 1) _currentGameState = GameState.GameOn;

        switch (_currentGameState)
        {
            case GameState.Load:
                SetPlayerMovementTo(false);
                break;

            case GameState.MapSelection:
                _resultScreen.SetActive(false);
                SetPlayerMovementTo(true);
                break;

            case GameState.GameOn:
                SetPlayerMovementTo(false);

                _remainingGameTime = _selectedSong.GetClipLength(0);
                //_remainingGameTime = 30f; // testing
                _selectedSong.PlaySoundAt(_player.transform.position +
                    _player.transform.forward * -0.6f, 6.52f);
                break;
        }
    }

    private void SetPlayerMovementTo(bool condition)
    {
        _locomotionProvider.enabled = condition;
        _movementProvider.enabled = condition;
        _player.transform.position = Vector3.zero;
    }
    #endregion

    private void Update()
    {
        switch (_currentGameState)
        {
            case GameState.GameOn:
                if (!_isGameOn) return;

                _remainingGameTime -= Time.deltaTime;
                _timeSpan = TimeSpan.FromSeconds(_remainingGameTime);

                if (ScoreManager.Instance.CurrentCombo > 10)
                {
                    RenderSettings.fogColor = _feverColor;
                }
                else
                {
                    //61788A , 7AA3C3
                    //ColorUtility.TryParseHtmlString("7AA3C3", out _normalColor);
                    RenderSettings.fogColor = _normalColor;
                }

                if (_remainingGameTime <= 0)
                {
                    _selectedSong.StopSound();
                    _currentGameState = GameState.Result;
                }
                break;

            case GameState.Result:
                _resultScreen.SetActive(true);
                break;
        }
    }

    #region Public Methods
    /// <summary>
    /// This method will set the game play song.
    /// This should be called right before the selected game scene is loaded. 
    /// </summary>
    public void SetGameSong(SoundFXRef song)
    {
        _selectedSong = song;
    }

    public string GetTime()
    {
        return _timeSpan.ToString("mm'.'ss'.'ff");
    }

    public string GetTitle()
    {
        return _selectedSong.name;
    }

    // needs change
    public void GameOn() { _isGameOn = true; }

    public bool IsGameOn() { return _isGameOn; }
    #endregion

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdateGameState;
    }
}

public enum GameState { Load, MapSelection, GameOn, Result }