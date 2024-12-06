using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pepperPrefab;
    [SerializeField]
    private float timeout = 5.0f;
    private float leftTime;

    void Start()
    {
        GameState.AddListener(nameof(GameState.isBurst), OnBurstChanged);
        leftTime = timeout;
    }

    void Update()
    {
        if (leftTime > 0f && !GameState.isBurst)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0f)
            {
                SpawnPepper();
                leftTime = timeout;

			}
        }
    }
    private void SpawnPepper()
    {
        var pepper = GameObject.Instantiate(pepperPrefab);
        Vector3 pos = new Vector3(Random.Range(50, 950), 0, Random.Range(50, 950));
        pos.y = Terrain.activeTerrain.SampleHeight(pos) + Random.Range(0.5f, 10.0f);
        pepper.transform.position = pos;
    }
    private void OnBurstChanged(string ignored)
    {
        if ( ! GameState.isBurst)
        {
            leftTime = timeout;
        }        
    }
    private void OnDestroy()
    {
        GameState.RemoveListener(nameof(GameState.isBurst), OnBurstChanged);
    }
}



/* Д.З. До Display - відображення додати відомості про 
 * кількість зібраних "монет". Забезпечити адаптивність
 * до змін розмірів вікна.
 */
