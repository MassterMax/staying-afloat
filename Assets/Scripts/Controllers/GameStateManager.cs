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
    HyperJumpController hyperJumpController;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Called OnSceneLoaded");
        if (scene.name == GAME_SCENE_NAME)
        {
            Debug.Log("Called OnSceneLoaded in GAME_SCENE_NAME");
            uiManager = FindAnyObjectByType<UIManager>();
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
        hyperJumpController.DoHyperJump(() => LoadAfterJump());
    }

    void LoadAfterJump()
    {
        inStart = false;
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    public void LoadStart()
    {
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
}
