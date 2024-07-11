using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static int highScoreArcade = 0;
    private static int highScoreSurvival = 0;
    [HideInInspector] public int currentScore = 0;
    [SerializeField] private TextMeshProUGUI highScoreText_Start;
    [SerializeField] private TextMeshProUGUI highScoreText_Play;
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        currentScore = 0;
        highScoreArcade = PlayerPrefs.GetInt("highScoreArcade", highScoreArcade);
        highScoreSurvival = PlayerPrefs.GetInt("highScoreSurvival", highScoreSurvival);

        SetHighScoreText(highScoreText_Start, highScoreArcade);
        SetHighScoreText(highScoreText_Play, highScoreArcade);
        UpdateScore(0);

    }

    public void ConfirmScore(ModeState mode) 
    {

    }

    public void UpdateScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString();
        CheckHighScore();
    }

    public void UpdateHighScoreMode(ModeState mode, bool canUpdateScore)
    {
        if(canUpdateScore == false) return;

        switch (mode)
        {
            case ModeState.Arcade:
                SetHighScoreText(highScoreText_Start, highScoreArcade);
                SetHighScoreText(highScoreText_Play, highScoreArcade);
                break;
            case ModeState.Survival:
                SetHighScoreText(highScoreText_Start, highScoreSurvival);
                SetHighScoreText(highScoreText_Play, highScoreSurvival);
                break;
        }
    }

    public void CheckHighScore()
    {
        if (GameManager.modeState == ModeState.Arcade)
        {
            if (currentScore > PlayerPrefs.GetInt("highScoreArcade", highScoreArcade))
            {
                highScoreArcade = currentScore;
                UpdateAndSave("highScoreArcade", highScoreArcade);
            }

        }
        else if (GameManager.modeState == ModeState.Survival)
        {
            if (currentScore > PlayerPrefs.GetInt("highScoreSurvival", highScoreSurvival))
            {
                highScoreSurvival = currentScore;
                UpdateAndSave("highScoreSurvival",highScoreSurvival);
            }
        }
    }

    public void UpdateAndSave(string scoretype, int scorevalue)
    {
        PlayerPrefs.SetInt(scoretype, scorevalue);
        PlayerPrefs.Save();

        SetHighScoreText(highScoreText_Play, scorevalue);

    }

    public void SetHighScoreText(TextMeshProUGUI highScoreTextObject, float highScoreValue)
    {
        highScoreTextObject.text = $"Best: {Mathf.Round(highScoreValue)}";
    }
}
