using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    #region Properties
    public float CurrentScore { get; private set; } = 0f;
    public int CurrentCombo { get; private set; } = 0;
    public int CurrentMiss { get; private set; } = 0;
    public int BestCombo { get; private set; } = 0;
    public int ScoreMultiplier { get; private set; } = 1;
    #endregion

    #region Public Methods
    public void AddScore(float score)
    {
        CurrentCombo += 1;
        if (CurrentCombo >= BestCombo)
        {
            BestCombo = CurrentCombo;
        }

        // I'm pretty sure there's smarter way of doing this...
        ScoreMultiplier = CurrentCombo >= 20 ? CurrentCombo >= 30 ? CurrentCombo >= 40 ? CurrentCombo >= 50 ?
            CurrentCombo >= 60 ? CurrentCombo >= 70 ? CurrentCombo >= 80 ? 8 : 7 : 6 : 5 : 4 : 3 : 2 : 1;
        CurrentScore += score * ScoreMultiplier;
    }

    public void Missed()
    {
        CurrentCombo = 0;
        ++CurrentMiss;
    }
    #endregion
}
