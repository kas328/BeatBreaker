using TMPro;
using UnityEngine;

public class ScoreViewer : Singleton<ScoreViewer>
{
    #region Serialize Field
    [Header("Outputs")]
    [SerializeField] private TextMeshProUGUI output = null;

    [Header("Score Cut")]
    [SerializeField] private float perfect = 90f;
    [SerializeField] private float good = 70f;
    [SerializeField] private float bad = 60f;
    #endregion

    #region Output
    public void UpdateScore(float score)
    {
        output.text = "Score" + "\n" + Mathf.Round(score) + "\n";
        if (score >= perfect) output.text += "Perfect";
        else if (score >= good) output.text += "Good";
        else if (score >= bad) output.text += "Bad";
    }
    #endregion
}