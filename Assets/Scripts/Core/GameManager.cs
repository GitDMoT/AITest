using UnityEngine;

/// <summary>
/// Central game manager that coordinates all game systems.
/// Handles game state, stage progression, and system initialization.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public GameState CurrentState = GameState.Playing;
    public int CurrentStage = 1;
    public float StageTimer = 0f;

    [Header("Stage Settings")]
    [Tooltip("Duration of each stage in seconds")]
    public float StageDuration = 300f; // 5 minutes

    [Header("References")]
    public PlayerController Player;
    public CardManager CardManager;
    public EnergySystem EnergySystem;
    public ExperienceSystem ExperienceSystem;
    public SpawnManager SpawnManager;

    public enum GameState
    {
        Playing,
        Paused,
        StageTransition,
        GameOver,
        LevelUp
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartStage(1);
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing) return;
        StageTimer += Time.deltaTime;
        if (StageTimer >= StageDuration)
        {
            CompleteStage();
        }
    }

    public void StartStage(int stageNumber)
    {
        CurrentStage = stageNumber;
        StageTimer = 0f;
        CurrentState = GameState.Playing;
        if (SpawnManager != null)
        {
            SpawnManager.InitializeStage(stageNumber);
        }
        Debug.Log($"Stage {stageNumber} started!");
    }

    private void CompleteStage()
    {
        CurrentState = GameState.StageTransition;
        Debug.Log($"Stage {CurrentStage} complete!");
        Invoke(nameof(StartNextStage), 3f);
    }

    private void StartNextStage()
    {
        StartStage(CurrentStage + 1);
    }

    public void OnPlayerDeath()
    {
        CurrentState = GameState.GameOver;
        Debug.Log("Game Over!");
    }

    public void TriggerLevelUp()
    {
        CurrentState = GameState.LevelUp;
    }

    public void ResumePlaying()
    {
        CurrentState = GameState.Playing;
    }

    /// <summary>
    /// Returns the normalized progress through the current stage (0 to 1).
    /// </summary>
    public float GetStageProgress()
    {
        return Mathf.Clamp01(StageTimer / StageDuration);
    }
}
