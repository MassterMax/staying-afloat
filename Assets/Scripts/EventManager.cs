using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    float minTimeToNextEvent = 5f;
    float maxTimeToNextEvent = 15f;
    float timeToNextEvent = 10f;
    float timer = 0f;

    [Header("Spawn settings")]
    [SerializeField] Vector3 spawnOffset = new Vector3(6f, 0f, 0f);
    [SerializeField] Vector2 targetOffsetRangeX = new Vector2(2f, 6f);
    [SerializeField] Vector2 targetOffsetRangeY = new Vector2(-1f, 1f);
    [SerializeField] Vector2 lootRange = new Vector2(5, 30); // энергия в коробке

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToNextEvent)
        {
            CreateEvent();
            timer = 0f;
            timeToNextEvent = Random.Range(minTimeToNextEvent, maxTimeToNextEvent);
        }

        // for debug
        if (Input.GetKeyDown(KeyCode.E))
        {
            CreateEvent();
        }
    }

    void CreateEvent()
    {
        Debug.Log("Event Created");
        SpawnBox();
    }

    void SpawnBox()
    {
        if (boxPrefab == null) return;

        Vector2 spawnPos = GetRandomEllipsePoint();
        Vector2 targetPos = GetRandomPointInOtherQuadrant(spawnPos, 12f, 6f);

        GameObject go = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
        Box boxComp = go.GetComponent<Box>();
        float energyLoot = GetBoxEnergy();
        boxComp.Init(spawnPos, targetPos, null, energyLoot);
    }

    float GetBoxEnergy()
    {
        float p = Random.Range(0f, 1f);
        if (p < 0.5f) return 5f;
        else if (p < 0.85f) return 10f;
        else return 20f;
    }

    Vector2 GetRandomEllipsePoint(float minAngle = Mathf.PI / 2f, float maxAngle = 3f * Mathf.PI / 2f, float radiusX = 12f, float radiusY = 6f)
    {
        float angle = Random.Range(minAngle, maxAngle);
        float x = Mathf.Cos(angle) * radiusX;
        float y = Mathf.Sin(angle) * radiusY;
        return new Vector2(x, y);
    }

    int GetQuadrant(Vector2 point, Vector2 center)
    {
        if (point.x >= center.x && point.y >= center.y) return 1;
        if (point.x < center.x && point.y >= center.y) return 2;
        if (point.x < center.x && point.y < center.y) return 3;
        return 4; // x >= center.x && y < center.y
    }

    Vector2 GetRandomPointInOtherQuadrant(Vector2 currentPoint, float radiusX, float radiusY)
    {
        int currentQuadrant = GetQuadrant(currentPoint, Vector2.zero);

        // список возможных четвертей кроме текущей
        int[] otherQuadrants = new int[3];
        int idx = 0;
        for (int q = 1; q <= 4; q++)
        {
            if (q != currentQuadrant)
            {
                otherQuadrants[idx++] = q;
            }
        }

        // случайная четверть
        int targetQuadrant = otherQuadrants[Random.Range(0, 3)];

        // генерируем случайный угол в этой четверти
        float angle = 0f;
        switch (targetQuadrant)
        {
            case 1: angle = Random.Range(0f, Mathf.PI / 2f); break;
            case 2: angle = Random.Range(Mathf.PI / 2f, Mathf.PI); break;
            case 3: angle = Random.Range(Mathf.PI, 3f * Mathf.PI / 2f); break;
            case 4: angle = Random.Range(3f * Mathf.PI / 2f, 2f * Mathf.PI); break;
        }

        float x = Mathf.Cos(angle) * radiusX;
        float y = Mathf.Sin(angle) * radiusY;

        return new Vector2(x, y);
    }
}