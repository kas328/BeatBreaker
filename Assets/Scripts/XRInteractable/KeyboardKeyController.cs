using System;
using System.Collections;
using System.Collections.Generic;
using OVR;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class KeyboardKeyController : MonoBehaviour
{
    #region Serialize Field
    [Space, Header("Score Panel")]
    [SerializeField] private Transform _songCoverAnchor = null;
    [SerializeField] private TextMeshProUGUI _score = null;
    [SerializeField] private TextMeshProUGUI _output = null;
    [SerializeField] private int _charLimit = 8;

    [Space, Header("Rank Panel")]
    [SerializeField] private TextMeshProUGUI _songTitle = null;
    [SerializeField] private TextMeshProUGUI _rank = null;

    [Space, Header("Detail Panel")]
    [SerializeField] private TextMeshProUGUI _detail = null;

    [Space, Header("Audio")]
    [SerializeField] SoundFXRef _click = null;
    #endregion

    #region Private Field
    private PhysicsButton[] _physicsButtons = null;
    private Rigidbody[] childRigidbodies = null;

    // in testing
    private ScreenFader _fader = null;
    [SerializeField] private Volume _volume = null;
    private DepthOfField _dof = null;

    private bool _isEnterPressed = false;
    #endregion

    private void Awake()
    {
        IgnoreCollisions();
        GetComponents();
    }

    #region Initialize
    private void IgnoreCollisions()
    {
        childRigidbodies = GetComponentsInChildren<Rigidbody>();
        Collider selectorCollider = GetComponent<Collider>();

        foreach (Rigidbody rigidbody in childRigidbodies)
        {
            Physics.IgnoreCollision(selectorCollider, rigidbody.GetComponent<Collider>(), true);
        }
    }

    private void GetComponents()
    {
        _physicsButtons = GetComponentsInChildren<PhysicsButton>();
        _volume.profile.TryGet<DepthOfField>(out _dof);
    }
    #endregion

    private void OnEnable()
    {
        //StartCoroutine(ScreenReady()); // shit doesn't work -_-
        //_dof.active = true;
        ResultInfo();
        RefreshRank(GameStateChanger.Instance.GetTitle());
        RefreshDetails();

        foreach (PhysicsButton btn in _physicsButtons)
        {
            btn.onKeyPush += Push;
        }
    }

    #region Refresh Result
    private IEnumerator ScreenReady()
    {
        yield return _fader.StartBlur();
    }

    private void ResultInfo()
    {
        _output.text = "";
        _songTitle.text = GameStateChanger.Instance.GetTitle();
        _score.text = ScoreManager.Instance.CurrentScore.ToString();
        GameObject cover = Instantiate(Resources.Load("SongCover/" + GameStateChanger.Instance.GetTitle()) as GameObject,
            _songCoverAnchor.position, Quaternion.identity);
        cover.transform.localScale *= 0.6f;
    }

    private void RefreshRank(string songTitle)
    {
        List<Tuple<string, string, float>> rank = SaveSystem.Instance.GetRankOfSong(songTitle);
        _rank.text = "";
        if (rank != null)
        {
            for (int i = 0; i < rank.Count; i++)
            {
                _rank.text += i + 1 + ". " + rank[i].Item2 + " : " + rank[i].Item3 + "\n";
            }
        }
    }

    private void RefreshDetails()
    {
        _detail.text = "Best Combo : " + ScoreManager.Instance.BestCombo + "\n" +
            "Miss : " + ScoreManager.Instance.CurrentMiss;
    }
    #endregion

    #region Push Event
    private void Push(KeyboardKeys key, Vector3 pos)
    {
        _click.PlaySoundAt(pos);

        switch (key)
        {
            case KeyboardKeys.UnderBar:
                KeyInput("_");
                break;

            case KeyboardKeys.Menu:
                Menu();
                break;

            case KeyboardKeys.Delete:
                Delete();
                break;

            case KeyboardKeys.Enter:
                Enter();
                break;

            default:
                KeyInput(key.ToString());
                break;
        }
    }
    #endregion

    #region Key Action
    private void KeyInput(string key)
    {
        if (_isEnterPressed) return;
        if (_output.text.Length > _charLimit) return;
        _output.text += key;
    }

    private void Menu()
    {
        //_dof.active = false;
        _isEnterPressed = false;
        SceneLoader.Instance.LoadNewScene("LevelSelector");
    }

    private void Delete()
    {
        if (_isEnterPressed) return;
        _output.text = _output.text.Substring(0, _output.text.Length - 1);
    }

    private void Enter()
    {
        if (_isEnterPressed) return;
        _isEnterPressed = true;
        SaveSystem.Instance.Save(_songTitle.text, _output.text, ScoreManager.Instance.CurrentScore);
        SaveSystem.Instance.Load("Ranking");
        RefreshRank(_songTitle.text);
    }
    #endregion

    private void OnDisable()
    {
        foreach (PhysicsButton btn in _physicsButtons)
        {
            btn.onKeyPush -= Push;
        }
    }
}