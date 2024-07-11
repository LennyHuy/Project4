
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Shake(float duration , float strength)
    {
        transform.DOShakePosition(duration, strength);
        transform.DOShakeRotation(duration,strength);
    }

}
