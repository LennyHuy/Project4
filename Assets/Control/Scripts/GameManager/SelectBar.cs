using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


[System.Serializable]
public class Modes
{
    public ModeState name;
    public float[] value;
}

public class SelectBar : MonoBehaviour
{

    public Modes[] mode;
    [SerializeField] private TextMeshProUGUI modeUI;
    [SerializeField] private TextMeshProUGUI valueUI;

    private int valueIndex = 0;
    private int modeIndex = 0;
    private bool canUpdateScore;
    private bool canChangeStatus;
    private Button playButton;
    private GameObject selectBar;

    private GameManager gameManager;
    private ScoreManager scoreManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        playButton = GameObject.FindGameObjectWithTag("Play").GetComponent<Button>();
        selectBar = GameObject.FindGameObjectWithTag("SelectBar");
        Input.multiTouchEnabled = false;
    }
    void Start()
    {
        playButton.interactable = true;   
        canUpdateScore = true;  
        canChangeStatus = true;
        UpdateMode();      
    }
    public void ChangeNextMode()
    {
        if(canChangeStatus == false) return;

        modeIndex++;
        if (modeIndex == mode.Length) modeIndex = 0;
        UpdateMode();
    }

    public void ChangePreviousMode()
    {
        if(canChangeStatus == false) return;
        
        if (modeIndex == 0) modeIndex = mode.Length;
        modeIndex--;
        UpdateMode();
    }

    public void ChangeNextValue()
    {
        if(canChangeStatus == false) return;

        valueIndex++;
        if (valueIndex == mode[modeIndex].value.Length) valueIndex = 0;
        valueUI.text = mode[modeIndex].value[valueIndex].ToString();

        SoundManager.Instance.PlayDefault();

    }

    public void ChangePreviousValue()
    {
        if(canChangeStatus == false) return;

        if (valueIndex == 0) valueIndex = mode[modeIndex].value.Length;
        valueIndex--;
        valueUI.text = mode[modeIndex].value[valueIndex].ToString();

        SoundManager.Instance.PlayDefault();
    }

    public void UpdateMode()
    {
        valueIndex =0;
        modeUI.text = mode[modeIndex].name.ToString();
        valueUI.text = mode[modeIndex].value[valueIndex].ToString();
        scoreManager.UpdateHighScoreMode(mode[modeIndex].name, canUpdateScore);

        SoundManager.Instance.PlayDefault();
    }

    public void TapToPlay()
    {
        gameManager.ConfirmMode(mode[modeIndex].name, mode[modeIndex].value[valueIndex]);
        SoundManager.Instance.PlayDefault();

        playButton.interactable = false;
        canUpdateScore = false;
        canChangeStatus = false;
        Input.multiTouchEnabled = true;

        selectBar.transform.DOLocalMoveY (-1000f, 0.5f , false);
        playButton.transform.DOLocalMoveY (-1000f, 0.75f , false);
        //Debug.Log("Mode : " + mode[modeIndex].name + " Value : " + mode[modeIndex].value[valueIndex]);
    }
}
