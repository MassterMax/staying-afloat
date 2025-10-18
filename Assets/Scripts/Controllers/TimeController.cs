using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Tooltip("If true, timer starts automatically on Start()")]
    [SerializeField] private bool startOnAwake = false;

    // elapsed real seconds since timer started
    private float elapsedRealSeconds = 0f;
    // last reported total game hours (integer)
    private int lastReportedGameHours = -1;
    private bool running = false;

    UIManager uiManager;

    void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    void Start()
    {
        // if (startOnAwake)
        //     StartTimer();
        StartTimer();
    }

    void Update()
    {
        if (!running) return;

        elapsedRealSeconds += Time.deltaTime;
        // 1 real sec == 1 game hour
        int totalGameHours = Mathf.FloorToInt(elapsedRealSeconds);

        if (totalGameHours != lastReportedGameHours)
        {
            lastReportedGameHours = totalGameHours;
            PushTimeToUI();
        }
    }

    public void StartTimer()
    {
        elapsedRealSeconds = 0f;
        lastReportedGameHours = -1;
        running = true;
        // immediate update to show Day 1 — 00:00
        PushTimeToUI();
    }

    public void StopTimer()
    {
        running = false;
    }

    void PushTimeToUI()
    {
        uiManager.UpdateDayAndTime(lastReportedGameHours);
    }
}
