using System;
using System.Collections.Generic;
using UnityEngine;
using RewardSystem;
using GameSystem;
using UI;
using ConfigSystem;

namespace BombSystem
{
    public class BombHandler : MonoBehaviour
    {
    public static BombHandler Instance { get; private set; }

    [Header("Configuration")]
    [SerializeField] private ReviveConfig reviveConfig;

    public event Action OnBombHit;
    public event Action<List<Reward>> OnRewardsLost;

    private int reviveCount = 0;

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

    private void Start()
    {
        ValidateReferences();
    }

    private void ValidateReferences()
    {
        if (reviveConfig == null)
            Debug.LogError("[BombHandler] ReviveConfig is not assigned!");
    }

    public void HandleBombHit()
    {
        Debug.Log("[BombHandler] BOMB HIT! Waiting for player decision...");
        
        OnBombHit?.Invoke();
        
    }

    public void GiveUp()
    {
        Debug.Log("[BombHandler] Player gave up! All rewards lost!");

        List<Reward> lostRewards = GameStateManager.Instance.GetCollectedRewards();

        GameStateManager.Instance.ClearRewards();

        OnRewardsLost?.Invoke(lostRewards);

        reviveCount = 0;
        Debug.Log("[BombHandler] Revive counter reset");

        Debug.Log("[BombHandler] Restarting game...");
        GameStateManager.Instance.RestartGame();
    }

    public bool MoneyRevive(int baseCost)
    {
        int actualCost = GetCurrentReviveCost();
        
        Debug.Log($"[BombHandler] Money revive attempt! Base: ${baseCost}, Actual: ${actualCost}, Revive count: {reviveCount}");

        var cashController = FindObjectOfType<CashDisplayController>();
        if (cashController != null && cashController.GetCurrentCash() >= actualCost)
        {
            cashController.AddCash(-actualCost);
            reviveCount++;
            Debug.Log($"[BombHandler] Revive successful! New revive count: {reviveCount}, Next cost will be: ${GetCurrentReviveCost()}");
            Revive();
            return true;
        }
        else
        {
            Debug.LogWarning($"[BombHandler] Not enough money to revive! Need: ${actualCost}, Have: ${cashController?.GetCurrentCash() ?? 0}");
            return false;
        }
    }

    public void AdsRevive()
    {
        Debug.Log("[BombHandler] Ads revive! (Simulated)");
        
        Revive();
    }

    private void Revive()
    {
        Debug.Log("[BombHandler] Player revived! Continuing game...");
        
        GameStateManager.Instance.SetState(GameState.Playing);
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

    public int GetCurrentReviveCost()
    {
        if (reviveConfig == null)
        {
            Debug.LogWarning("[BombHandler] ReviveConfig is null, using default cost");
            return 1000;
        }

        float cost = reviveConfig.reviveCost * Mathf.Pow(reviveConfig.reviveCostMultiplier, reviveCount);
        return Mathf.RoundToInt(cost);
    }

    public void ResetReviveCounter()
    {
        reviveCount = 0;
        Debug.Log("[BombHandler] Revive counter reset");
    }

    public int GetReviveCount()
    {
        return reviveCount;
    }
    }
}
