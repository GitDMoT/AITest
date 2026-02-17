using UnityEngine;

/// <summary>
/// XP and levelling. Doubles each level: 10, 20, 40, 80.
/// </summary>
public class ExperienceSystem : MonoBehaviour
{
    public int BaseXPRequired = 10;
    public float XPMultiplier = 2f;
    public int CurrentLevel = 1;
    public int CurrentXP = 0;

    public int XPToNextLevel => Mathf.RoundToInt(BaseXPRequired * Mathf.Pow(XPMultiplier, CurrentLevel - 1));
    public float LevelProgress => (float)CurrentXP / XPToNextLevel;

    public void AddExperience(int amount)
    {
        CurrentXP += amount;
        while (CurrentXP >= XPToNextLevel)
        {
            CurrentXP -= XPToNextLevel;
            CurrentLevel++;
            Debug.Log($"Level Up! Now level {CurrentLevel}");
            if (GameManager.Instance != null) GameManager.Instance.TriggerLevelUp();
        }
    }
}
