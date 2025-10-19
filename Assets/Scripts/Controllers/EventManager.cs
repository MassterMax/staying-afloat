using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    [SerializeField] GameObject goldenBoxPrefab;
    [SerializeField] GameObject asteroidPrefab;
    float minTimeToNextEvent = 2f;
    float maxTimeToNextEvent = 7f;
    float timeToNextEvent;
    float timer = 0f;
    float massiveTimer = 0f;
    float timeToNextMassiveEvent = 20f;
    TimeController timeController;

    void Awake()
    {
        timeController = FindAnyObjectByType<TimeController>();
    }
    void Start()
    {
        CalculateTimeToNextEvent();
    }

    void CalculateTimeToNextEvent()
    {
        timeToNextEvent = Random.Range(minTimeToNextEvent, maxTimeToNextEvent);
        timeToNextEvent -= AllStatsContainer.Instance.TimeBetweenEventsMinus(timeController.GetLastReportedGameHours());
        timeToNextEvent = Mathf.Max(0.1f, timeToNextEvent);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        massiveTimer += Time.deltaTime;
        if (timer >= timeToNextEvent)
        {
            CreateEvent();
            timer = 0f;
            CalculateTimeToNextEvent();
        }
        if (massiveTimer >= timeToNextMassiveEvent)
        {
            massiveTimer = 0f;
            CreateMassiveEvent();
        }

        // for debug
        if (Input.GetKeyDown(KeyCode.E))
        {
            CreateEvent();
        }
    }

    void CreateMassiveEvent()
    {
        Debug.Log("CreateMassiveEvent");
        int days = timeController.LastReportedGameHours / 24;
        days -= 5;
        int cnt = Mathf.Min(days / 2, 5);
        for (int i = 0; i < cnt; ++i)
        {
            SpawnAsteroid();
        }
    }

    void CreateEvent()
    {
        Debug.Log("Event Created");
        bool created = false;
        float t = Random.Range(0f, 1f);
        if (t < AllStatsContainer.Instance.GetAsteroidChance(timeController.GetLastReportedGameHours()))
        {
            SpawnAsteroid();
            created = true;
        }

        t = Random.Range(0f, 1f);
        if (t < AllStatsContainer.Instance.GetBoxChance(timeController.GetLastReportedGameHours()))
        {
            SpawnBox();
            created = true;
        }

        t = Random.Range(0f, 1f);
        if (t < AllStatsContainer.Instance.GetGoldenBoxChance(timeController.GetLastReportedGameHours()))
        {
            SpawnGoldenBox();
            created = true;
        }

        if (!created)
        {
            SpawnBox();
        }
    }

    void SpawnAsteroid()
    {
        if (asteroidPrefab == null) return;

        Vector2 spawnPos = GetRandomEllipsePoint();
        float t = Random.Range(0f, 1f);
        Vector2 targetPos = Vector2.zero;
        if (t > AllStatsContainer.Instance.AsteroidHitChance)
        {
            Debug.Log("Missing asteroid!");
            Vector2 dir = Vector2.right;
            if (spawnPos.y > 0)
            {
                dir += Vector2.up;
            }
            else
            {
                dir += Vector2.down;
            }
            dir *= 2;
            targetPos = Vector2.zero + dir; // center + small offset (to feel fear)
            Vector2 asteroidCourse = targetPos - spawnPos;
            targetPos += asteroidCourse.normalized * 20f;
        }

        GameObject go = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
        Asteroid asteroidComp = go.GetComponent<Asteroid>();
        asteroidComp.Init(spawnPos, targetPos);
    }

    void SpawnBox()
    {
        Vector2 spawnPos = GetRandomEllipsePoint();
        Vector2 targetPos = GetRandomPointInOtherQuadrant(spawnPos, 12f, 8f);

        GameObject go = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
        Box boxComp = go.GetComponent<Box>();
        float energyLoot = AllStatsContainer.Instance.GetBoxEnergy();
        boxComp.Init(spawnPos, targetPos, null, energyLoot);
    }

    // will give improvements (not implemented yet)
    void SpawnGoldenBox()
    {
        Vector2 spawnPos = GetRandomEllipsePoint();
        Vector2 targetPos = GetRandomPointInOtherQuadrant(spawnPos, 12f, 8f);

        GameObject go = Instantiate(goldenBoxPrefab, spawnPos, Quaternion.identity);
        GoldenBox boxComp = go.GetComponent<GoldenBox>();
        boxComp.Init(spawnPos, targetPos, null);
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