using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private const string GAME_SCENE_NAME = "Game";
    private const string START_SCENE_NAME = "Start";
    UIManager uiManager;
    bool paused = false;
    public bool Paused => paused;
    public static GameStateManager Instance { get; private set; }
    public event Action OnPauseStateChanged;
    private bool inStart = false;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Called OnSceneLoaded");
        if (scene.name == GAME_SCENE_NAME)
        {
            Debug.Log("Called OnSceneLoaded in GAME_SCENE_NAME");
            uiManager = FindAnyObjectByType<UIManager>();
            ResumeGame();
        }
    }

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // uiManager = FindAnyObjectByType<UIManager>();
        // ResumeGame();

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals(START_SCENE_NAME))
        {
            inStart = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inStart && Input.anyKeyDown)
        {
            LoadGame();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            HandlePause();
        }
    }

    void LoadGame()
    {
        inStart = false;
        SceneManager.LoadScene(GAME_SCENE_NAME);
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
}
