using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// In-game HUD for health, energy, cards, XP, and stage progress.
/// </summary>
public class GameHUD : MonoBehaviour
{
    public PlayerController Player;
    public EnergySystem Energy;
    public CardManager Cards;
    public ExperienceSystem Experience;
    public Slider HealthBar, ShieldBar, EnergyRegenBar, DrawTimerBar, XPBar, StageProgressBar;
    public Text HealthText, EnergyText, HandText, LevelText, StageText;

    private void Update()
    {
        if (Player) {
            if (HealthBar) { HealthBar.maxValue = Player.MaxHealth; HealthBar.value = Player.CurrentHealth; }
            if (ShieldBar) { ShieldBar.maxValue = 200f; ShieldBar.value = Player.ShieldHealth; }
            if (HealthText) HealthText.text = $"HP: {Player.CurrentHealth}/{Player.MaxHealth}";
        }
        if (Energy) {
            if (EnergyText) EnergyText.text = $"Energy: {Energy.AvailableEnergy}/{Energy.MaxEnergy}";
            if (EnergyRegenBar) EnergyRegenBar.value = Energy.GetRegenProgress();
        }
        if (Cards) {
            if (DrawTimerBar) DrawTimerBar.value = Cards.GetDrawTimerProgress();
            if (HandText) {
                string h = ""; for (int i = Cards.Hand.Count - 1; i >= 0; i--) h += $"[{Cards.Hand[i].CardName}] ";
                HandText.text = $"Hand ({Cards.Hand.Count}/{Cards.MaxHandSize}): {h}";
            }
        }
        if (Experience) {
            if (LevelText) LevelText.text = $"Level {Experience.CurrentLevel}";
            if (XPBar) { XPBar.maxValue = Experience.XPToNextLevel; XPBar.value = Experience.CurrentXP; }
        }
        if (StageText) StageText.text = $"Stage {GameManager.Instance.CurrentStage}";
        if (StageProgressBar) StageProgressBar.value = GameManager.Instance.GetStageProgress();
    }
}
