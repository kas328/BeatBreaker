using TMPro;
using UnityEngine;

public class ScoreVisualizer : MonoBehaviour
{
    #region Serialize Field
    [Space, Header("UI")]
    [SerializeField] private TextMeshProUGUI _score = null;
    [SerializeField] private TextMeshProUGUI _combo = null;
    [SerializeField] private TextMeshProUGUI _scoreMultiplier = null;
    [SerializeField] private TextMeshProUGUI _remainingTime = null;
    #endregion

    private void Awake()
    {
        //this is for testing.
        GameStateChanger.Instance.GameOn();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _score.text = Mathf.Round(ScoreManager.Instance.CurrentScore).ToString();
        _combo.text = ScoreManager.Instance.CurrentCombo.ToString();
        _scoreMultiplier.text = ScoreManager.Instance.ScoreMultiplier.ToString();
        _remainingTime.text = GameStateChanger.Instance.GetTime();
    }
}
