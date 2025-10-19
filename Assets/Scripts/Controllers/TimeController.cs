using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // elapsed real seconds since timer started
    private float elapsedRealSeconds;
    // last reported total game hours (integer)
    private int lastReportedGameHours;
    private bool running = false;

    public float ElapsedRealSeconds => elapsedRealSeconds;
    public int LastReportedGameHours => lastReportedGameHours;

    UIManager uiManager;
    public event Action LastReportedGameHoursUpdated;

    void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    void Start()
    {
        // if (startOnAwake)
        //     StartTimer();

    }

    void Update()
    {
        if (!running) return;

        // debug
        if (Input.GetKeyDown(KeyCode.S))
        {
            elapsedRealSeconds += 10f;
        }

        elapsedRealSeconds += Time.deltaTime;
        // 1 real sec == 1 game hour
        int totalGameHours = Mathf.FloorToInt(elapsedRealSeconds);

        if (totalGameHours != lastReportedGameHours)
        {
            lastReportedGameHours = totalGameHours;
            PushTimeToUI();
            LastReportedGameHoursUpdated?.Invoke();
        }
    }

    public void StartTimer()
    {
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

    public int GetLastReportedGameHours()
    {
        return lastReportedGameHours;
    }

    public void LoadSave(float _elapsedRealSeconds, int _lastReportedGameHours)
    {
        elapsedRealSeconds = _elapsedRealSeconds;
        lastReportedGameHours = _lastReportedGameHours;
        StartTimer();
    }

    public void Reset()
    {
        LoadSave(0f, -1);
    }
}
