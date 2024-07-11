using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class Boxes
{
    public string name;
    public GameObject prefabs;
    [Range(0f, 100f)] public float chance;
    [HideInInspector] public double weight;
}
public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private Boxes[] boxes;
    //public ObjectPool<Box> pool;

    [Header("SpawnRate")]
    public float spawnRate = 1f;
    public float maxSpawnRateCap = 0.5f;
    public float increaseRate = 0.005f;

    [Header("SpawnPos")]
    public float spawnPosXOffset;
    public float spawnPosY;


    private float screenWidthWorld;
    private float screenHeightWorld;
    private float originalSpawnRate;

    private double accumulatedWeight;
    private System.Random rand = new System.Random();

    public void ResetSpawnRate() => spawnRate = originalSpawnRate;

    private void Awake()
    {
        CalculateWeight();
    }
    private void Start()
    {
        screenWidthWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        screenHeightWorld = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        originalSpawnRate = spawnRate;
        //pool = new ObjectPool<Box>(createBox,OnTakeBoxFromPool, OnReturnBulletToPool, OnDestroyBox, true, 100, 200 );
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnTarget());
        StartCoroutine(SpawnWaveTarget());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    public IEnumerator SpawnTarget()
    {
        while (GameManager.gameState == GameState.Play || GameManager.gameState == GameState.Stop)
        {
            yield return new WaitForSeconds(spawnRate);
            //pool.Get();
            Boxes randomBox = boxes[GetRandomBoxIndex()];
            GameObject obj = Instantiate(randomBox.prefabs, spawnLocation(), randomBox.prefabs.transform.rotation);
            if (spawnRate >= maxSpawnRateCap)
            {
                spawnRate -= increaseRate;
            }
        }
    }

    public IEnumerator SpawnWaveTarget()
    {
        while (GameManager.gameState == GameState.Play)
        {
            yield return new WaitForSeconds(Random.Range(4f, 8f));
            int randomSize = Random.Range(3, 5);
            for (int i = 0; i < randomSize; i++)
            {
                Boxes randomBox = boxes[GetRandomBoxIndex()];
                GameObject obj = Instantiate(randomBox.prefabs, spawnLocation(), randomBox.prefabs.transform.rotation);
            }

        }
    }

    Vector2 spawnLocation()
    {
        return new Vector2(Random.Range(-screenWidthWorld + spawnPosXOffset, screenWidthWorld - spawnPosXOffset), spawnPosY);
    }

    Vector2 spawnLocationRandom(){
        int rand = Random.Range(0,10);
        switch(rand){
            case 0:
            return new Vector2(Random.Range(-screenWidthWorld + spawnPosXOffset, screenWidthWorld - spawnPosXOffset), screenHeightWorld + 10f);
            default:
            return new Vector2(Random.Range(-screenWidthWorld + spawnPosXOffset, screenWidthWorld - spawnPosXOffset), spawnPosY);
        }
    }

    private int GetRandomBoxIndex()
    {
        double r = rand.NextDouble() * accumulatedWeight;

        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i].weight >= r) return i;
        }

        return 0;
    }

    private void CalculateWeight()
    {
        accumulatedWeight = 0f;
        foreach (Boxes box in boxes)
        {
            accumulatedWeight += box.chance;
            box.weight = accumulatedWeight;
        }

    }

    // private Box createBox()
    // {
    //     Boxes randomBox = boxes[GetRandomBoxIndex()];
    //     Box box;
    //     for (int i = 0; i < boxes.Length; i++)
    //     {
    //         box = Instantiate(randomBox.prefabs, spawnLocation(), randomBox.prefabs.transform.rotation);
    //         box.SetPool(pool);
    //     }

    // }

    // private void OnTakeBoxFromPool(Box box){
    //     box.transform.position = spawnLocation();

    //     box.gameObject.SetActive(true);
    // }

    // private void OnReturnBulletToPool(Box box){
    //     box.gameObject.SetActive(false);
    // }

    // private void OnDestroyBox(Box box){
    //     Destroy(box.gameObject);
    // }
}
