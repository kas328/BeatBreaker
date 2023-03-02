using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBlock : MonoBehaviour
{
    #region Serialize Field
    [Space, Header("Album")]
    [SerializeField] private Album _selectedAlbum = Album.None;
    [SerializeField] private List<GameObject> _songCovers = null;

    [Space, Header("Song Cover")]
    [SerializeField] private float _downScale = 0.3f;
    [SerializeField] private float _upScale = 1f;
    #endregion

    #region Private Field
    private int _firstSongIndex = 0;
    private int _lastSongIndex = 0;
    private string _albumTitle = null;
    private List<GameObject> _songs = null;

    private static Vector3 _downScaleVector = Vector3.zero;
    private static Vector3 _upScaleVector = Vector3.zero;
    #endregion

    #region Properties
    public int FirstSongIndex { get => _firstSongIndex; }
    public int LastSongIndex { get => _lastSongIndex; }
    public string AlbumTitle { get => _albumTitle; }
    public List<GameObject> Songs { get => _songs; }

    public static Vector3 DownScale { get => _downScaleVector; }
    public static Vector3 UpScale { get => _upScaleVector; }
    public List<GameObject> SongCovers { get => _songCovers; }
    #endregion

    private void Start()
    {
        SetupIndex();
        StartCoroutine(CreateSongCovers());
        InitVariables();
    }

    #region Initialize
    private void SetupIndex()
    {
        switch (_selectedAlbum)
        {
            case Album.None:
                break;

            case Album.VolumeOne:
                // this has to change according to the existing scenes
                _firstSongIndex = 2;
                _lastSongIndex = 5;
                _albumTitle = _selectedAlbum.ToString();
                break;

            case Album.VolumeTwo:
                _firstSongIndex = 8;
                _lastSongIndex = 13;
                _albumTitle = _selectedAlbum.ToString();
                break;
        }
    }

    private IEnumerator CreateSongCovers()
    {
        _songs = new List<GameObject>();

        for (int i = 0; i < _songCovers.Count; i++)
        {
            GameObject cover = Instantiate(_songCovers[i]);

            Color alphaZeroColor = cover.GetComponent<Renderer>().material.color;
            alphaZeroColor.a = 0f;
            Material material = cover.GetComponent<Renderer>().material;
            material.color = alphaZeroColor;
            cover.transform.localScale = _downScaleVector;

            cover.SetActive(false);
            _songs.Add(cover);
            yield return null;
        }
    }

    private void InitVariables()
    {
        _downScaleVector = _songCovers[0].transform.localScale;
        _downScaleVector *= _downScale;
        _upScaleVector = _songCovers[0].transform.localScale;
        _upScaleVector *= _upScale;
    }
    #endregion
}

public enum Album { None, VolumeOne, VolumeTwo }