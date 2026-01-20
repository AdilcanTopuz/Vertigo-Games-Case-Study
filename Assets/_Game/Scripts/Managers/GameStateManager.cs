using System;
using System.Collections.Generic;
using UnityEngine;
using RewardSystem;
using ZoneSystem;
using SpinSystem;
using WheelSystem;
using SaveSystem;
using BombSystem;


namespace GameSystem
{
    public enum GameState
    {
        Menu,
        Playing,
        GameOver,
        Victory
    }

    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Menu;

        private List<Reward> collectedRewards = new List<Reward>();

        public event Action<GameState> OnStateChanged;
        public event Action<Reward> OnRewardAdded;
        public event Action OnRewardsCleared;
        public event Action<List<Reward>> OnGameCompleted;

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

        public void StartGame()
        {
            ClearRewards();
            
            ZoneManager.Instance.ResetZone();
            WheelManager.Instance.LoadWheelForZone(1);

            if (BombHandler.Instance != null)
            {
                BombHandler.Instance.ResetReviveCounter();
            }

            SetState(GameState.Playing);

            Debug.Log("[GameStateManager] Game started!");
        }

        public void LeaveGame()
        {
            if (collectedRewards.Count > 0)
            {
                Debug.Log("=== SAVING GAME DATA ===");
                Debug.Log($"[GameStateManager] Saving {collectedRewards.Count} rewards to inventory...");
                
                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.SaveRewards(new List<Reward>(collectedRewards));
                }
                else
                {
                    Debug.LogWarning("[GameStateManager] SaveManager not found! Rewards not saved.");
                }
                
                foreach (var reward in collectedRewards)
                {
                    RewardDistributor.Instance.GiveReward(reward);
                    Debug.Log($"[GameStateManager] Distributed reward: {reward.GetDisplayText()}");
                }

                Debug.Log("[GameStateManager] Save completed successfully!");
                Debug.Log("========================");

                OnGameCompleted?.Invoke(new List<Reward>(collectedRewards));

                Debug.Log($"[GameStateManager] Player left with {collectedRewards.Count} rewards!");
            }

            Debug.Log("[GameStateManager] Restarting game...");
            RestartGame();
        }

        public void RestartGame()
        {
            StartGame();
        }

        public void AddReward(Reward reward)
        {
            collectedRewards.Add(reward);
            OnRewardAdded?.Invoke(reward);

            Debug.Log($"[GameStateManager] Reward added: {reward.GetDisplayText()} (Total: {collectedRewards.Count})");
        }

        public void ClearRewards()
        {
            int count = collectedRewards.Count;
            collectedRewards.Clear();
            OnRewardsCleared?.Invoke();

            Debug.Log($"[GameStateManager] Cleared {count} rewards!");
        }

        public List<Reward> GetCollectedRewards()
        {
            return new List<Reward>(collectedRewards);
        }

        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            OnStateChanged?.Invoke(newState);

            Debug.Log($"[GameStateManager] State changed to: {newState}");
        }

        public bool CanLeave()
        {
            return SpinController.Instance.State == SpinState.Idle &&
                   CurrentState == GameState.Playing &&
                   (ZoneManager.Instance.IsSafeZone() || ZoneManager.Instance.IsSuperZone());
        }
    }
}
