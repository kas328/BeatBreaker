using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    #region Serialize Field
    [SerializeField] private UnityEvent OnLoadBegin = new UnityEvent();
    [SerializeField] private UnityEvent OnLoadEnd = new UnityEvent();
    [SerializeField] private ScreenFader _screenFader = null;
    #endregion

    #region Private Field
    private bool _isLoading = false;
    private int _sceneCount = 0;
    private string[] _scenes = null;
    #endregion

    #region Properties
    public int SceneCount { get => _sceneCount; }
    public string[] Scenes { get => _scenes; }
    #endregion

    private void Awake()
    {
        LoadFirstScene();
        GetAllSceneNames();
    }

    #region Initialize
    private void LoadFirstScene()
    {
        if (!Application.isEditor)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
        }
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
    }

    private void GetAllSceneNames()
    {
        _sceneCount = SceneManager.sceneCountInBuildSettings;
        _scenes = new string[_sceneCount];

        for (int i = 0; i < _sceneCount; i++)
        {
            _scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
    }
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetActiveScene;
    }

    #region Delegates
    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }
    #endregion

    #region Load Process
    public void LoadNewScene(string sceneName)
    {
        if (!_isLoading)
        {
            StartCoroutine(LoadScene(sceneName));
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
        _isLoading = true;

        OnLoadBegin?.Invoke();
        yield return _screenFader.StartFadeIn();
        yield return StartCoroutine(UnloadCurrent());

        yield return new WaitForSeconds(1f);
        print("hi");

        yield return StartCoroutine(LoadNew(sceneName));
        yield return _screenFader.StartFadeOut();
        OnLoadEnd?.Invoke();

        _isLoading = false;
    }

    private IEnumerator UnloadCurrent()
    {
        AsyncOperation unloadAsyncOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        while (!unloadAsyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(unloadAsyncOperation.progress / .9f);
            yield return null;
        }
    }

    private IEnumerator LoadNew(string sceneName)
    {
        AsyncOperation loadAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!loadAsyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadAsyncOperation.progress / .9f);
            yield return null;
        }
    }
    #endregion

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActiveScene;
    }
}