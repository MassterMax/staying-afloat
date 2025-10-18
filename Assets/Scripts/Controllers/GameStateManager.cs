using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    private const string GAME_SCENE_NAME = "Game";
    private const string START_SCENE_NAME = "Start";
    UIManager uiManager;
    StatsManager statsManager;
    BlackHoleController blackHoleController;
    bool paused = false;
    bool end = false;
    bool canReturn = false;
    public bool Paused => paused;
    public static GameStateManager Instance { get; private set; }
    public event Action OnPauseStateChanged;
    private bool inStart = false;
    HyperJumpController hyperJumpController;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Called OnSceneLoaded");
        if (scene.name == GAME_SCENE_NAME)
        {
            Debug.Log("Called OnSceneLoaded in GAME_SCENE_NAME");
            uiManager = FindAnyObjectByType<UIManager>();
            statsManager = FindAnyObjectByType<StatsManager>();
            blackHoleController = FindAnyObjectByType<BlackHoleController>();
            AllStatsContainer.Instance.ResetStats();
            ResumeGame();
        }
        else if (scene.name == START_SCENE_NAME)
        {
            Debug.Log("Called OnSceneLoaded in START_SCENE_NAME");
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
            }
            return;
        }

        if (inStart && Input.anyKeyDown)
        {
            LoadGame();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            HandlePause();
        }
    }

    public void FlyAway(Action action)
    {
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

    void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1f;
        uiManager.HandlePause(paused);
        OnPauseStateChanged?.Invoke();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoseBlackHole()
    {
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
}
