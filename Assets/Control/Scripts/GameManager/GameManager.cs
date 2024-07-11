using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum GameState { Play, Pause, Stop };
public enum ModeState { None, Survival, Arcade };
public class GameManager : MonoBehaviour
{
    public static GameState gameState;
    public static ModeState modeState;
    private float time;
    private int live;


    private float gameValue;

    private GameObject gameplayPanel;
    private GameObject startPanel;
    private GameObject gameOverPanel;
    private GameObject pausePanel;
    private GameObject liveLogo;
    private GameObject fadeSquare;

    private BoxSpawner boxSpawner;
    private TextMeshProUGUI valueText;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;

        gameplayPanel = GameObject.Find("GameplayPanel");
        startPanel = GameObject.Find("StartPanel");
        gameOverPanel = GameObject.Find("GameOverPanel");
        pausePanel = GameObject.Find("PausePanel");
        fadeSquare = GameObject.Find("FadeScreen");

        boxSpawner = GameObject.Find("BoxSpawner").GetComponent<BoxSpawner>();
        valueText = GameObject.FindGameObjectWithTag("Timer").GetComponentInChildren<TextMeshProUGUI>();
        liveLogo = GameObject.FindGameObjectWithTag("Live");

    }
    void Start() => StopMode();
    void Update() => CheckTimeOut();

    public void ArcadeMode() => StartCoroutine(WaitTillEnterMode(ModeState.Arcade, time));
    public void SurvivalMode() => StartCoroutine(WaitTillEnterMode(ModeState.Survival, live));

    public void ConfirmMode(ModeState mode, float value) => StartCoroutine(WaitTillEnterMode(mode, value));

    private IEnumerator WaitTillEnterMode(ModeState mode, float value)
    {
        boxSpawner.StopSpawning();
        boxSpawner.ResetSpawnRate();
        yield return new WaitForSeconds(3f);
        PlayMode(mode, value);
    }

    private void PlayMode(ModeState mode, float givenValue)
    {
        gameState = GameState.Play;
        boxSpawner.StartSpawning();

        gameplayPanel.transform.DOLocalMoveY(0f, 0.3f ,false);

        startPanel.transform.DOLocalMoveY(-3000f, 3f ,false);


        switch (mode)
        {
            case ModeState.Arcade:
                modeState = ModeState.Arcade;
                gameValue = givenValue;
                liveLogo.SetActive(false);
                break;

            case ModeState.Survival:
                modeState = ModeState.Survival;
                gameValue = givenValue;
                liveLogo.SetActive(true);
                break;
        }

        UpdateValue(givenValue);
    }

    public void StopMode()
    {
        gameState = GameState.Stop;
        modeState = ModeState.None;

        boxSpawner.StartSpawning();

        pausePanel.SetActive(false);

        startPanel.SetActive(true);

        StartCoroutine(FadeBlackOutSquare(false));
    }

    private void ExitPlayMode()
    {
        gameState = GameState.Stop;
        modeState = ModeState.None;

        boxSpawner.StopSpawning();
        Invoke(nameof(DisplayGameOverPanel), 4f);
    }

    public void CheckTimeOut()
    {
        if (gameState == GameState.Stop) return;

        if (modeState == ModeState.Arcade)
        {
            gameValue -= Time.deltaTime;
            UpdateValue(gameValue);
        }

    }

    //Adjust the text with a given value 
    public void UpdateValue(float value)
    {
        if (value > 0f)
        {
            valueText.text = ((int)value).ToString();
        }
        else
        {
            valueText.text = "0";
            ExitPlayMode();
        }
    }

    public float GetGameValue() { return gameValue; }
    public void SetGameValue(float gameValue)
    {
        this.gameValue = gameValue;
        UpdateValue(gameValue);
    }

    public float SetTime(float value) => this.time = value;
    public int SetLive(int value) => this.live = value;

    public void Pause()
    {
        if (gameState == GameState.Stop) return;

        SoundManager.Instance.PlayDefault();

        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        gameState = GameState.Pause;
    }
    public void Resume()
    {
        SoundManager.Instance.PlayDefault();

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        gameState = GameState.Play;
    }


    public void IncreaseValue(float value) => gameValue += value;
    private void DisplayGameOverPanel()
    {
        gameplayPanel.transform.DOLocalMoveY(3000f, 2f ,false);
        
        gameOverPanel.transform.DOLocalMoveY(0f, 0.7f, false);
    }
    public void ExitToMenu()
    {
        SoundManager.Instance.PlayDefault();

        Time.timeScale = 1f;
        boxSpawner.StopSpawning();

        StartCoroutine(FadeBlackOutSquare(true));
        Invoke(nameof(ReLoadNewScene), 3f);
    }
    private void ReLoadNewScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true)
    {
        Color objectColor = fadeSquare.GetComponent<Image>().color;
        float fadeAmount;
        float fadeSpeed = 2f;

        if(fadeToBlack)
        {
            while( fadeSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        } else 
        {
            while( fadeSquare.GetComponent<Image>().color.a >  0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }


}

