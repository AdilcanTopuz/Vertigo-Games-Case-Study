using System;
using System.Collections.Generic;
using UnityEngine;
using RewardSystem;
using GameSystem;

public class BombHandler : MonoBehaviour
{
    public static BombHandler Instance { get; private set; }

    public event Action OnBombHit;
    public event Action<List<Reward>> OnRewardsLost;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HandleBombHit()
    {
        Debug.Log("[BombHandler] BOMB HIT! All rewards lost!");

        List<Reward> lostRewards = GameStateManager.Instance.GetCollectedRewards();

        GameStateManager.Instance.ClearRewards();

        OnRewardsLost?.Invoke(lostRewards);
        OnBombHit?.Invoke();

        GameStateManager.Instance.SetState(GameState.GameOver);
    }

    public bool CanContinue()
    {
        return false;
    }

    public void Continue()
    {
        if (!CanContinue()) return;

        GameStateManager.Instance.SetState(GameState.Playing);
    }
}
