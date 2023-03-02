using OVR;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelSelector : MonoBehaviour
{
    #region Serialize Field
    [Space, Header("View Points")]
    [SerializeField] private Transform _selectionPoint = null;
    [SerializeField] private Vector3 _leftPointOffset = Vector3.zero;
    [SerializeField] private Vector3 _rightPointOffset = Vector3.zero;
    [SerializeField] private TextMeshProUGUI _songDetail = null;

    [Space, Header("Smoothing Value")]
    [SerializeField] private AnimationCurve _animationCurve = null;
    [SerializeField] private float _smoothMoveDurationTime = 0.6f;
    [SerializeField] private float _smoothScaleDurationTime = 0.6f;
    [SerializeField] private float _scaleDownValue = 0.65f;

    [Space, Header("Actuater")]
    [SerializeField] private SandBagHitPoint[] _sandBagHitPoints = null;

    [Space, Header("Audio")]
    [SerializeField] private SoundFXRef _nextButton = null;
    [SerializeField] private SoundFXRef _playButton = null;
    [SerializeField] private SoundFXRef _musicBlockAttach = null;
    #endregion

    #region Private Field
    private MusicBlock _selectedMusicBlock = null;
    private SoundFXRef _playingSound = null;

    private bool _isMusicBoxAttached = false;
    private int _firstSongIndex = 0;
    private int _lastSongIndex = 0;
    private int _selectedSceneIndex = 2;
    private string _selectedSceneTitle = null;

    private Vector3 _centerPosition = Vector3.zero;
    private Vector3 _leftPosition = Vector3.zero;
    private Vector3 _rightPosition = Vector3.zero;

    private int _leftPoint = -1;
    private int _centerPoint = -1;
    private int _rightPoint = -1;

    private Vector3 _lerpPositionValue = Vector3.zero;
    private Vector3 _lerpScaleValue = Vector3.zero;
    #endregion

    private void Awake()
    {
        InitVariables();
    }

    #region Initialize
    private void InitVariables()
    {
        _centerPosition = _selectionPoint.position;
        _leftPosition = _centerPosition + _leftPointOffset;
        _rightPosition = _centerPosition + _rightPointOffset;
    }
    #endregion

    private void OnEnable()
    {
        foreach (SandBagHitPoint bag in _sandBagHitPoints)
        {
            bag.onHitSandBag += Hit;
        }
    }

    #region Push Event
    private void Hit(HitFunctions function)
    {
        if (!_isMusicBoxAttached) return;

        switch (function)
        {
            case HitFunctions.Play:
                PlaySelectedLevel();
                break;

            case HitFunctions.Previous:
                PreviousLevel();
                break;

            case HitFunctions.Next:
                NextLevel();
                break;
        }
    }
    #endregion

    #region Button Functions
    private void PlaySelectedLevel()
    {
        _playingSound.StopSound();
        _playButton.PlaySoundAt(_selectionPoint.position, 0f, 2f);

        GameStateChanger.Instance.SetGameSong(_playingSound);
        _selectedSceneTitle = SceneLoader.Instance.Scenes[_selectedSceneIndex];
        SceneLoader.Instance.LoadNewScene(_selectedSceneTitle);
    }

    private void PreviousLevel()
    {
        _nextButton.PlaySoundAt(_selectionPoint.position, 0f, 2f);

        _selectedSceneIndex -= 1;
        if (_selectedSceneIndex != _firstSongIndex - 1)
        {
            ShowPrevious(_selectedMusicBlock);
        }
        _selectedSceneIndex = Mathf.Clamp(_selectedSceneIndex, _firstSongIndex, _lastSongIndex);

    }

    private void NextLevel()
    {
        _nextButton.PlaySoundAt(_selectionPoint.position, 0f, 2f);

        _selectedSceneIndex += 1;
        if (_selectedSceneIndex != _lastSongIndex + 1)
        {
            ShowNext(_selectedMusicBlock);
        }
        _selectedSceneIndex = Mathf.Clamp(_selectedSceneIndex, _firstSongIndex, _lastSongIndex);

    }
    #endregion

    #region Controls
    private void SetGameSong()
    {
        _playingSound.StopSound();
        GameObject song = _selectedMusicBlock.Songs[_centerPoint];
        _playingSound = song.GetComponent<SongCover>().Song;
        GameStateChanger.Instance.SetGameSong(_playingSound);
        _playingSound.PlaySoundAt(transform.position + transform.forward * -0.6f, 0.8f);
    }
    #endregion

    #region Visualizer
    /// <summary>
    /// Show next behaviour logic.
    /// </summary>
    private void ShowNext(MusicBlock musicBlock)
    {
        if (_leftPoint != -1)
        {
            GameObject songCover = musicBlock.Songs[_leftPoint];
            Vector3 startScaleLeft = songCover.transform.localScale;
            Color startColor = songCover.GetComponent<Renderer>().material.color;

            StartCoroutine(SmoothFade(songCover, startScaleLeft, MusicBlock.DownScale, startColor, startColor.a, 0f,
                _animationCurve, _smoothScaleDurationTime, FadeMode.Out));
        }

        _leftPoint = _centerPoint;
        GameObject songCoverCenter = musicBlock.Songs[_centerPoint];
        Vector3 startPosCenter = songCoverCenter.transform.position;
        StartCoroutine(SmoothMove(songCoverCenter, startPosCenter, _leftPosition, _animationCurve, _smoothMoveDurationTime));

        Vector3 startScale = songCoverCenter.transform.localScale;
        Color color = songCoverCenter.GetComponent<Renderer>().material.color;
        StartCoroutine(SmoothFade(songCoverCenter, startScale, MusicBlock.UpScale * _scaleDownValue, color, color.a, _scaleDownValue,
                _animationCurve, _smoothScaleDurationTime, FadeMode.In));

        _centerPoint = _rightPoint;
        GameObject songCoverRight = musicBlock.Songs[_rightPoint];
        Vector3 startPosRight = songCoverRight.transform.position;
        StartCoroutine(SmoothMove(songCoverRight, startPosRight, _centerPosition, _animationCurve, _smoothMoveDurationTime));

        Vector3 startScaleR = songCoverRight.transform.localScale;
        Color colorR = songCoverRight.GetComponent<Renderer>().material.color;
        StartCoroutine(SmoothFade(songCoverRight, startScaleR, MusicBlock.UpScale, colorR, colorR.a, 1f,
                _animationCurve, _smoothScaleDurationTime, FadeMode.In));

        ++_rightPoint;
        if (_rightPoint == 4) _rightPoint = -1;
        else
        {
            GameObject songCover = musicBlock.Songs[_rightPoint];
            songCover.transform.position = _rightPosition;
            Vector3 startScaleRight = songCover.transform.localScale;
            Color startColor = songCover.GetComponent<Renderer>().material.color;
            
            StartCoroutine(SmoothFade(songCover, startScaleRight, MusicBlock.UpScale * _scaleDownValue, startColor, startColor.a, _scaleDownValue,
                _animationCurve, _smoothScaleDurationTime, FadeMode.In));
        }
        SetGameSong();
        _songDetail.text = _playingSound.name;
    }

    /// <summary>
    /// Show previous behaviour logic.
    /// </summary>
    private void ShowPrevious(MusicBlock musicBlock)
    {
        if (_rightPoint != -1)
        {
            GameObject songCover = musicBlock.Songs[_rightPoint];
            Vector3 startScaleLeft = songCover.transform.localScale;
            Color startColor = songCover.GetComponent<Renderer>().material.color;

            StartCoroutine(SmoothFade(songCover, startScaleLeft, MusicBlock.DownScale, startColor, startColor.a, 0f,
                _animationCurve, _smoothScaleDurationTime, FadeMode.Out));

        }
        _rightPoint = _centerPoint;
        GameObject songCoverCenter = musicBlock.Songs[_centerPoint];
        Vector3 startPosCenter = songCoverCenter.transform.position;
        StartCoroutine(SmoothMove(songCoverCenter, startPosCenter, _rightPosition, _animationCurve, _smoothMoveDurationTime));

        Vector3 startScale = songCoverCenter.transform.localScale;
        Color color = songCoverCenter.GetComponent<Renderer>().material.color;
        StartCoroutine(SmoothFade(songCoverCenter, startScale, MusicBlock.UpScale * _scaleDownValue, color, color.a, _scaleDownValue,
                _animationCurve, _smoothScaleDurationTime, FadeMode.In));

        _centerPoint = _leftPoint;
        GameObject songCoverLeft = musicBlock.Songs[_leftPoint];
        Vector3 startPosLeft = songCoverLeft.transform.position;
        StartCoroutine(SmoothMove(songCoverLeft, startPosLeft, _centerPosition, _animationCurve, _smoothMoveDurationTime));

        Vector3 startScaleR = songCoverLeft.transform.localScale;
        Color colorR = songCoverLeft.GetComponent<Renderer>().material.color;
        StartCoroutine(SmoothFade(songCoverLeft, startScaleR, MusicBlock.UpScale, colorR, colorR.a, 1f,
                _animationCurve, _smoothScaleDurationTime, FadeMode.In));

        --_leftPoint;
        if (_leftPoint != -1)
        {
            GameObject songCover = musicBlock.Songs[_leftPoint];
            songCover.transform.position = _leftPosition;
            Vector3 startScaleLeft = songCover.transform.localScale;
            Color startColor = songCover.GetComponent<Renderer>().material.color;

            StartCoroutine(SmoothFade(songCover, startScaleLeft, MusicBlock.UpScale * _scaleDownValue, startColor, startColor.a, _scaleDownValue,
                _animationCurve, _smoothScaleDurationTime, FadeMode.In));
        }
        SetGameSong();
        _songDetail.text = _playingSound.name;
    }

    /// <summary>
    /// Smoothly moves the song cover when the switch button is pushed.
    /// </summary>
    private IEnumerator SmoothMove(GameObject songCover, Vector3 startPos, Vector3 targetPos, AnimationCurve ac, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            _lerpPositionValue = Vector3.Lerp(startPos, targetPos, ac.Evaluate(elapsedTime / duration));
            songCover.transform.position = _lerpPositionValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _lerpPositionValue = targetPos;
    }

    /// <summary>
    /// Downscale or upscale, fades out or in, the color when the song cover is out of the viewer point.
    /// </summary>
    private IEnumerator SmoothFade(GameObject songCover, Vector3 startScale, Vector3 targetScale, Color startColor, float startAlpha, float targetAlpha,
        AnimationCurve ac, float duration, FadeMode fadeMode)
    {
        if (fadeMode.Equals(FadeMode.In)) songCover.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            Material material = songCover.GetComponent<Renderer>().material;
            startColor.a = Mathf.Lerp(startAlpha, targetAlpha, ac.Evaluate(elapsedTime / duration));
            material.color = startColor;

            Color imageColor = songCover.GetComponentInChildren<Image>().color;
            imageColor.a = Mathf.Lerp(startAlpha, targetAlpha, ac.Evaluate(elapsedTime / duration));
            songCover.GetComponentInChildren<Image>().color = imageColor;

            _lerpScaleValue = Vector3.Lerp(startScale, targetScale, ac.Evaluate(elapsedTime / duration));
            songCover.transform.localScale = _lerpScaleValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (fadeMode.Equals(FadeMode.Out)) songCover.SetActive(false);
    }
    #endregion

    #region Socket Event
    /// <summary>
    /// This is called when music block is attatched to the socket.
    /// </summary>
    public void OnMusicBlockAttach(GameObject socket)
    {
        _isMusicBoxAttached = true;

        GameObject interactor = socket.GetComponent<XRSocketInteractor>().selectTarget.gameObject;
        _selectedMusicBlock = interactor.GetComponent<MusicBlock>();
        _musicBlockAttach.PlaySoundAt(interactor.transform.position, 0.3f, 2f);

        _firstSongIndex = _selectedMusicBlock.FirstSongIndex;
        _lastSongIndex = _selectedMusicBlock.LastSongIndex;
        _selectedSceneIndex = _firstSongIndex;
        _selectedSceneTitle = SceneLoader.Instance.Scenes[_selectedSceneIndex];

        AttachVisualize();
    }

    private void AttachVisualize()
    {
        _leftPoint = -1; _centerPoint = 0; _rightPoint = 1;

        GameObject firstSong = _selectedMusicBlock.Songs[_centerPoint];
        _playingSound = firstSong.GetComponent<SongCover>().Song;
        GameStateChanger.Instance.SetGameSong(_playingSound);
        _playingSound.PlaySound(1.6f);
        // this can be set in a different place.

        firstSong.transform.position = _centerPosition;
        Color color = firstSong.GetComponent<Renderer>().material.color;
        StartCoroutine(SmoothFade(firstSong, firstSong.transform.localScale, MusicBlock.UpScale, color, color.a, 1f,
            _animationCurve, _smoothScaleDurationTime, FadeMode.In));

        GameObject secondSong = _selectedMusicBlock.Songs[_rightPoint];
        secondSong.transform.position = _rightPosition;
        color = secondSong.GetComponent<Renderer>().material.color;
        StartCoroutine(SmoothFade(secondSong, secondSong.transform.localScale, MusicBlock.UpScale * _scaleDownValue, color, color.a, _scaleDownValue,
            _animationCurve, _smoothScaleDurationTime, FadeMode.In));

        _songDetail.text = _playingSound.name;
    }

    /// <summary>
    /// This is called when music block is detached from the socket.
    /// </summary>
    public void OnMusicBlockDetach()
    {
        _isMusicBoxAttached = false;
        _songDetail.text = null;
        _playingSound.StopSound();

        GameObject center = _selectedMusicBlock.Songs[_centerPoint];
        Color color = center.GetComponent<Renderer>().material.color;
        StartCoroutine(SmoothFade(center, center.transform.localScale, MusicBlock.DownScale, color, color.a, 0f,
            _animationCurve, _smoothScaleDurationTime, FadeMode.Out));

        if (_rightPoint != -1)
        {
            GameObject right = _selectedMusicBlock.Songs[_rightPoint];
            color = right.GetComponent<Renderer>().material.color;
            StartCoroutine(SmoothFade(right, right.transform.localScale, MusicBlock.DownScale, color, color.a, 0f,
                _animationCurve, _smoothScaleDurationTime, FadeMode.Out));
        }
        
        if (_leftPoint != -1)
        {
            GameObject left = _selectedMusicBlock.Songs[_leftPoint];
            color = left.GetComponent<Renderer>().material.color;
            StartCoroutine(SmoothFade(left, left.transform.localScale, MusicBlock.DownScale, color, color.a, 0f,
                _animationCurve, _smoothScaleDurationTime, FadeMode.Out));
        }
    }
    #endregion

    private void OnDisable()
    {
        foreach (SandBagHitPoint bag in _sandBagHitPoints)
        {
            bag.onHitSandBag -= Hit;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_selectionPoint.position, Vector3.one * 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_selectionPoint.position + _leftPointOffset, Vector3.one * 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_selectionPoint.position + _rightPointOffset, Vector3.one * 0.1f);
    }

    public enum FadeMode { In, Out }
}