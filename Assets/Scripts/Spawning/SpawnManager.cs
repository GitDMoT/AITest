using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Point-based enemy spawning with ramp and 3 reset points per stage.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    public float SpawnTickInterval = 2f;
    public float BaseSpawnPoints = 2f;
    public float MaxSpawnPoints = 15f;
    public float SpawnDistance = 12f;
    public GameObject BladeZombiePrefab;
    public GameObject OgrePrefab;
    public float[] ResetPoints = { 0.25f, 0.5f, 0.75f };
    public float ResetMultiplier = 0.4f;

    [System.Serializable]
    public class SpawnableEnemy { public GameObject Prefab; public string Name; public int PointCost; public int MinStage; }

    private List<SpawnableEnemy> enemies = new List<SpawnableEnemy>();
    private float spawnTimer, currentPoints;
    private int currentStage;
    private bool[] resetTriggered;
    private Transform playerTransform;

    private void Start() { if (GameManager.Instance.Player != null) playerTransform = GameManager.Instance.Player.transform; }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing || playerTransform == null) return;
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnTickInterval) { spawnTimer = 0f; SpawnTick(); }
        UpdateIntensity();
    }

    public void InitializeStage(int stage)
    {
        currentStage = stage; currentPoints = BaseSpawnPoints; spawnTimer = 0f;
        resetTriggered = new bool[ResetPoints.Length];
        enemies.Clear();
        if (BladeZombiePrefab) enemies.Add(new SpawnableEnemy { Prefab = BladeZombiePrefab, Name = "Blade Zombie", PointCost = 1, MinStage = 1 });
        if (OgrePrefab) enemies.Add(new SpawnableEnemy { Prefab = OgrePrefab, Name = "Ogre", PointCost = 5, MinStage = 1 });
    }

    private void UpdateIntensity()
    {
        float progress = GameManager.Instance.GetStageProgress();
        for (int i = 0; i < ResetPoints.Length; i++)
            if (!resetTriggered[i] && progress >= ResetPoints[i]) { resetTriggered[i] = true; currentPoints *= ResetMultiplier; }
        currentPoints += (MaxSpawnPoints - BaseSpawnPoints) / GameManager.Instance.StageDuration * Time.deltaTime;
        currentPoints = Mathf.Min(currentPoints, MaxSpawnPoints);
    }

    private void SpawnTick()
    {
        float pts = currentPoints;
        while (pts > 0)
        {
            var affordable = enemies.FindAll(e => e.PointCost <= pts && e.MinStage <= currentStage);
            if (affordable.Count == 0) break;
            var chosen = affordable[Random.Range(0, affordable.Count)];
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 pos = playerTransform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * SpawnDistance;
            if (chosen.Prefab) Instantiate(chosen.Prefab, pos, Quaternion.identity);
            pts -= chosen.PointCost;
        }
    }
}
