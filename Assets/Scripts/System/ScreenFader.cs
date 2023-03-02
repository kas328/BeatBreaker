using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenFader : MonoBehaviour
{
    #region Serialize Field
    [Space, Header("Fade")]
    [SerializeField] private float _fadeSpeed = 0.3f;
    [SerializeField] private float _intensity = 0.3f;

    [Space, Header("Blur")]
    [SerializeField] private float _blurSpeed = 0.3f;
    [SerializeField] private float _startDistance = 4f;
    #endregion

    #region Private Field
    private Volume _volume = null;
    private ShadowsMidtonesHighlights _smh = null;
    private float originShadowValue = 0f;
    private DepthOfField _dof = null;

    #endregion

    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    _fadeMaterial.SetFloat("_Intensity", _intensity);
    //    _fadeMaterial.SetColor("_FadeColor", _color);
    //    Graphics.Blit(source, destination, _fadeMaterial);
    //}

    private void Awake()
    {
        _volume = GetComponentInChildren<Volume>();
        _volume.profile.TryGet<ShadowsMidtonesHighlights>(out _smh);
        originShadowValue = _smh.shadows.value.magnitude;
    }

    #region Fader
    public Coroutine StartFadeIn()
    {
        StopAllCoroutines();
        return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (_smh.shadows.value.magnitude >= 1)
        {
            _smh.shadows.value -= Vector4.one * _fadeSpeed * Time.deltaTime;
            _smh.midtones.value -= Vector4.one * _fadeSpeed * Time.deltaTime;
            _smh.highlights.value -= Vector4.one * _fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public Coroutine StartFadeOut()
    {
        StopAllCoroutines();
        return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (_smh.shadows.value.magnitude <= originShadowValue)
        {
            _smh.shadows.value += Vector4.one * _fadeSpeed * Time.deltaTime;
            _smh.midtones.value += Vector4.one * _fadeSpeed * Time.deltaTime;
            _smh.highlights.value += Vector4.one * _fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region Blur
    public Coroutine StartBlur()
    {
        StopAllCoroutines();
        return StartCoroutine(BlurIn());
    }

    private IEnumerator BlurIn()
    {
        while (_dof.gaussianStart.value > _startDistance)
        {
            _dof.gaussianStart.value -= _blurSpeed * Time.deltaTime;
            yield return null;
        }
    }
    #endregion
}
