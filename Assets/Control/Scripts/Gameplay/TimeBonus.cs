using System.Collections;
using UnityEngine;
using TMPro;

public class TimeBonus : MonoBehaviour
{
    private GameManager gameManager;
    private TextMeshProUGUI valueText;
    private CanvasGroup canvasGroup;
    private Coroutine coroutine;
    private int bonusValue;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        valueText = gameObject.GetComponent<TextMeshProUGUI>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void SetValue(int bonusValue){
        this.bonusValue = bonusValue;
        DisplayText();
        NumberDisplay();
    }

    public void NumberDisplay()
    {
        if (bonusValue > 0)
        {
            valueText.text = "+" + bonusValue;
            valueText.color = Color.green;
        }

        else if (bonusValue < 0)
        {
            valueText.text = bonusValue.ToString();
            valueText.color = Color.red;
        }

        else
        {
            valueText.text = "+" + bonusValue;
            valueText.color = Color.white;
        }
    }

    public void DisplayText()
    {
        canvasGroup.alpha = 1f;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator WaitTillRemove()
    {
        yield return new WaitForSeconds(1f);
        canvasGroup.alpha = 0f;
    }

    IEnumerator FadeOut()
    {
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha; // Ensure the alpha is exactly the end value

        // If you want to do something after fading out, you can put it here

        yield return null;
    }

}
