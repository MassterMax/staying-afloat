using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    float minTimeToNextEvent = 5f;
    float maxTimeToNextEvent = 10f;
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
        Vector2 targetPos = GetRandomPointInOtherQuadrant(spawnPos, 12f, 8f);

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

    Vector2 GetRandomEllipsePoint(float minAngle = Mathf.PI / 2f, float maxAngle = 3f * Mathf.PI / 2f, float radiusX = 12f, float radiusY = 8f)
    {
        float angle = Random.Range(minAngle, maxAngle);
        float x = Mathf.Cos(angle) * radiusX;
        float y = Mathf.Sin(angle) * radiusY;
        return new Vector2(x, y);
    }

    Vector2 GetRandomPointInOtherQuadrant(Vector2 currentPoint, float radiusX, float radiusY)
    {
        // Генерирует точку на эллипсе так, чтобы её угол отличался от угла currentPoint
        // минимум на 90° (pi/2). Убираем деление на четверти и используем ограничение по углу.
        const float minDelta = Mathf.PI / 2f;      // 90 градусов
        const float maxDelta = 3f * Mathf.PI / 2f; // 270 градусов

        float currentAngle = Mathf.Atan2(currentPoint.y, currentPoint.x);
        // разрешённый диапазон углов: [currentAngle + minDelta, currentAngle + maxDelta]
        float angle = Random.Range(currentAngle + minDelta, currentAngle + maxDelta);

        float x = Mathf.Cos(angle) * radiusX;
        float y = Mathf.Sin(angle) * radiusY;
        return new Vector2(x, y);
    }

    void OnDrawGizmos()
    {
        // параметры должны совпадать с GetRandomEllipsePoint по умолчанию
        float radiusX = 12f;
        float radiusY = 8f;

        int segments = 128;
        Gizmos.color = Color.gray;

        Vector3 prev = new Vector3(Mathf.Cos(0f) * radiusX, Mathf.Sin(0f) * radiusY, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / segments * Mathf.PI * 2f;
            Vector3 curr = new Vector3(Mathf.Cos(t) * radiusX, Mathf.Sin(t) * radiusY, 0f);
            Gizmos.DrawLine(prev, curr);
            prev = curr;
        }
    }
}