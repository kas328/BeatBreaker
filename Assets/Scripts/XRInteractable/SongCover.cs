using OVR;
using UnityEngine;
using UnityEngine.UI;

public class SongCover : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] private SoundFXRef _song = null;
    [SerializeField] private Sprite _coverImage = null;
    #endregion

    #region Private Field
    private Image _image = null;
    #endregion

    #region Properties
    public SoundFXRef Song { get => _song; }
    #endregion

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _image.sprite = _coverImage;
    }
}