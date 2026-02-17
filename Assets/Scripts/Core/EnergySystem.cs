using UnityEngine;

/// <summary>
/// Manages player energy. Starts at 3, regens 1 every 2 seconds.
/// </summary>
public class EnergySystem : MonoBehaviour
{
    public int MaxEnergy = 3;
    public float CurrentEnergy;
    public float RegenRate = 0.5f;
    public int AvailableEnergy => Mathf.FloorToInt(CurrentEnergy);

    private void Start() { CurrentEnergy = MaxEnergy; }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += RegenRate * Time.deltaTime;
            CurrentEnergy = Mathf.Min(CurrentEnergy, MaxEnergy);
        }
    }

    public bool TrySpendEnergy(int amount)
    {
        if (AvailableEnergy >= amount) { CurrentEnergy -= amount; return true; }
        return false;
    }

    public float GetRegenProgress() { return CurrentEnergy - Mathf.Floor(CurrentEnergy); }
}
