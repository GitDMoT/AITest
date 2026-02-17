using UnityEngine;

/// <summary>
/// Level-up screen for unlocking/upgrading cards.
/// </summary>
public class LevelUpUI : MonoBehaviour
{
    public CardManager CardManager;
    public GameObject LevelUpPanel;
    public CardData[] UnlockableCards;

    private void Update()
    {
        if (LevelUpPanel)
        {
            bool show = GameManager.Instance.CurrentState == GameManager.GameState.LevelUp;
            LevelUpPanel.SetActive(show);
            if (show) Time.timeScale = 0f;
        }
    }

    public void OnSelectNewCard(int index)
    {
        if (index >= 0 && index < UnlockableCards.Length)
            CardManager.AddCardToDeck(UnlockableCards[index]);
        Close();
    }

    private void Close() { Time.timeScale = 1f; GameManager.Instance.ResumePlaying(); }
}
