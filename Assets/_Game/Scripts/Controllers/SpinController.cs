using System;
using System.Collections;
using UnityEngine;
using WheelSystem;
using ZoneSystem;
using GameSystem;
using RewardSystem;
using BombSystem;

namespace SpinSystem
{
    public enum SpinState
    {
        Idle,
        Spinning,
        Stopped
    }

    public class SpinController : MonoBehaviour
    {
        public static SpinController Instance { get; private set; }

        public SpinState State { get; private set; } = SpinState.Idle;

        public event Action OnSpinStarted;
        public event Action<WheelSlice> OnSpinCompleted;
        public event Action OnSpinResultProcessed;

        [Header("Spin Settings")]
        [SerializeField] private float spinDuration = 2f;

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

        public bool CanSpin()
        {
            return State == SpinState.Idle &&
                   GameStateManager.Instance.CurrentState == GameState.Playing;
        }

        public void Spin()
        {
            if (!CanSpin())
            {
                Debug.LogWarning("[SpinController] Cannot spin right now!");
                return;
            }

            State = SpinState.Spinning;
            OnSpinStarted?.Invoke();

            Debug.Log("[SpinController] Spin started!");

            StartCoroutine(SimulateSpin());
        }

        private IEnumerator SimulateSpin()
        {
            yield return new WaitForSeconds(spinDuration);

            WheelSlice result = WheelManager.Instance.GetRandomSlice();

            State = SpinState.Stopped;
            OnSpinCompleted?.Invoke(result);

            Debug.Log($"[SpinController] Spin result: {result.SliceName} (Bomb: {result.IsBomb})");

        }

        public void ProcessReward(RewardSystem.Reward reward)
        {
            GameStateManager.Instance.AddReward(reward);
            Debug.Log($"[SpinController] Reward added: {reward.GetDisplayText()}");
            
            State = SpinState.Idle;
            OnSpinResultProcessed?.Invoke();
            Debug.Log("[SpinController] State changed to: Idle");
        }

        public void ProcessBomb()
        {
            BombHandler.Instance.HandleBombHit();
            State = SpinState.Idle;
            OnSpinResultProcessed?.Invoke();
            Debug.Log("[SpinController] Bomb handled, state changed to: Idle");
        }
    }
}
