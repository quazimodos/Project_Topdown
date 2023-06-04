using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGamePaused;

    Player player;
    [SerializeField] GameScreen gameScreen;
    [SerializeField] Spawner spawner;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] Core core;


    public event EventHandler OnGameOverEvent;
    public event EventHandler OnGamePauseEvent;
    public event EventHandler<OnBeforeSaveEventEventArgs> OnBeforeSaveEvent;
    public event EventHandler<OnGameWonEventEventArgs> OnGameWonEvent;

    public class OnGameWonEventEventArgs
    {
        public int score;
        public int coins;
    }

    public class OnBeforeSaveEventEventArgs
    {
        public GameObject target;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        player.OnDeath += OnGameOver;
        core.OnDeath += OnGameOver;
        spawner.OnGameCompleted += OnGameWon;
        spawner.OnBeforeSave += OnBeforeSave;
    }
    public void PauseGame(bool state)
    {
        isGamePaused = state;
        if (state)
        {
            OnGamePauseEvent?.Invoke(this, EventArgs.Empty);
        }
    }
    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    void OnBeforeSave()
    {
        OnBeforeSaveEvent?.Invoke(this, new OnBeforeSaveEventEventArgs { target = player.gameObject });
    }

    void OnGameOver()
    {
        OnGameOverEvent?.Invoke(this, EventArgs.Empty);
    }

    void OnGameWon()
    {
        OnGameWonEvent?.Invoke(this, new OnGameWonEventEventArgs
        {
            score = scoreManager.GetScore(),
            coins = CoinManager.Instance.TotalCoin
        });
    }
}
