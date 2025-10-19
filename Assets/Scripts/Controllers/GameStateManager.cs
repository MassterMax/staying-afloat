using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private const int START_SAVE = 24 * 9;
    [SerializeField] GameObject explosionPrefab;
    private const string GAME_SCENE_NAME = "Game";
    private const string START_SCENE_NAME = "Start";
    UIManager uiManager;
    StatsManager statsManager;
    BlackHoleController blackHoleController;
    bool readingPage = false;
    bool paused = false;
    bool end = false;
    bool canReturn = false;
    public bool Paused => paused;
    public static GameStateManager Instance { get; private set; }
    public event Action OnPauseStateChanged;
    private bool inStart = false;
    HyperJumpController hyperJumpController;
    TimeController timeController;
    AudioManager audioManager;

    // saved state
    bool saved = false;
    private int savedHP;
    private float savedEnergy;
    private float savedDistance;
    private float savedElapsedRealSeconds;
    private int savedLastReportedGameHours;
    private int savedRegenUpgrade = 0;
    private int savedHookUprgade = 0;
    private int savedGunUprgade = 0;
    private int savedEngineUpgrade = 0;
    // upgrades
    private int regenUpgrade = 0;
    private int hookUprgade = 0;
    private int gunUprgade = 0;
    private int engineUpgrade = 0;
    // options
    int firstOptionIndex;
    int secondOptionIndex;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Called OnSceneLoaded");
        if (scene.name == GAME_SCENE_NAME)
        {
            Debug.Log("Called OnSceneLoaded in GAME_SCENE_NAME");
            uiManager = FindAnyObjectByType<UIManager>();
            statsManager = FindAnyObjectByType<StatsManager>();
            blackHoleController = FindAnyObjectByType<BlackHoleController>();

            // unsubscribe from previous timeController (if any) then subscribe to the new one
            // if (timeController != null)
            //     timeController.LastReportedGameHoursUpdated -= HandleLastReportedGameHoursUpdated;
            timeController = FindAnyObjectByType<TimeController>();
            timeController.LastReportedGameHoursUpdated += HandleLastReportedGameHoursUpdated;
            StartGame();
        }
        else if (scene.name == START_SCENE_NAME)
        {
            Debug.Log("Called OnSceneLoaded in START_SCENE_NAME");
            // saved = false;
        }
        hyperJumpController = FindAnyObjectByType<HyperJumpController>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        audioManager = GetComponent<AudioManager>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals(START_SCENE_NAME))
        {
            inStart = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            if (canReturn && Input.anyKeyDown)
            {
                LoadStart();
                // if (saved)
                // {
                //     Debug.Log("Load save!");
                //     LoadLastSave();
                // }
                // else
                // {
                //     LoadStart();
                // }
            }
            return;
        }

        if (inStart)
        {
            if (Input.anyKeyDown)
            {
                LoadGame();
            }
            return;
        }

        if (readingPage)
        {
            // Debug.Log("GameStateManager: Inside readingPage");
            return;
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            HandlePause();
        }
    }

    public void FlyAway(Action action)
    {
        if (end) return; // already finished

        saved = false;
        end = true;
        hyperJumpController.DoHyperJump(() =>
        {
            paused = true;
            Time.timeScale = 0f;
            action?.Invoke();
            canReturn = true;
        }, keepPanel: false);
    }

    void LoadGame()
    {
        hyperJumpController.DoHyperJump(() => LoadAfterJump());
    }

    void LoadAfterJump()
    {
        inStart = false;
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    public void LoadStart()
    {
        canReturn = false;
        end = false;
        inStart = true;
        SceneManager.LoadScene(START_SCENE_NAME);
        paused = false;
        Time.timeScale = 1f;
    }

    // private void LoadLastSave()
    // {
    //     SceneManager.LoadScene(GAME_SCENE_NAME);
    // }

    void HandlePause()
    {
        paused = !paused;
        Debug.Log("Game Paused: " + paused);
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        uiManager.HandlePause(paused);
        OnPauseStateChanged?.Invoke();
    }

    public void ResumeGameAfterPage()
    {
        paused = false;
        readingPage = false;
        Time.timeScale = 1f;
        OnPauseStateChanged?.Invoke();
    }

    void StartGame()
    {
        if (saved)
        {
            Debug.Log("got saved state!");
            AllStatsContainer.Instance.ResetStats();
            timeController.LoadSave(savedElapsedRealSeconds, savedLastReportedGameHours);
            statsManager.LoadSave(savedDistance, savedEnergy, savedHP);
            regenUpgrade = savedRegenUpgrade;
            hookUprgade = savedHookUprgade;
            gunUprgade = savedGunUprgade;
            engineUpgrade = savedEngineUpgrade;
            AllStatsContainer.Instance.LoadUpgrades(regenUpgrade, hookUprgade, gunUprgade, engineUpgrade);
            uiManager.ShowUpgradeStats(regenUpgrade, hookUprgade, gunUprgade, engineUpgrade);
            paused = false;
            readingPage = false;
            Time.timeScale = 1f;
            OnPauseStateChanged?.Invoke();
        }
        else
        {
            statsManager.Reset();
            timeController.Reset();
            ShowDayPage(1);
            AllStatsContainer.Instance.ResetStats();

            regenUpgrade = 0;
            hookUprgade = 0;
            gunUprgade = 0;
            engineUpgrade = 0;
            uiManager.ShowUpgradeStats(regenUpgrade, hookUprgade, gunUprgade, engineUpgrade);
        }
    }

    public void GoldenPickup()
    {
        firstOptionIndex = UnityEngine.Random.Range(1, 5);
        secondOptionIndex = UnityEngine.Random.Range(0, 5);
        if (secondOptionIndex == firstOptionIndex)
        {
            secondOptionIndex = 0;
        }
        ShowOptionPage();
    }

    public void ResumeGameAfterOptionPage(bool firstOptionChoosed)
    {
        Play("powerUp");
        Debug.Log("firstOptionChoosed: " + firstOptionChoosed);
        int selected = firstOptionChoosed ? firstOptionIndex : secondOptionIndex;

        if (selected == 0)
        {
            statsManager.AddEnergyFromUpgrade(50f);
        }
        else if (selected == 1)
        {
            regenUpgrade += 1;
        }
        else if (selected == 2)
        {
            hookUprgade += 1;
        }
        else if (selected == 3)
        {
            gunUprgade += 1;
        }
        else if (selected == 4)
        {
            engineUpgrade += 1;
        }

        paused = false;
        readingPage = false;
        Time.timeScale = 1f;
        OnPauseStateChanged?.Invoke();
        AllStatsContainer.Instance.LoadUpgrades(regenUpgrade, hookUprgade, gunUprgade, engineUpgrade);
        uiManager.ShowUpgradeStats(regenUpgrade, hookUprgade, gunUprgade, engineUpgrade);
    }

    void ShowOptionPage()
    {
        Play("paperSheet");
        Time.timeScale = 0f;
        paused = true;
        readingPage = true;
        OnPauseStateChanged?.Invoke();
        string option1Replic = ReplicasController.Get("option_" + firstOptionIndex);
        string option2Replic = ReplicasController.Get("option_" + secondOptionIndex);
        uiManager.ShowOptionPage(ReplicasController.Get("option_question"), option1Replic, option2Replic);
    }

    void ShowDayPage(int i)
    {
        Play("paperSheet");
        Time.timeScale = 0f;
        paused = true;
        readingPage = true;
        OnPauseStateChanged?.Invoke();
        uiManager.ShowSimplePage(ReplicasController.Get("day_" + i));
    }

    void HandleLastReportedGameHoursUpdated()
    {
        if (timeController.GetLastReportedGameHours() == 24 * 1)
        {
            ShowDayPage(2);
        }
        if (timeController.GetLastReportedGameHours() == 24 * 2)
        {
            ShowDayPage(3);
        }
        if (timeController.GetLastReportedGameHours() == 24 * 5)
        {
            ShowDayPage(6);
        }
        if (timeController.GetLastReportedGameHours() == 24 * 8)
        {
            ShowDayPage(9);
        }

        if (timeController.GetLastReportedGameHours() >= 24 * 9 && (timeController.GetLastReportedGameHours() + 24) % (24 * 5) == 0)
        {
            Debug.Log("save state!");
            savedElapsedRealSeconds = timeController.ElapsedRealSeconds;
            savedLastReportedGameHours = timeController.LastReportedGameHours;
            savedEnergy = statsManager.Energy;
            savedDistance = statsManager.Distance;
            savedHP = statsManager.CurrentHp;
            savedRegenUpgrade = regenUpgrade;
            savedHookUprgade = hookUprgade;
            savedGunUprgade = gunUprgade;
            savedEngineUpgrade = engineUpgrade;
            saved = true;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            timeController.LastReportedGameHoursUpdated -= HandleLastReportedGameHoursUpdated;
        }
    }

    public void LoseBlackHole()
    {
        if (end) return; // already finished
        end = true;
        statsManager.LoseAllEnergy();
        uiManager.HideShipUIPanel();
        StartCoroutine(LoseAfterBH(ShowLoseScreen));
    }

    private void ShowLoseScreen()
    {
        uiManager.ShowLosePanel();
        canReturn = true;
    }

    private IEnumerator LoseAfterBH(Action action)
    {
        while (blackHoleController.GetScale() < 2f)
        {
            yield return new WaitForFixedUpdate();
        }
        paused = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);
        action?.Invoke();
    }

    public void LoseExplosion()
    {
        if (end) return; // already finished

        end = true;
        statsManager.LoseAllEnergy();
        uiManager.HideShipUIPanel();
        StartCoroutine(LoseAfterExplosion(ShowLoseScreen));
    }

    private IEnumerator LoseAfterExplosion(Action action)
    {
        // TODO create explosion
        // while (blackHoleController.GetScale() < 2f)
        // {
        //     yield return new WaitForFixedUpdate();
        // }
        List<GameObject> explosions = new List<GameObject>();
        for (int i = 0; i <= 5; ++i)
        {
            yield return new WaitForSeconds(0.2f);
            Vector2 spawnPos = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
            GameObject spawned = Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
            explosions.Add(spawned);
        }
        // yield return new WaitForSeconds(2f);
        // for (int i = 0; i < 5; ++i)
        // {
        //     Debug.Log(explosions[i]);
        // }
        while (explosions.Any(x => x != null))
        {
            yield return new WaitForFixedUpdate();
        }
        paused = true;
        Time.timeScale = 0f;
        // yield return new WaitForSecondsRealtime(3f);
        action?.Invoke();
    }

    public void Play(string name)
    {
        audioManager.Play(name);
    }

    public void ContinuePlayBH()
    {
        audioManager.PlayBH();
    }

    public void StopPlayBH()
    {
        audioManager.PauseBH();
    }

    public void ChangeVolume(float volume, bool sfx)
    {
        audioManager.ChangeVolume(volume, sfx);
    }

    public float GetVolume(bool sfx)
    {
        return audioManager.GetVolume(sfx);
    }
}
