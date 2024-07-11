using UnityEngine;
using UnityEngine.Pool;

public class Box : MonoBehaviour
{
    public float force;
    public float minOffsetForce;
    public float maxOffsetForce;
    public float torque;
    public float timeValue;
    public ParticleSystem particle;
    public AudioClip explode;


    private int randomBonusTime;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private GameManager gameManager;
    private ScoreManager scoreManager;
    private BoxSpawner boxSpawner;
    private TimeBonus timeBonus;
    private BoxParticle boxParticle;
    //private ObjectPool<Box> pool;
    void Awake()
    {
        //Find the parent gameobject then find the child gameobj "Slide" 's TimeSlider component directly without the need to find the child first

        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timeBonus = GameObject.Find("Canvas (Background)").GetComponentInChildren<TimeBonus>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        boxSpawner = GameObject.Find("BoxSpawner").GetComponent<BoxSpawner>();
        boxParticle = particle.GetComponent<BoxParticle>();
    }

    //OnEnable is called every time the object is active again after deactivation (after Awake , before Start)
    private void OnEnable() { }

    void Start()
    {
        rb.AddForce(Vector2.up * RandomForce(), ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-torque, torque), ForceMode2D.Impulse);
    }

    float RandomForce()
    {
        return force + Random.Range(minOffsetForce, maxOffsetForce);
    }

    void OnMouseDown()
    {
        //Prevent further doing after game ends
        if (gameManager.GetGameValue() <= 0f) return;

        //Prevent unecessary display after game ends and inputs during pausing
        if (GameManager.gameState != GameState.Play) return;
        
        //Change particle color based on type of box
        boxParticle.CheckBox(gameObject.name);

        //ObjectPoolManager.SpawnObject(particle.gameObject,gameObject.transform.position,Quaternion.identity);
        Instantiate(particle,gameObject.transform.position,Quaternion.identity);

        //Play SFX
        SoundManager.Instance.PlaySound(explode);

        //Then destroy it
        Destroy(gameObject);

        //Increment the score if conditions are met and stop all after that
        if (gameObject.name == "Box(Clone)")
        {
            if (GameManager.gameState == GameState.Play)
            {
                scoreManager.UpdateScore(1);              
                return;
            }

        }

        switch (gameObject.name)
        {
            case "BadBox(Clone)":
                ChangingValue();
                ScreenShake.Instance.Shake(0.5f, 0.75f);
                break;
            case "BonusBox(Clone)":
                ChangingValue();
                break;
            case "QuestionBox(Clone)":
                ChangingRandomValue();
                ScreenShake.Instance.Shake(0.5f,0.3f);
                break;
        }
    }

    void ChangingValue()
    {
        if (GameManager.modeState == ModeState.Arcade)
        {
            AdjustValueBy(timeValue);
        }

        if (GameManager.modeState == ModeState.Survival)
        {
            if (gameObject.name == "BadBox(Clone)")
            {
                AdjustValueBy(-1);
            }
            if (gameObject.name == "BonusBox(Clone)")
            {
                AdjustValueBy(1);
            }
        }
    }

    void ChangingRandomValue()
    {
        switch (GameManager.modeState)
        {
            case ModeState.Arcade:
                randomBonusTime = Mathf.RoundToInt(Random.Range(-timeValue, timeValue));
                break;
            case ModeState.Survival:
                randomBonusTime = Mathf.RoundToInt(Random.Range(-1, 2));
                break;
        }
        AdjustValueBy(randomBonusTime);
    }

    void AdjustValueBy(float value)
    {
        gameManager.SetGameValue(gameManager.GetGameValue() + value);
        timeBonus.SetValue((int)value);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
        //boxSpawner.pool.Release(this);


        if (gameObject.name == "Box(Clone)" && GameManager.modeState == ModeState.Survival)
        {
            AdjustValueBy(-1);
            ScreenShake.Instance.Shake(1f, 1f);
            SoundManager.Instance.PlaySound(explode);
        }
    }

    // public void SetPool(ObjectPool<Box> pool){
    //     this.pool = pool;
    // }



}
