using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScreen : BaseScreen
{

    [SerializeField] TextMeshProUGUI collectedGoldText;
    [SerializeField] Spawner spawner;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(StartGame());
        GameManager.Instance.OnGameOverEvent += GameManager_OnGameOverEvent;
        GameManager.Instance.OnGameWonEvent += GameManager_OnGameWonEvent;
    }

    private void GameManager_OnGameWonEvent(object sender, GameManager.OnGameWonEventEventArgs e)
    {
        OpenPopup<WinPopup>("Popups/WinPopup", popup =>
        {
            popup.SetFinalResult(e.score, e.coins);
            CoinManager.Instance.AddCoin(e.coins);
            ScoreManager.Instance.SendHighScore(e.score);
        });
    }

    private void GameManager_OnGameOverEvent(object sender, System.EventArgs e)
    {
        OpenPopup<LosePopup>("Popups/LosePopup");
    }

    public void OnPauseButtonPressed()
    {
        OpenPopup<PausePopup>("Popups/PausePopup");
    }

    public void AddGold(int amount)
    {
        CoinManager.Instance.AddCoinToShow(amount);
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        collectedGoldText.text = CoinManager.Instance.TotalCoin.ToString();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);
        CoinManager.Instance.ResetCoin();
        UpdateGoldText();
        spawner.StartWaves();
    }
}
